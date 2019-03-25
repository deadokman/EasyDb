namespace EasyDb.ViewModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using EasyDb.ViewModel.Settings;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using NLog;

    /// <summary>
    /// Defines the <see cref="SettingsWindowViewModel" />
    /// </summary>
    public class SettingsWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Defines the _logger
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowViewModel"/> class.
        /// </summary>
        public SettingsWindowViewModel()
        {
            _logger = LogManager.GetCurrentClassLogger();
            SettignsCollection = new List<IProgramSettings>();
            SettignsCollection.Add(new GeneralSettingsViewModel());
            StoreCommand = new RelayCommand(
                () =>
                    {
                        foreach (var programSettings in SettignsCollection)
                        {
                            programSettings.SaveSettings();
                        }
                    });

            ResetSettingsCommand = new RelayCommand(
                () =>
                    {
                        foreach (var programSettings in SettignsCollection)
                        {
                            programSettings.ResetDefault();
                        }
                    });
        }

        /// <summary>
        /// Gets or sets the ResetSettingsCommand
        /// Revert all changes without saving
        /// </summary>
        public ICommand ResetSettingsCommand { get; set; }

        /// <summary>
        /// Gets or sets the SelectedSettingItem
        /// Currently selected settings item
        /// </summary>
        public IProgramSettings SelectedSettingItem { get; set; }

        /// <summary>
        /// Gets or sets the SettignsCollection
        /// Collection of settings controls
        /// </summary>
        public IList<IProgramSettings> SettignsCollection { get; set; }

        /// <summary>
        /// Gets or sets the StoreCommand
        /// Store command
        /// </summary>
        public ICommand StoreCommand { get; set; }

        /// <summary>
        /// The Cleanup
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}