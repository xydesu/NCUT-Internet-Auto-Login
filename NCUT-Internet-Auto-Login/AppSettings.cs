using System;
using System.IO;
using System.Xml.Serialization;

namespace NCUT_Internet_Auto_Login
{
    public class AppSettings
    {
        public string Username { get; set; } = "ncut";
        public string EncryptedPassword { get; set; } = "";

        // 不將原始密碼寫入持久化
        [XmlIgnore]
        public string Password
        {
            get => SecurityHelper.DecryptString(EncryptedPassword);
            set => EncryptedPassword = SecurityHelper.EncryptString(value);
        }

        private static string GetConfigPath()
        {
            // 改為使用 CommonApplicationData (即 %ProgramData%)，讓系統服務(LocalSystem)與一般使用者能共用此設定檔
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(appData, "NCUT-Internet-Auto-Login", "config.xml");
        }

        public static AppSettings Load()
        {
            string path = GetConfigPath();
            if (File.Exists(path))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        return (AppSettings)serializer.Deserialize(fs);
                    }
                }
                catch
                {
                    // 忽略讀取錯誤，直接回傳新實例
                }
            }
            return new AppSettings();
        }

        public void Save()
        {
            string path = GetConfigPath();
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(fs, this);
                }
            }
            catch
            {
                // 忽略寫入錯誤
            }
        }
    }
}
