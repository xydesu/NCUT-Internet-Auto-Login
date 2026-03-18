# NCUT Internet Auto Login

自動登入 NCUT（[國立勤益科技大學](https://zh.wikipedia.org/zh-tw/%E5%9C%8B%E7%AB%8B%E5%8B%A4%E7%9B%8A%E7%A7%91%E6%8A%80%E5%A4%A7%E5%AD%B8)）校園網的 Windows 應用程式。

## 簡介

本工具由兩個元件組成：

- **NCUT-Internet-Auto-Login.exe** — Windows Forms 圖形介面，讓使用者輸入帳號密碼、管理服務，並即時查看登入日誌。
- **NCUT-Internet-Auto-Login.Worker.exe** — .NET 9 Windows 背景服務，持續監控網路連線，偵測到 Captive Portal 時自動送出登入請求。

## 功能特點

- ✅ 偵測 Captive Portal 攔截並自動登入
- ✅ 以 Windows 服務形式在背景持續執行
- ✅ 現代化 ModernUI 圖形介面
- ✅ 即時日誌顯示
- ✅ 帳號密碼以 DPAPI 加密儲存於 `%ProgramData%\NCUT-Internet-Auto-Login\config.xml`
- ✅ 系統托盤支援（最小化到托盤、右鍵快速選單）
- ✅ 開機自動啟動（寫入 Windows 登錄檔）
- ✅ 後台啟動模式（開機時直接縮到托盤）
- ✅ 命令列參數 `-minimized` 支援

## 系統需求

- Windows 10 x64 或更新版本
- .NET Framework 4.7.2（GUI）
- .NET 9 Runtime（Worker 服務，已包含於安裝檔）

## 安裝

1. 至 [Releases](https://github.com/xydesu/NCUT-Internet-Auto-Login/releases) 下載最新的 `NCUT-Internet-Auto-Login-Setup.exe`。
2. 以**系統管理員**身分執行安裝程式（需要管理員權限以安裝 Windows 服務）。
3. 安裝完成後啟動 **NCUT Internet Auto Login**。
4. 輸入您的校園網帳號和密碼，點擊「**啟動服務**」。

## 使用說明

### 基本操作

1. 啟動應用程式。
2. 在介面中輸入校園網帳號和密碼。
3. 點擊「**啟動服務**」，背景服務開始持續監控網路連線。
4. 程式偵測到 Captive Portal 時會自動登入，並在日誌中顯示結果。
5. 點擊「**停止服務**」可停止監控。

### 系統托盤

- **最小化按鈕**：縮到系統托盤，不佔用任務欄。
- **關閉按鈕（X）**：隱藏到托盤而不結束程式。
- **雙擊托盤圖示**：重新顯示主視窗。
- **右鍵托盤圖示**：快速選單，包含顯示/隱藏視窗、啟動/停止服務、結束程式。

### 開機自動啟動

1. 勾選「**開機自動啟動**」，程式會在 Windows 登入後自動開啟。
2. 若同時勾選「**自啟動時在後台運行**」，開機後程式將直接最小化到系統托盤。

> 兩個選項互相關聯：取消「開機自動啟動」會自動停用「自啟動時在後台運行」。

### 命令列參數

| 參數 | 說明 |
|------|------|
| `-minimized` 或 `/minimized` | 啟動時直接最小化到系統托盤 |

```
NCUT-Internet-Auto-Login.exe -minimized
```

## 技術架構

| 元件 | 技術 |
|------|------|
| GUI | .NET Framework 4.7.2 + Windows Forms + 自訂 ModernUI |
| Worker 服務 | .NET 9 + `Microsoft.Extensions.Hosting.WindowsServices` |
| 打包（GUI） | Costura.Fody 6.0.0（單一 EXE） |
| 打包（Worker） | 自包含單一 EXE（win-x64） |
| 安裝程式 | Inno Setup |
| 設定儲存 | XML + DPAPI 加密 |

### Worker 服務登入流程

1. 定期向 `http://www.gstatic.com/generate_204` 發出 HTTP 請求。
2. 若收到 302/200（而非預期的 204），判定為 Captive Portal 攔截。
3. GET 登入頁面，解析 `magic`、`4Tredir` 等 hidden 欄位。
4. POST 登入表單，驗證回應中是否包含 `keepalive` 關鍵字。

### 設定檔位置

```
%ProgramData%\NCUT-Internet-Auto-Login\config.xml
```

密碼以 Windows DPAPI（`ProtectedData`）加密後存入 `EncryptedPassword` 欄位。

### 登錄檔項目

| 路徑 | 鍵值 | 說明 |
|------|------|------|
| `HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run` | `!_NCUT_AutoLogin` | 開機自動啟動路徑 |
| `HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run` | `NCUT_StartMinimized` | 後台啟動標記（值為 `1`） |

## 建置

### 前置需求

- Visual Studio 2022（含 .NET Framework 4.7.2 和 .NET 9 工作負載）
- NuGet 套件還原

### GUI 專案（NCUT-Internet-Auto-Login）

```powershell
# 在 NCUT-Internet-Auto-Login 目錄下執行
.\Build-Release.ps1
# 輸出：bin\Release\NCUT-Internet-Auto-Login.exe
```

或在 Visual Studio 中切換到 **Release** 模式後按 `Ctrl+Shift+B`。

### Worker 專案（NCUT-Internet-Auto-Login.Worker）

```powershell
dotnet publish NCUT-Internet-Auto-Login.Worker\NCUT-Internet-Auto-Login.Worker.csproj `
  -c Release -r win-x64 --self-contained true `
  -p:PublishSingleFile=true -o publish
```

### 安裝程式（Inno Setup）

```powershell
ISCC /dMyAppVersion=1.0.0 installer\setup.iss
# 輸出：installer\dist\NCUT-Internet-Auto-Login-Setup.exe
```

## 作者

- 原始 Python 版本：sangege & AI LIFE  
- C# 版本及現代化 UI：GitHub Copilot

## 原始專案

本專案 Fork 自 **apple050620312** 的原始版本：

> <https://github.com/apple050620312/NCUT-Internet-Auto-Login>

感謝原作者的貢獻。

## 相關連結

- 首頁 / Issues：<https://github.com/xydesu/NCUT-Internet-Auto-Login>
- Releases：<https://github.com/xydesu/NCUT-Internet-Auto-Login/releases>
- 原始專案：<https://github.com/apple050620312/NCUT-Internet-Auto-Login>
