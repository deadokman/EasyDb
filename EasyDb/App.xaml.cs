using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro;

namespace EasyDb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        /// <summary>
        /// Current application accent color
        /// </summary>
        public static Accent SelectedAccent { get; set; }

        /// <summary>
        /// CurrentApplication theme
        /// </summary>
        public static AppTheme SelectedAppTheme { get; set; }



        public App()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;
           // AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US")); //Нейтральная культура для этого проекта
            m_Languages.Add(new CultureInfo("ru-RU"));

            Language = EasyDb.Properties.Settings.Default.DefaultLanguage;
            var theme = ThemeManager.DetectAppStyle(this);
            SelectedAccent = ThemeManager.Accents.FirstOrDefault(af => EasyDb.Properties.Settings.Default.AccentTheme == af.Name) ?? theme.Item2;
            SelectedAppTheme = ThemeManager.AppThemes.FirstOrDefault(ac => EasyDb.Properties.Settings.Default.BaseTheme == ac.Name) ?? theme.Item1;
            ThemeManager.ChangeAppStyle(Current, SelectedAccent, SelectedAppTheme);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var unhandedWindow = new EasyDb.UnhandledErrorWindow(e.Exception, MainWindow);
            unhandedWindow.ShowDialog();

            e.Handled = true;
        }

        //Евент для оповещения всех окон приложения
        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "en-US":
                        dict.Source = new Uri(String.Format("Localization/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Localization/lang.xaml", UriKind.Relative);
                        break;
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Localization/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                //4. Вызываем евент для оповещения всех окон.
                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            EasyDb.Properties.Settings.Default.DefaultLanguage = Language;
            EasyDb.Properties.Settings.Default.Save();
        }
    }
}
