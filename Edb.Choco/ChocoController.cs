// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChocoController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the <see cref="ChocoController" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Edb.Environment
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    using Edb.Environment.Interface;

    /// <summary>
    /// Defines the <see cref="ChocoController" />
    /// </summary>
    public class ChocoController : IChocolateyController
    {
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
        /// Initializes a new instance of the <see cref="ChocoController"/> class.
        /// </summary>
        public ChocoController()
        {
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
        /// Powershell script run
        /// </summa ry>
        /// <param name="script">
        /// Script text
        /// </param>
        /// <returns>
        /// Execution results
        /// The <see cref="string"/>.
        /// </returns>
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
            catch (Exception objException)
            {
                // Log the exception
            }
        }
    }
}
