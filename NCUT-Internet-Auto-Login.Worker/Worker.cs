using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace NCUT_Internet_Auto_Login.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        // 調整 HttpClient 以捕捉 Captive Portal 攔截
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false, // 不自動跳轉，便於抓取 Captive Portal 的 302 狀態
            ServerCertificateCustomValidationCallback = (s, c, ch, e) => true // Captive Portal 常用自簽憑證，需繞過驗證
        };
        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(15)
        };
    }

    private bool IsActualNetworkAvailable()
    {
        if (!NetworkInterface.GetIsNetworkAvailable()) return false;

        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // 只檢查實體有線網路或 Wi-Fi，且狀態為 Up
            if ((ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet || 
                 ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                ni.OperationalStatus == OperationalStatus.Up)
            {
                // 排除常見的虛擬網卡特徵
                string desc = ni.Description.ToLower();
                if (!desc.Contains("virtual") && 
                    !desc.Contains("vmware") &&
                    !desc.Contains("hyper-v"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("✔️ AutoLoginService 背景服務啟動中");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // 1. 確認本機實體/虛擬網卡是否啟用並已連線
                if (!IsActualNetworkAvailable())
                {
                    _logger.LogWarning("❌ 本機無任何啟動的網路連線，等待恢復...");
                    await Task.Delay(3000, stoppingToken);
                    continue;
                }

                // 2. 檢查是否被 Captive Portal 攔截
                var portalCheck = await CheckCaptivePortalAsync(stoppingToken);

                if (portalCheck.IsIntercepted)
                {
                    _logger.LogInformation("🛑 偵測到 Captive Portal，準備自動登入...");
                    await ExecuteLoginAsync(portalCheck.RedirectUrl, stoppingToken);
                    // 登入後給予網路設備緩衝時間
                    await Task.Delay(3000, stoppingToken);
                }
                else if (portalCheck.IsOnline)
                {
                    // 網路已暢通
                    _logger.LogInformation("✔️ 網路連線正常");
                    await Task.Delay(10000, stoppingToken); // 正常狀態下減少檢查頻率
                }
                else
                {
                    _logger.LogWarning("⚠️ 未知網路狀態，將於稍後重試");
                    await Task.Delay(3000, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "⚠️ 監控過程發現異常導致中斷");
                await Task.Delay(5000, stoppingToken); // 發生錯誤時減少 CPU
            }
        }

        _logger.LogInformation("✔️ AutoLoginService 背景服務已終止");
    }

    private class PortalCheckResult
    {
        public bool IsOnline { get; set; }
        public bool IsIntercepted { get; set; }
        public string RedirectUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// 利用 /generate_204 的特性來檢查 Captive Portal
    /// </summary>
    private async Task<PortalCheckResult> CheckCaptivePortalAsync(CancellationToken token)
    {
        var result = new PortalCheckResult();
        try
        {
            var response = await _httpClient.GetAsync("http://www.gstatic.com/generate_204", token);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) // 204
            {
                result.IsOnline = true;
                return result;
            }

            // 若 StatusCode 為 302 Found，或是其他轉址，代表被擋掉且要跳轉
            if (response.StatusCode == System.Net.HttpStatusCode.Found || 
                response.StatusCode == System.Net.HttpStatusCode.Redirect ||
                response.StatusCode == System.Net.HttpStatusCode.OK) // 有些防火牆直接回傳 200 帶有 JS 轉址
            {
                // 嘗試從 Header Location 取得轉址
                if (response.Headers.Location != null)
                {
                    result.RedirectUrl = response.Headers.Location.ToString();
                    result.IsIntercepted = true;
                    return result;
                }
                else
                {
                    // 若沒有 Location，便讀取 Content 看裡面是否有 window.location="" 的 JS 轉址
                    string content = await response.Content.ReadAsStringAsync(token);
                    var match = Regex.Match(content, @"window\.location(?:\.href)?\s*=\s*(?:""|')([^""']+)(?:""|')");
                    if (match.Success)
                    {
                        result.RedirectUrl = match.Groups[1].Value;
                        result.IsIntercepted = true;
                        return result;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"檢查 Portal 時發生錯誤 ({ex.Message})");
        }

        return result;
    }

    /// <summary>
    /// 根據轉址送出登入封包：先 GET 登入頁面解析表單，再用正確參數 POST
    /// </summary>
    private async Task ExecuteLoginAsync(string portalUrl, CancellationToken token)
    {
        if (string.IsNullOrEmpty(portalUrl))
        {
            _logger.LogWarning("登入目標不可為空，無法執行此次登入");
            return;
        }

        try
        {
            // 解析 Portal 基底資訊
            if (!Uri.TryCreate(portalUrl, UriKind.Absolute, out Uri? parsedUri))
            {
                _logger.LogWarning("⚠️ 無法解析 Portal URL: {PortalUrl}", portalUrl);
                return;
            }

            string baseUrl = $"{parsedUri.Scheme}://{parsedUri.Authority}";
            _logger.LogInformation("📡 正在 GET 登入頁面: {PortalUrl}", portalUrl);

            // 第一步：GET 登入頁面取得表單 HTML
            var getRequest = new HttpRequestMessage(HttpMethod.Get, portalUrl);
            var getResponse = await _httpClient.SendAsync(getRequest, token);

            // Captive Portal 可能回傳 302 到實際登入頁
            string loginPageUrl = portalUrl;
            if ((getResponse.StatusCode == System.Net.HttpStatusCode.Found ||
                 getResponse.StatusCode == System.Net.HttpStatusCode.Redirect) &&
                getResponse.Headers.Location != null)
            {
                loginPageUrl = getResponse.Headers.Location.IsAbsoluteUri
                    ? getResponse.Headers.Location.ToString()
                    : $"{baseUrl}{getResponse.Headers.Location}";
                _logger.LogInformation("↪️ 跟隨轉址至: {LoginPageUrl}", loginPageUrl);
                getRequest = new HttpRequestMessage(HttpMethod.Get, loginPageUrl);
                getResponse = await _httpClient.SendAsync(getRequest, token);
            }

            string pageHtml = await getResponse.Content.ReadAsStringAsync(token);

            // 第二步：解析表單中的 hidden fields
            string magic = ExtractHiddenField(pageHtml, "magic") ?? "";
            string fourTredir = ExtractHiddenField(pageHtml, "4Tredir") ?? "";
            string formAction = ExtractFormAction(pageHtml);

            // 若 HTML 解析失敗，改用 URL 推算作為備案
            if (string.IsNullOrEmpty(magic))
            {
                var magicMatch = Regex.Match(portalUrl, @"(?:magic=|/fgtauth\?)([^&""'\s]+)");
                if (magicMatch.Success) magic = magicMatch.Groups[1].Value;
            }
            if (string.IsNullOrEmpty(fourTredir))
            {
                fourTredir = "http://www.gstatic.com/generate_204";
            }

            _logger.LogInformation("解析登入參數 -> Magic: {Magic}, 4Tredir: {FourTredir}", 
                string.IsNullOrEmpty(magic) ? "未找到" : magic, fourTredir);

            // 第三步：建構 POST 目標 URL
            string targetUrl;
            if (!string.IsNullOrEmpty(formAction) && formAction != "/")
            {
                targetUrl = formAction.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? formAction
                    : $"{baseUrl}{formAction}";
            }
            else
            {
                // action="/" 表示 POST 到根路徑
                targetUrl = $"{baseUrl}/";
            }

            // 第四步：送出登入 POST 請求
            var appSettings = NCUT_Internet_Auto_Login.AppSettings.Load();
            string username = string.IsNullOrEmpty(appSettings.Username) ? "ncut" : appSettings.Username;
            string password = string.IsNullOrEmpty(appSettings.Password) ? "ncut" : appSettings.Password;

            var loginData = new Dictionary<string, string>
            {
                { "4Tredir", fourTredir },
                { "magic", magic },
                { "username", username },
                { "password", password }
            };

            using var content = new FormUrlEncodedContent(loginData);
            using var postRequest = new HttpRequestMessage(HttpMethod.Post, targetUrl);
            postRequest.Content = content;
            postRequest.Headers.Add("Referer", loginPageUrl);
            postRequest.Headers.Add("Origin", baseUrl);
            postRequest.Headers.Add("Upgrade-Insecure-Requests", "1");

            _logger.LogInformation("📤 POST 登入請求至: {TargetUrl}", targetUrl);
            var response = await _httpClient.SendAsync(postRequest, token);
            var responseContent = await response.Content.ReadAsStringAsync(token);

            // 檢查登入結果特徵
            if (responseContent.ToLower().Contains("keepalive"))
            {
                _logger.LogInformation("✔️ 登入請求成功，已繞過 Captive Portal");
            }
            else
            {
                _logger.LogInformation("🛑 登入請求已發送，但不確定是否成功，將稍後再次確認。");
            }
        }
        catch (Exception ex)
        {
            string detail = ex.InnerException != null ? $"{ex.Message} -> {ex.InnerException.Message}" : ex.Message;
            _logger.LogError("⚠️ 執行登入期間發生錯誤: {Detail}", detail);
        }
    }

    /// <summary>
    /// 從 HTML 中提取指定 hidden input 的 value
    /// </summary>
    private string? ExtractHiddenField(string html, string fieldName)
    {
        var pattern = $@"<input[^>]*name\s*=\s*""{Regex.Escape(fieldName)}""[^>]*value\s*=\s*""([^""]*)""";
        var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
        if (match.Success) return match.Groups[1].Value;

        // 嘗試 value 在 name 前面的順序
        pattern = $@"<input[^>]*value\s*=\s*""([^""]*)""[^>]*name\s*=\s*""{Regex.Escape(fieldName)}""";
        match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    /// <summary>
    /// 從 HTML 中提取 form 的 action 屬性
    /// </summary>
    private string ExtractFormAction(string html)
    {
        var match = Regex.Match(html, @"<form[^>]*action\s*=\s*""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : "/";
    }

    public override void Dispose()
    {
        _httpClient?.Dispose();
        base.Dispose();
    }
}
