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
            this.AccentColors = ThemeManager.Accents.Select(
                a => new AccentColorMenuData()
                         {
                             Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as SolidColorBrush
                         }).ToList();
            this.AppThemes = ThemeManager.AppThemes.Select(
                a => new AppThemeMenuData()
                         {
                             Name = a.Name,
                             BorderColorBrush = a.Resources["BlackColorBrush"] as SolidColorBrush,
                             ColorBrush = a.Resources["WhiteColorBrush"] as SolidColorBrush
                         }).ToList();
            this.LoadDefaults();
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
                return this._accentColors;
            }

            set
            {
                this._accentColors = value;
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
                return this._name;
            }

            set
            {
                this._name = value;
                RaisePropertyChanged(() => this.Name);
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
                return this._pluginsPath;
            }

            set
            {
                this._pluginsPath = value;
                RaisePropertyChanged(() => this.PluginsPath);
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
                return this._selectedAccent;
            }

            set
            {
                this._selectedAccent = value;
                RaisePropertyChanged(() => this.SelectedAccent);
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
                return this._selectedAppTheme;
            }

            set
            {
                this._selectedAppTheme = value;
                RaisePropertyChanged(() => this.SelectedAppTheme);
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
                return this._selectedLang;
            }

            set
            {
                this._selectedLang = value;
                RaisePropertyChanged(() => this.SelectedLang);
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
                return this._settingsWidth;
            }

            set
            {
                this._settingsWidth = value;
                RaisePropertyChanged(() => this.SttingsWidthValue);
            }
        }

        /// <summary>
        /// The Cleanup
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
            App.LanguageChanged -= this.AppOnLanguageChanged;
        }

        /// <summary>
        /// The ResetDefault
        /// </summary>
        public void ResetDefault()
        {
            this.LoadDefaults();
            this.SaveSettings();
        }

        /// <summary>
        /// The SaveSettings
        /// </summary>
        public void SaveSettings()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            if (this._selectedAccent != null && this._selectedAccent.Name != theme.Item2.Name)
            {
                this.ApplyTheme(
                    this._selectedAccent,
                    () => Properties.Settings.Default.AccentTheme = this._selectedAccent.Name);
            }

            if (this._selectedAppTheme != null && this._selectedAppTheme.Name != theme.Item1.Name)
            {
                this.ApplyTheme(
                    this._selectedAppTheme,
                    () => Properties.Settings.Default.BaseTheme = this._selectedAppTheme.Name);
            }

            App.Language = this._selectedLang;
            Properties.Settings.Default.SettingsFlyoutWidthLimiter = this._settingsWidth;
            this.InvalidateMainWindowWidth();
            if (Properties.Settings.Default.PluginsPath != this._pluginsPath)
            {
                Properties.Settings.Default.PluginsPath = this._pluginsPath;

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
            this.Name = (string)Application.Current.Resources["gs_General"];
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
            this.SelectedAppTheme = this.AppThemes.FirstOrDefault(at => at.Name == App.SelectedAppTheme.Name);
            this.SelectedAccent = this.AccentColors.FirstOrDefault(ac => ac.Name == App.SelectedAccent.Name);
            this.SttingsWidthValue = Properties.Settings.Default.SettingsFlyoutWidthLimiter;
            this.PluginsPath = Properties.Settings.Default.PluginsPath;
            this.PluginsPath = Path.GetFullPath(this.PluginsPath);
            this.DisplayControl = new GeneralSettringsPage();
            this.DisplayControl.DataContext = this;
            this.SelectedLang = App.Language;
            App.LanguageChanged += this.AppOnLanguageChanged;
            this.Name = (string)Application.Current.Resources["gs_General"];

            // Реакция на команду выбора папки плагинов
            this.SetPathCommand = new RelayCommand(
                () =>
                    {
                        using (var dlg = new FolderBrowserDialog())
                        {
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.PluginsPath = Path.GetFullPath(dlg.SelectedPath);
                            }

                            // DatasourceManager.Instance.ReloadPlugins();
                        }
                    });
        }
    }
}