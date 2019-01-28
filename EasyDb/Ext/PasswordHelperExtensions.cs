namespace EasyDb.Ext
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    /// <summary>
    /// Converts secure string to Byte array in encoding
    /// </summary>
    public static class PasswordHelperExtensions
    {
        /// <summary>
        /// The ToByteArray
        /// </summary>
        /// <param name="secureString">The secureString<see cref="SecureString"/></param>
        /// <param name="encoding">The encoding<see cref="Encoding"/></param>
        /// <returns>The />Converts secure string to byte array in specific encoding</returns>
        public static byte[] ToByteArray(this SecureString secureString, Encoding encoding = null)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }

            encoding = encoding ?? Encoding.UTF8;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);

                return encoding.GetBytes(Marshal.PtrToStringUni(unmanagedString));
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(unmanagedString);
                }
            }
        }
    }
}