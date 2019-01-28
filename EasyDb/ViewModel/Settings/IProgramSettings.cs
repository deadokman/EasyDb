namespace EasyDb.ViewModel.Settings
{
    using System.Windows.Controls;

    /// <summary>
    /// Defines the <see cref="IProgramSettings" />
    /// </summary>
    public interface IProgramSettings
    {
        /// <summary>
        /// Gets the DisplayControl
        /// Plugin display control
        /// </summary>
        UserControl DisplayControl { get; }

        /// <summary>
        /// Gets the Name
        /// Plugin name for settings window
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Reset default setrings
        /// </summary>
        void ResetDefault();

        /// <summary>
        /// Save plugin settings
        /// </summary>
        void SaveSettings();
    }
}