#!/bin/sh
# NCUT Multi-WAN Auto Login Script for OpenWrt
# Author: sangege

# ==============================================================================
# 【帳號密碼設定區】
# 針對不同的網路介面 (如 wan, lan1, lan2) 設定對應的帳號密碼
# ==============================================================================
# 預設帳號密碼 (當找不到對應介面的專屬設定時使用)
DEFAULT_ACCOUNT="s3b332038"
DEFAULT_PASSWORD="B123841421"

# 各介面專屬帳號密碼 (請將 YOUR_ACCOUNT_XX 替換為實際帳號)
ACCOUNT_wan="s3b332038"
PASSWORD_wan="B123841421"

ACCOUNT_wan2="s3b434006"
PASSWORD_wan2="N127061849"

ACCOUNT_wan3="s3b441114"
PASSWORD_wan3="E126116357"

ACCOUNT_wan4="s3b417072"
PASSWORD_wan4="H126465545"
# ==============================================================================

IFACE=$1
if [ -z "$IFACE" ]; then
    echo "Usage: $0 <interface>"
    echo "Example: $0 lan1"
    exit 1
fi

# 動態讀取對應介面的帳號密碼
eval ACCOUNT=\$"ACCOUNT_$IFACE"
eval PASSWORD=\$"PASSWORD_$IFACE"

# 若該介面未設定專屬帳號密碼，則使用預設值
if [ -z "$ACCOUNT" ] || [ -z "$PASSWORD" ]; then
    ACCOUNT=$DEFAULT_ACCOUNT
    PASSWORD=$DEFAULT_PASSWORD
fi

COOKIE_JAR="/tmp/ncut_cookie_${IFACE}.txt"
TMP_HTML="/tmp/ncut_check_${IFACE}.html"

log() {
    logger -t "ncut-autologin[$IFACE]" "$1"
    echo "[$IFACE] $1"
}

# 透過 ubus 取得真正的實體介面名稱 (L3 Device)
L3_DEV=$(ubus call network.interface."$IFACE" status 2>/dev/null | grep -o '"l3_device": *"[^"]*"' | awk -F'"' '{print $4}')
if [ -z "$L3_DEV" ]; then
    L3_DEV=$(ubus call network.interface."$IFACE" status 2>/dev/null | grep -o '"device": *"[^"]*"' | awk -F'"' '{print $4}')
fi

# 如果仍然找不到，做個 fallback
if [ -z "$L3_DEV" ]; then
    L3_DEV="$IFACE"
fi

log "啟動自動登入，邏輯介面: $IFACE, 實體介面: $L3_DEV"

# ARP 隔離 (防止多孔同網段造成的丟包)
if [ -d "/proc/sys/net/ipv4/conf/$L3_DEV" ]; then
    sysctl -w net.ipv4.conf."$L3_DEV".arp_ignore=1 >/dev/null 2>&1
    sysctl -w net.ipv4.conf."$L3_DEV".arp_announce=2 >/dev/null 2>&1
    log "已套用 ARP 隔離優化 (arp_ignore=1, arp_announce=2)"
else
    log "無法套用 ARP 隔離，找不到介面 $L3_DEV 的 sysctl 設定"
fi

# 冷啟動緩衝與 Heartbeat 熱機
log "執行冷啟動緩衝，等待網路介面穩定 (4 秒)..."
sleep 4

# 使用強制綁定介面的 curl 指令
CURL_CMD="curl --interface $L3_DEV"

is_system_network_connected() {
    # 檢查該邏輯介面在系統中是否已經成功 UP (取得 IP)
    local up_status=$(ubus call network.interface."$IFACE" status 2>/dev/null | grep '"up": true')
    if [ -n "$up_status" ]; then
        return 0
    else
        return 1
    fi
}

check_login_status() {
    res=$($CURL_CMD -s -k -L -w "%{http_code}|%{url_effective}" "http://www.gstatic.com/generate_204" -o "$TMP_HTML")
    http_code=$(echo "$res" | cut -d'|' -f1)
    url_eff=$(echo "$res" | cut -d'|' -f2)
    
    if [ "$http_code" = "204" ]; then
        echo "ONLINE"
        return 0
    fi
    
    if grep -qiE "fgtauth|勤益科技大學" "$TMP_HTML" 2>/dev/null || echo "$url_eff" | grep -qi "fgtauth"; then
        echo "NEEDS_LOGIN"
        return 0
    fi
    
    echo "UNSTABLE"
}

login() {
    rm -f "$COOKIE_JAR"
    
    res=$($CURL_CMD -s -k -L -c "$COOKIE_JAR" -w "%{http_code}|%{url_effective}" "http://www.gstatic.com/generate_204" -o "$TMP_HTML")
    url_eff=$(echo "$res" | cut -d'|' -f2)
    
    # 優先從網頁內容抓取 JS 跳轉 (window.location = "https://...")
    redirect_url=$(grep -oE "window\.location[[:space:]]*=[[:space:]]*[\"'][^\"']+[\"']" "$TMP_HTML" 2>/dev/null | sed -E "s/.*[\"'](.*)[\"'].*/\1/" | head -n1)
    
    # [額外容錯] 若 Fortinet 直接給了 302 HTTP 跳轉，擷取最終目標網址
    if [ -z "$redirect_url" ] && echo "$url_eff" | grep -qi "fgtauth"; then
        redirect_url="$url_eff"
    fi
    
    if [ -z "$redirect_url" ]; then
        log "登入異常: 無法從頁面解析重新導向網址(Redirect URL)。"
        return 1
    fi
    
    gateway_ip=$(echo "$redirect_url" | grep -oE "https?://[^/:]+" | sed 's|https://||' | sed 's|http://||')
    if [ -z "$gateway_ip" ]; then
        log "登入異常: 無法從重新導向網址解析閘道(Gateway)。"
        return 1
    fi
    
    magic=$(echo "$redirect_url" | grep -oE "fgtauth\?[^&]+" | cut -d'?' -f2)
    if [ -z "$magic" ]; then
        log "登入異常: 無法提取認證 magic 參數。"
        return 1
    fi
    
    # 這是極度關鍵的一步！必須先用 GET 訪問一次跳轉網址，防火牆才會正式初始化這個 magic session！
    $CURL_CMD -s -k -L -c "$COOKIE_JAR" -b "$COOKIE_JAR" "$redirect_url" -o /dev/null
    
    log "狀態: 正在向閘道器 $gateway_ip 發送認證請求..."
    
    post_res=$($CURL_CMD -s -k -L -b "$COOKIE_JAR" \
        -w "%{http_code}" \
        -H "Content-Type: application/x-www-form-urlencoded" \
        -H "Upgrade-Insecure-Requests: 1" \
        -H "Referer: $redirect_url" \
        -H "Origin: http://$gateway_ip:1000" \
        --data-urlencode "4Tredir=http://www.gstatic.com/generate_204" \
        --data-urlencode "magic=$magic" \
        --data-urlencode "username=$ACCOUNT" \
        --data-urlencode "password=$PASSWORD" \
        "http://$gateway_ip:1000/" -o "$TMP_HTML")
        
    if grep -qFi "/keepalive?" "$TMP_HTML" 2>/dev/null; then
        log "登入成功: 已成功完成校園網路認證！"
    else
        log "登入異常: 登入請求完成，但未偵測到成功標記。HTTP Code: $post_res"
    fi
}

# --- Main Loop ---
if ! command -v curl >/dev/null 2>&1; then
    log "錯誤: 系統缺少 curl 指令，腳本無法運行。請執行 opkg install curl。"
    exit 1
fi

last_state="INITIAL"

while true; do
    if ! is_system_network_connected; then
        if [ "$last_state" != "SYSTEM_OFFLINE" ]; then
            log "等待: 介面尚未取得 IP 或建立路由，等待連線恢復..."
            last_state="SYSTEM_OFFLINE"
        fi
        sleep 3
        continue
    fi
    
    status=$(check_login_status)
    
    if [ "$status" = "ONLINE" ]; then
        if [ "$last_state" != "ONLINE" ]; then
            log "連線正常: 已經在網路登入狀態！"
            last_state="ONLINE"
        fi
        sleep 5
        
    elif [ "$status" = "NEEDS_LOGIN" ]; then
        if [ "$last_state" != "NEEDS_LOGIN" ]; then
            log "未登入狀態: 準備執行自動登入程序..."
            last_state="NEEDS_LOGIN"
        fi
        login
        sleep 2
        
    elif [ "$status" = "UNSTABLE" ]; then
        if [ "$last_state" != "UNSTABLE" ]; then
            log "警告: 已連接但無法存取外網或找到認證頁。這通常代表網路異常。"
            last_state="UNSTABLE"
        fi
        sleep 3
    else
        sleep 3
    fi
done
