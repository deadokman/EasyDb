// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChocoController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the <see cref="ChocoController" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Edb.Environment.Chocolatey
{
    using chocolatey;
    using chocolatey.infrastructure.app.domain;
    using chocolatey.infrastructure.logging;
    using chocolatey.infrastructure.results;
    using Edb.Environment.ChocolateyGui;
    using Edb.Environment.Interface;
    using Edb.Environment.Model;
    using Microsoft.VisualStudio.Threading;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using HttpClient = System.Net.Http.HttpClient;
    using LogLevel = NLog.LogLevel;

    /// <summary>
    /// Defines the <see cref="ChocoController" />
    /// </summary>
    public class ChocoController : IChocolateyController
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private readonly Autofac.Extras.NLog.ILogger logger;

        /// <summary>
        /// Defines the Lock
        /// </summary>
        private static readonly AsyncReaderWriterLock Lock = new AsyncReaderWriterLock();

        /// <summary>
        /// Powershell script default name
        /// </summary>
        public const string PowershellScriptName = "install.ps1";

        /// <summary>
        /// Defines the CohocoInstallPath
        /// </summary>
        private const string CohocoInstallPath = @"C:\ProgramData\chocolatey";

        /// <summary>
        /// URL to powershell installation
        /// </summary>
        private const string DownloadPowershellUrl = @"https://chocolatey.org/install.ps1";

        /// <summary>
        /// Chocolatey install
        /// </summary>
        private string command = @"@powershell -NoProfile -ExecutionPolicy Bypass -Command ""iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin";

        /// <summary>
        /// Defines the _loggerWrapper
        /// </summary>
        private ChocolateyLoggerWrapper _loggerWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocoController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="Autofac.Extras.NLog.ILogger"/></param>
        public ChocoController(Autofac.Extras.NLog.ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggerWrapper = new ChocolateyLoggerWrapper(logger);
        }

        /// <summary>
        /// Download powershell installation script
        /// </summary>
        /// <returns>Path to powershell </returns>
        public async Task<string> DownloadAndStorePowershellInstall()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(DownloadPowershellUrl);
            }
        }

        /// <summary>
        /// Get information about chocolatey package
        /// </summary>
        /// <param name="packageId">The packageId<see cref="string"/></param>
        /// <returns>The <see cref="Task{PackageResult}"/></returns>
        public async Task<PackageResult> GetPackageInformation(string packageId)
        {
            var choco = Lets.GetChocolatey().SetCustomLogging(_loggerWrapper);
            choco = choco.Set(
                config =>
                    {
                        config.CommandName = "list";
                        config.Input = packageId;
                        config.ListCommand.Exact = true;
                        config.QuietOutput = true;
                        config.RegularOutput = false;
                    });

            return await choco.ListAsync<PackageResult>().ContinueWith(r => r.Result.FirstOrDefault());
        }

        /// <summary>
        /// Register choco message listner
        /// </summary>
        /// <param name="listner">listner</param>
        public void RegisterLisner(IChocoMessageListner listner)
        {
            this._loggerWrapper.RegisterListner(listner);
        }

        /// <summary>
        /// Unregister choco message listner
        /// </summary>
        /// <param name="listner">listner</param>
        public void UnregisterLisner(IChocoMessageListner listner)
        {
            this._loggerWrapper.UnregisterListner(listner);
        }

        /// <summary>
        /// The Runpowershell
        /// </summary>
        /// <param name="script">The script<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public Task<string> Runpowershell(string script)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        // create Powershell runspace
                        using (var runspace = RunspaceFactory.CreateRunspace())
                        {
                            runspace.Open();
                            var pipeline = runspace.CreatePipeline();
                            pipeline.Commands.AddScript(script);
                            pipeline.Commands.Add("Out-String");

                            // execute the script
                            var results = pipeline.Invoke();
                            var stringBuilder = new StringBuilder();
                            if (pipeline.HadErrors)
                            {
                                foreach (var err in (Collection<ErrorRecord>)pipeline.Error.Read())
                                {
                                    stringBuilder.Append(err);
                                    throw new Exception(stringBuilder.ToString());
                                }
                            }

                            foreach (PSObject obj in results)
                            {
                                stringBuilder.AppendLine(obj.ToString());
                            }

                            runspace.Close();
                            return stringBuilder.ToString();
                        }
                    });
        }

        /// <summary>
        /// Returns true if choco installed
        /// </summary>
        /// <returns>true if installed</returns>
        public bool ValidateChocoInstall()
        {
            return Directory.Exists(CohocoInstallPath);
        }

        /// <summary>
        /// Check that application runs in administrator mode
        /// </summary>
        /// <returns>true if is administrator</returns>
        public bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                .IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Install package from chocolatey
        /// </summary>
        /// <param name="id">Package ID</param>
        /// <param name="version">Version</param>
        /// <param name="source">URL</param>
        /// <param name="force">Force install</param>
        /// <returns></returns>
        public async Task<PackageOperationResult> InstallPackage(
            string id,
            string version = null,
            Uri source = null,
            bool force = false)
        {
            using (await Lock.WriteLockAsync())
            {
                var choco = Lets.GetChocolatey().SetCustomLogging(this._loggerWrapper);
                choco.Set(
                    config =>
                    {
                        config.CommandName = CommandNameType.install.ToString();
                        config.PackageNames = id;
                        config.Features.UsePackageExitCodes = false;

                        if (version != null)
                        {
                            config.Version = version.ToString();
                        }

                        if (source != null)
                        {
                            config.Sources = source.ToString();
                        }

                        if (force)
                        {
                            config.Force = true;
                        }
                    });

                Action<LogMessage> grabErrors;
                var errors = GetErrors(out grabErrors);

                using (this._loggerWrapper.Intercept(grabErrors))
                {
                    await choco.RunAsync();
                    if (Environment.ExitCode != 0)
                    {
                        Environment.ExitCode = 0;
                        return new PackageOperationResult { Successful = false, Messages = errors.ToArray() };
                    }

                    return PackageOperationResult.SuccessfulCached;
                }
            }
        }

        /// <summary>
        /// Execure chocolatey installation
        /// </summary>
        /// <param name="progressReportFunc">Install report func</param>
        public void ExecuteChocoInstall(Func<string> progressReportFunc)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                var procStartInfo = new ProcessStartInfo("cmd", "/c " + this.command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;

                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();

                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// The GetErrors
        /// </summary>
        /// <param name="grabErrors">The grabErrors<see cref="Action{LogMessage}"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        private static List<string> GetErrors(out Action<LogMessage> grabErrors)
        {
            var errors = new List<string>();
            grabErrors = m =>
            {
                switch (m.LogLevel)
                {
                    case LogLevelType.Warning:
                    case LogLevelType.Error:
                    case LogLevelType.Fatal:
                        errors.Add(m.Message);
                        break;
                }
            };

            return errors;
        }
    }
}
