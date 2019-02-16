using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ViewModel.Choco
{
    using System.Windows.Input;

    /// <summary>
    /// Interface for Chocolatey install dialog
    /// </summary>
    public interface IChocolateyInstallViewModel
    {
        /// <summary>
        /// Gets
        /// </summary>
        bool HideDialog { get; set; }

        /// <summary>
        /// Application running in administrative mode
        /// </summary>
        bool IsInAdministrativeMode { get; }

        /// <summary>
        /// Application running NOT in administrative mode
        /// </summary>
        bool IsNotInAdministrativeMode { get; }

        /// <summary>
        /// Close application command
        /// </summary>
        ICommand CloseApplication { get; }

        /// <summary>
        /// Close dialog command
        /// </summary>
        ICommand CloseDialog { get; }

        /// <summary>
        /// Install chocolatey
        /// </summary>
        ICommand InstallChocolateyCommand { get; }

        /// <summary>
        /// Navigate to URL
        /// </summary>
        ICommand NavigateLink { get; }

        /// <summary>
        /// Installation was coompleted Successfully
        /// </summary>
        bool InstallationCompletedSuccessfully { get; set; }

        /// <summary>
        /// There are was one or more error while installation
        /// </summary>
        bool InstallationHasErrors { get; set; }

        /// <summary>
        /// Errors in installation
        /// </summary>
        string InstallErrors { get; set; }
    }
}
