using System;
using System.Security;

namespace EasyDb.Interfaces
{
    /// <summary>
    /// Interface for password storage
    /// </summary>
    public interface IPasswordStorage
    {
        /// <summary>
        /// Store password
        /// </summary>
        /// <param name="strPwd">Password string</param>
        /// <param name="datasourceId">ID for password datasource</param>
        /// <returns>Password secure string</returns>
        void StorePasswordSecure(SecureString strPwd, Guid datasourceId);

        /// <summary>
        /// Returns secure string from Registry
        /// </summary>
        /// <param name="str">Result string</param>
        /// <param name="guid">Datasource ID</param>
        /// <returns>return true if password exists for ID</returns>
        bool TryGetPluginPassword(out SecureString str, Guid guid);
    }
}
