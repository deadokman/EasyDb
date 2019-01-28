using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using EasyDb.Ext;
using EasyDb.Interfaces;
using Microsoft.Win32;
using NLog;

namespace EasyDb.SecureStore
{
    /// <summary>
    /// Store DB password secure
    /// </summary>
    public class PasswordStoreSecure : IPasswordStorage
    {
        private const string RegistryKey = "EasyDbStorage";

        private ILogger _logger;

        public PasswordStoreSecure()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Unsecu
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UnsecureString(SecureString str)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Try to get password from protected source
        /// </summary>
        /// <param name="str">Password secure string</param>
        /// <param name="guid">Module GUID</param>
        /// <returns></returns>
        public bool TryGetPluginPassword(out SecureString str, Guid guid)
        {
            str = new SecureString();
            var mainKey = Registry.CurrentUser.OpenSubKey(RegistryKey);
            if (mainKey == null)
            {
                return false;
            }

            var pluginKey = mainKey.OpenSubKey(guid.ToString());
            if (pluginKey == null)
            {
                return false;
            }

            try
            {
                var entropy = (byte[]) pluginKey.GetValue("entropy");
                var ciphertext = (byte[]) pluginKey.GetValue("ciphertext");
                foreach (var c in Encoding.UTF8.GetChars(
                    ProtectedData.Unprotect(ciphertext, entropy, DataProtectionScope.CurrentUser)))
                {
                    str.AppendChar(c);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(new Exception($"Error during exract password for plugin: {guid}", ex));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Store password secure
        /// </summary>
        /// <param name="strPwd"></param>
        /// <param name="datasourceId"></param>
        public void StorePasswordSecure(SecureString strPwd, Guid datasourceId)
        {
            byte[] plaintext = strPwd.ToByteArray();
            // Generate additional entropy (will be used as the Initialization vector)
            byte[] entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            byte[] ciphertext = ProtectedData.Protect(plaintext, entropy,
                DataProtectionScope.CurrentUser);
            var mainKey = Registry.CurrentUser.CreateSubKey(RegistryKey);
            var pluginKey = mainKey?.CreateSubKey(datasourceId.ToString());
            pluginKey?.SetValue("entropy", entropy, RegistryValueKind.Binary);
            pluginKey?.SetValue("ciphertext", ciphertext, RegistryValueKind.Binary);
        }
    }
}
