namespace EasyDb.SecureStore
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    using EasyDb.Ext;
    using EasyDb.Interfaces;

    using Microsoft.Win32;

    using NLog;

    /// <summary>
    /// Store DB password secure
    /// </summary>
    public class PasswordStoreSecure : IPasswordStorage
    {
        /// <summary>
        /// Defines the RegistryKey
        /// </summary>
        private const string RegistryKey = "EasyDbStorage";

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordStoreSecure"/> class.
        /// </summary>
        public PasswordStoreSecure()
        {
            this._logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Store password secure
        /// </summary>
        /// <param name="strPwd">password secure string</param>
        /// <param name="datasourceId">datasource guid for password</param>
        public void StorePasswordSecure(SecureString strPwd, Guid datasourceId)
        {
            byte[] plaintext = strPwd.ToByteArray();

            // Generate additional entropy (will be used as the Initialization vector)
            byte[] entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            byte[] ciphertext = ProtectedData.Protect(plaintext, entropy, DataProtectionScope.CurrentUser);
            var mainKey = Registry.CurrentUser.CreateSubKey(RegistryKey);
            var pluginKey = mainKey?.CreateSubKey(datasourceId.ToString());
            pluginKey?.SetValue("entropy", entropy, RegistryValueKind.Binary);
            pluginKey?.SetValue("ciphertext", ciphertext, RegistryValueKind.Binary);
        }

        /// <summary>
        /// Try to get password from protected source
        /// </summary>
        /// <param name="str">Password secure string</param>
        /// <param name="guid">Module GUID</param>
        /// <returns>True if password extracted</returns>
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
                var entropy = (byte[])pluginKey.GetValue("entropy");
                var ciphertext = (byte[])pluginKey.GetValue("ciphertext");
                foreach (var c in Encoding.UTF8.GetChars(
                    ProtectedData.Unprotect(ciphertext, entropy, DataProtectionScope.CurrentUser)))
                {
                    str.AppendChar(c);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(new Exception($"Error during exract password for plugin: {guid}", ex));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Convert Secure string to string
        /// </summary>
        /// <param name="str">Secure string</param>
        /// <returns>Regular string</returns>
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
    }
}