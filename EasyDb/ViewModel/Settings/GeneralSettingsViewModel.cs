using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using CSGO.Trader;
using CSGO.Trader.ViewModel.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro;
using Application = System.Windows.Application;
using GeneralSettringsPage = EasyDb.CustomControls.GeneralSettringsPage;
using UserControl = System.Windows.Controls.UserControl;

namespace EasyDb.ViewModel.Settings
{
    /// <summary>
    /// General application settings
    /// Основные настройки приложения
    /// </summary>
    public class GeneralSettingsViewModel : ViewModelBase, IProgramSettings
    {
        #region Backing flields

        private AccentColorMenuData _selectedAccent;
        private List<AccentColorMenuData> _accentColors;
        private AppThemeMenuData _selectedAppTheme;
        private double _settingsWidth;
        private CultureInfo _selectedLang;
        private string _name;
        private string _pluginsPath;

        #endregion

        /// <summary>
        /// Команда вызова окна выбора папки
        /// </summary>
        public ICommand SetPathCommand { get; set; }

        public override void Cleanup()
        {
            base.Cleanup();
            App.LanguageChanged -= AppOnLanguageChanged;
        }

        /// <summary>
        /// Avalilable accent colors
        /// Цвета акцента
        /// </summary>
        public List<AccentColorMenuData> AccentColors
        {
            get { return _accentColors; }
            set { _accentColors = value; }
        }

        /// <summary>
        /// Avalilable base application themes
        /// Основные темы
        /// </summary>
        public List<AppThemeMenuData> AppThemes { get; set; }

        /// <summary>
        /// Languages
        /// </summary>
        public List<CultureInfo> AvailableLanguages { get { return App.Languages; } }

        private void ApplyTheme(AccentColorMenuData data, Action setSettings)
        {
            data.Apply();
            setSettings.Invoke();
        }

        /// <summary>
        /// Plugins directory
        /// </summary>
        public string PluginsPath
        {
            get { return _pluginsPath; }
            set
            {
                _pluginsPath = value;
                RaisePropertyChanged(() => PluginsPath);
            }
        }

        /// <summary>
        /// Flyout Width setting
        /// </summary>
        public double SttingsWidthValue
        {
            get { return _settingsWidth; }
            set
            {
                _settingsWidth = value;
                RaisePropertyChanged(() => SttingsWidthValue);
            }
        }

        /// <summary>
        /// Selected lang
        /// </summary>
        public CultureInfo SelectedLang
        {
            get { return _selectedLang; }
            set
            {
                _selectedLang = value;
                RaisePropertyChanged(() => SelectedLang);
            }
        }

        /// <summary>
        /// Selected accent
        /// Выбраная тема акциента
        /// </summary>
        public AccentColorMenuData SelectedAccent
        {
            get { return _selectedAccent; }
            set
            {
                _selectedAccent = value;
                RaisePropertyChanged(() => SelectedAccent);
            }
        }

        /// <summary>
        /// Select base application theme
        /// Выбраная базовая тема
        /// </summary>
        public AppThemeMenuData SelectedAppTheme
        {
            get { return _selectedAppTheme; }
            set
            {
                _selectedAppTheme = value;
                RaisePropertyChanged(() => SelectedAppTheme);
            }
        }

        public void ResetDefault()
        {
            LoadDefaults();
            SaveSettings();
        }

        public void SaveSettings()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            if (_selectedAccent != null && _selectedAccent.Name != theme.Item2.Name)
            {
                ApplyTheme(_selectedAccent, () => Properties.Settings.Default.AccentTheme = _selectedAccent.Name);
            }

            if (_selectedAppTheme != null && _selectedAppTheme.Name != theme.Item1.Name)
            {
                ApplyTheme(_selectedAppTheme, () => Properties.Settings.Default.BaseTheme = _selectedAppTheme.Name);
            }

            App.Language = _selectedLang;
            Properties.Settings.Default.SettingsFlyoutWidthLimiter = _settingsWidth;
            InvalidateMainWindowWidth();
            if (Properties.Settings.Default.PluginsPath != _pluginsPath)
            {
                Properties.Settings.Default.PluginsPath = _pluginsPath;
                //DatasourceManager.Instance.ReloadPlugins();
            }

            Properties.Settings.Default.Save();
        }

        private void InvalidateMainWindowWidth()
        {
            //hack
            App.Current.MainWindow.Width = App.Current.MainWindow.Width - 1;
            App.Current.MainWindow.Width = App.Current.MainWindow.Width + 1;
            //Raise window width changed (interface update)
            //Does not work
            //App.Current.MainWindow.InvalidateProperty(MetroWindow.WidthProperty);
            //does not work
            //BindingOperations.GetBindingExpression(b, Button.ContentProperty).UpdateTarget();

        }

        public GeneralSettingsViewModel()
        {
            AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as SolidColorBrush })
                                            .ToList();
            AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as SolidColorBrush, ColorBrush = a.Resources["WhiteColorBrush"] as SolidColorBrush })
                                           .ToList();
            LoadDefaults();
        }

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
            //Реакция на команду выбора папки плагинов
            SetPathCommand = new RelayCommand(() =>
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

        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
        {
            Name = (string)Application.Current.Resources["gs_General"];
        }

     //   public MarketPlacePlugin Owner { get { return null; } }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public UserControl DisplayControl { get; private set; }
    }
}
