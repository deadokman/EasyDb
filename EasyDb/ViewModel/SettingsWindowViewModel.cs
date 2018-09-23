using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using EasyDb.ViewModel.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog;

namespace EasyDb.ViewModel
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private ILogger _logger;

        public SettingsWindowViewModel()
        {
            _logger = LogManager.GetCurrentClassLogger();
            SettignsCollection = new List<IProgramSettings>();
            SettignsCollection.Add(new GeneralSettingsViewModel());
            StoreCommand = new RelayCommand(() =>
            {
                foreach (var programSettings in SettignsCollection)
                {
                    programSettings.SaveSettings();
                }
            });

            ResetSettingsCommand = new RelayCommand(() =>
            {
                foreach (var programSettings in SettignsCollection)
                {
                    programSettings.ResetDefault();
                }
            });
        }

        /// <summary>
        /// Store command
        /// </summary>
        public ICommand StoreCommand { get; set; }

        /// <summary>
        /// Revert all changes without saving
        /// </summary>
        public ICommand ResetSettingsCommand { get; set; }

        /// <summary>
        /// Currently selected settings item
        /// </summary>
        public IProgramSettings SelectedSettingItem { get; set; }

        /// <summary>
        /// Collection of settings controls
        /// </summary>
        public IList<IProgramSettings> SettignsCollection { get; set; }


        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
