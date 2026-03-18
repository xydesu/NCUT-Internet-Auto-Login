using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NCUT_Internet_Auto_Login
{
    internal static class UpdateChecker
    {
        private const string ApiUrl =
            "https://api.github.com/repos/xydesu/NCUT-Internet-Auto-Login/releases/latest";

        public const string ReleasesUrl =
            "https://github.com/xydesu/NCUT-Internet-Auto-Login/releases/latest";

        private const string InstallerAssetName = "NCUT-Internet-Auto-Login-Setup.exe";

        // Reuse a single HttpClient instance to avoid socket exhaustion
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        static UpdateChecker()
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "NCUT-Internet-Auto-Login");
        }

        public static Version CurrentVersion =>
            Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Checks GitHub Releases for a newer version.
        /// Returns <see cref="UpdateInfo.NoUpdate"/> on failure or when up-to-date.
        /// </summary>
        public static async Task<UpdateInfo> CheckAsync()
        {
            try
            {
                string json = await _http.GetStringAsync(ApiUrl).ConfigureAwait(false);

                // Extract tag_name, e.g. "v1.2.3"
                var tagMatch = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
                if (!tagMatch.Success)
                    return UpdateInfo.NoUpdate;

                string tagName = tagMatch.Groups[1].Value;
                string versionString = tagName.TrimStart('v');

                if (!Version.TryParse(versionString, out Version latestVersion))
                    return UpdateInfo.NoUpdate;

                if (latestVersion <= CurrentVersion)
                    return UpdateInfo.NoUpdate;

                // Find the installer asset's download URL
                string downloadUrl = string.Empty;

                // Look for the named installer asset first
                var assetMatch = Regex.Match(
                    json,
                    "\"name\"\\s*:\\s*\"" + Regex.Escape(InstallerAssetName) +
                    "\".*?\"browser_download_url\"\\s*:\\s*\"([^\"]+)\"",
                    RegexOptions.Singleline);

                if (assetMatch.Success)
                {
                    downloadUrl = assetMatch.Groups[1].Value;
                }
                else
                {
                    // Fall back to the first .exe asset in the release
                    var fallback = Regex.Match(
                        json,
                        "\"browser_download_url\"\\s*:\\s*\"([^\"]+\\.exe)\"");
                    if (fallback.Success)
                        downloadUrl = fallback.Groups[1].Value;
                }

                return new UpdateInfo(true, latestVersion, tagName, downloadUrl);
            }
            catch
            {
                return UpdateInfo.NoUpdate;
            }
        }

        /// <summary>
        /// Downloads the installer from <paramref name="downloadUrl"/> to a temp file
        /// and launches it silently (Inno Setup /SILENT flag).
        /// </summary>
        public static async Task DownloadAndInstallAsync(string downloadUrl)
        {
            string tempPath = Path.Combine(
                Path.GetTempPath(),
                InstallerAssetName);

            byte[] data = await _http.GetByteArrayAsync(downloadUrl).ConfigureAwait(false);
            File.WriteAllBytes(tempPath, data);

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = tempPath,
                    Arguments = "/SILENT",
                    UseShellExecute = true
                });
        }
    }

    internal class UpdateInfo
    {
        public static readonly UpdateInfo NoUpdate =
            new UpdateInfo(false, null, null, null);

        public bool IsUpdateAvailable { get; }
        public Version LatestVersion { get; }
        public string TagName { get; }
        public string DownloadUrl { get; }

        public UpdateInfo(bool isUpdateAvailable, Version latestVersion,
            string tagName, string downloadUrl)
        {
            IsUpdateAvailable = isUpdateAvailable;
            LatestVersion = latestVersion;
            TagName = tagName;
            DownloadUrl = downloadUrl;
        }
    }
}
