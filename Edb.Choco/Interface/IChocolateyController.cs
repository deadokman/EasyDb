// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChocolateyController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the <see cref="IChocolateyController" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Edb.Environment.Model;

namespace Edb.Environment.Interface
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IChocolateyController" />
    /// </summary>
    public interface IChocolateyController
    {
        /// <summary>
        /// The ExecuteChocoInstall
        /// </summary>
        /// <param name="progressReportFunc">The progressReportFunc<see cref="Func{string}"/></param>
        void ExecuteChocoInstall(Func<string> progressReportFunc);

        /// <summary>
        /// The ValidateChocoInstall
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        bool ValidateChocoInstall();

        /// <summary>
        /// Check that application runs in administrator mode
        /// </summary>
        /// <returns>true if is administrator</returns>
        bool IsAdministrator();

        /// <summary>
        /// Download powershell installation script
        /// </summary>
        /// <returns>Path to powershell </returns>
        Task<string> DownloadAndStorePowershellInstall();

        /// <summary>
        /// Powershell script run
        /// </summary>
        /// <param name="script">
        /// Script text
        /// </param>
        /// <returns>
        /// Execution results
        /// The <see cref="string"/>.
        /// </returns>
        Task<string> Runpowershell(string script);

        /// <summary>
        /// Install package from chocolatey
        /// </summary>
        /// <param name="id">Package ID</param>
        /// <param name="version">Version</param>
        /// <param name="source">URL</param>
        /// <param name="force">Force install</param>
        /// <returns></returns>
        Task<PackageOperationResult> InstallPackage(string id, string version = null, Uri source = null, bool force = false);
    }
}
