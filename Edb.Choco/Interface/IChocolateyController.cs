// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChocolateyController.cs" company="SimpleExample">
//   Controlls chocolatey packages
// </copyright>
// <summary>
//   Defines the
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Edb.Environment.Interface
{
    using System;
    using System.Threading.Tasks;
    using chocolatey.infrastructure.results;
    using Edb.Environment.Model;

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
        /// <param name="version">package Version</param>
        /// <param name="source">URL to packages source</param>
        /// <param name="force">Force install</param>
        /// <returns>Result of package operation</returns>
        Task<PackageOperationResult> InstallPackage(string id, string version = null, Uri source = null, bool force = false);


        /// <summary>
        /// Get information about chocolatey package
        /// </summary>
        /// <param name="packageId">Choco package Id</param>
        /// <returns>Result of package information extraction</returns>
        Task<PackageResult> GetPackageInformation(string packageId);

        /// <summary>
        /// Register choco message listner
        /// </summary>
        /// <param name="listner">Cohoclatey message listner</param>
        void RegisterLisner(IChocoMessageListner listner);

        /// <summary>
        /// Unregister choco message listner
        /// </summary>
        /// <param name="listner">Cohoclatey message listner</param>
        void UnregisterLisner(IChocoMessageListner listner);
    }
}
