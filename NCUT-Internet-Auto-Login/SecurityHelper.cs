using System;
using System.Security.Cryptography;
using System.Text;

namespace NCUT_Internet_Auto_Login
{
    public static class SecurityHelper
    {
        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);
                return Convert.ToBase64String(encryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                byte[] data = Convert.FromBase64String(cipherText);
                byte[] decryptedData = ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
