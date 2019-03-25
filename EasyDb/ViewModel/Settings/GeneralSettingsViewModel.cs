namespace EasyDb.ViewModel.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;

    using CSGO.Trader.ViewModel.Settings;

    using EasyDb.CustomControls;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MahApps.Metro;

    using Application = System.Windows.Application;
    using UserControl = System.Windows.Controls.UserControl;

    /// <summary>
    /// General application settings
    /// Основные настройки приложения
    /// </summary>
    public class GeneralSettingsViewModel : ViewModelBase, IProgramSettings
    {
        /// <summary>
        /// Defines the _accentColors
        /// </summary>
        private List<AccentColorMenuData> _accentColors;

        /// <summary>
        /// Defines the _name
        /// </summary>
        private string _name;

        /// <summary>
        /// Defines the _pluginsPath
        /// </summary>
        private string _pluginsPath;

        /// <summary>
        /// Defines the _selectedAccent
        /// </summary>
        private AccentColorMenuData _selectedAccent;

        /// <summary>
        /// Defines the _selectedAppTheme
        /// </summary>
        private AppThemeMenuData _selectedAppTheme;

        /// <summary>
        /// Defines the _selectedLang
        /// </summary>
        private CultureInfo _selectedLang;

        /// <summary>
        /// Defines the _settingsWidth
        /// </summary>
        private double _settingsWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralSettingsViewModel"/> class.
        /// </summary>
        public GeneralSettingsViewModel()
        {
            AccentColors = ThemeManager.Accents.Select(
                a => new AccentColorMenuData()
                         {
                             Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as SolidColorBrush
                         }).ToList();
            AppThemes = ThemeManager.AppThemes.Select(
                a => new AppThemeMenuData()
                         {
                             Name = a.Name,
                             BorderColorBrush = a.Resources["BlackColorBrush"] as SolidColorBrush,
                             ColorBrush = a.Resources["WhiteColorBrush"] as SolidColorBrush
                         }).ToList();
            LoadDefaults();
        }

        /// <summary>
        /// Gets or sets the AccentColors
        /// Avalilable accent colors
        /// Цвета акцента
        /// </summary>
        public List<AccentColorMenuData> AccentColors
        {
            get
            {
                return _accentColors;
            }

            set
            {
                _accentColors = value;
            }
        }

        /// <summary>
        /// Gets or sets the AppThemes
        /// Avalilable base application themes
        /// Основные темы
        /// </summary>
        public List<AppThemeMenuData> AppThemes { get; set; }

        /// <summary>
        /// Gets the AvailableLanguages
        /// Languages
        /// </summary>
        public List<CultureInfo> AvailableLanguages
        {
            get
            {
                return App.Languages;
            }
        }

        /// <summary>
        /// Gets the DisplayControl
        /// </summary>
        public UserControl DisplayControl { get; private set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// Gets or sets the PluginsPath
        /// Plugins directory
        /// </summary>
        public string PluginsPath
        {
            get
            {
                return _pluginsPath;
            }

            set
            {
                _pluginsPath = value;
                RaisePropertyChanged(() => PluginsPath);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedAccent
        /// Selected accent
        /// Выбраная тема акциента
        /// </summary>
        public AccentColorMenuData SelectedAccent
        {
            get
            {
                return _selectedAccent;
            }

            set
            {
                _selectedAccent = value;
                RaisePropertyChanged(() => SelectedAccent);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedAppTheme
        /// Select base application theme
        /// Выбраная базовая тема
        /// </summary>
        public AppThemeMenuData SelectedAppTheme
        {
            get
            {
                return _selectedAppTheme;
            }

            set
            {
                _selectedAppTheme = value;
                RaisePropertyChanged(() => SelectedAppTheme);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedLang
        /// Selected lang
        /// </summary>
        public CultureInfo SelectedLang
        {
            get
            {
                return _selectedLang;
            }

            set
            {
                _selectedLang = value;
                RaisePropertyChanged(() => SelectedLang);
            }
        }

        /// <summary>
        /// Gets or sets the SetPathCommand
        /// Команда вызова окна выбора папки
        /// </summary>
        public ICommand SetPathCommand { get; set; }

        /// <summary>
        /// Gets or sets the SttingsWidthValue
        /// Flyout Width setting
        /// </summary>
        public double SttingsWidthValue
        {
            get
            {
                return _settingsWidth;
            }

            set
            {
                _settingsWidth = value;
                RaisePropertyChanged(() => SttingsWidthValue);
            }
        }

        /// <summary>
        /// The Cleanup
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
            App.LanguageChanged -= AppOnLanguageChanged;
        }

        /// <summary>
        /// The ResetDefault
        /// </summary>
        public void ResetDefault()
        {
            LoadDefaults();
            SaveSettings();
        }

        /// <summary>
        /// The SaveSettings
        /// </summary>
        public void SaveSettings()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            if (_selectedAccent != null && _selectedAccent.Name != theme.Item2.Name)
            {
                ApplyTheme(
                    _selectedAccent,
                    () => Properties.Settings.Default.AccentTheme = _selectedAccent.Name);
            }

            if (_selectedAppTheme != null && _selectedAppTheme.Name != theme.Item1.Name)
            {
                ApplyTheme(
                    _selectedAppTheme,
                    () => Properties.Settings.Default.BaseTheme = _selectedAppTheme.Name);
            }

            App.Language = _selectedLang;
            Properties.Settings.Default.SettingsFlyoutWidthLimiter = _settingsWidth;
            InvalidateMainWindowWidth();
            if (Properties.Settings.Default.PluginsPath != _pluginsPath)
            {
                Properties.Settings.Default.PluginsPath = _pluginsPath;

                // DatasourceManager.Instance.ReloadPlugins();
            }

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// The ApplyTheme
        /// </summary>
        /// <param name="data">The data<see cref="AccentColorMenuData"/></param>
        /// <param name="setSettings">The setSettings<see cref="Action"/></param>
        private void ApplyTheme(AccentColorMenuData data, Action setSettings)
        {
            data.Apply();
            setSettings.Invoke();
        }

        /// <summary>
        /// The AppOnLanguageChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
        {
            Name = (string)Application.Current.Resources["gs_General"];
        }

        /// <summary>
        /// The InvalidateMainWindowWidth
        /// </summary>
        private void InvalidateMainWindowWidth()
        {
            // hack
            Application.Current.MainWindow.Width = Application.Current.MainWindow.Width - 1;
            Application.Current.MainWindow.Width = Application.Current.MainWindow.Width + 1;
        }

        /// <summary>
        /// The LoadDefaults
        /// </summary>
        private void LoadDefaults()
        {
            SelectedAppTheme = AppThemes.FirstOrDefault(at => at.Name == App.SelectedAppTheme.Name);
            SelectedAccent = AccentColors.FirstOrDefault(ac => ac.Name == App.SelectedAccent.Name);
            SttingsWidthValue = Properties.Settings.Default.SettingsFlyoutWidthLimiter;
            PluginsPath = Properties.Settings.Default.PluginsPath;
            PluginsPath = Path.GetFullPath(PluginsPath);
            DisplayControl = new GeneralSettringsPage();
            DisplayControl.DataContext = this;
            SelectedLang = App.Language;
            App.LanguageChanged += AppOnLanguageChanged;
            Name = (string)Application.Current.Resources["gs_General"];

            // Реакция на команду выбора папки плагинов
            SetPathCommand = new RelayCommand(
                () =>
                    {
                        using (var dlg = new FolderBrowserDialog())
                        {
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                PluginsPath = Path.GetFullPath(dlg.SelectedPath);
                            }

                            // DatasourceManager.Instance.ReloadPlugins();
                        }
                    });
        }
    }
}