using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EasyDb.ViewModel.Settings
{
    public interface IProgramSettings
    {
        /// <summary>
        /// Plugin name for settings window
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin display control
        /// </summary>
        UserControl DisplayControl { get; }

        /// <summary>
        /// Save plugin settings
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Reset default setrings
        /// </summary>
        void ResetDefault();
    }
}
