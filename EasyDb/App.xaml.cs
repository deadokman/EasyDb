// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EasyDb.ProjectManagment;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ViewModel.DbExplorer;
using EasyDb.ViewModel.StartupPage;
using GalaSoft.MvvmLight.Messaging;
using NuGet;

namespace EasyDb
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    using Autofac;
    using Autofac.Extras.NLog;

    using CommonServiceLocator;
    using EasyDb.IoC;
    using EasyDb.SandboxEnvironment;
    using EasyDb.SecureStore;
    using EasyDb.ViewModel;
    using EasyDb.ViewModel.Choco;
    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.Interfaces;
    using EasyDb.ViewModel.Settings;

    using Edb.Environment;
    using Edb.Environment.Chocolatey;
    using Edb.Environment.Connection;
    using Edb.Environment.Interface;

    using MahApps.Metro;
    using MahApps.Metro.Controls.Dialogs;

    using NLog;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Defines the m_Languages
        /// </summary>
        private static List<CultureInfo> _mLanguages = new List<CultureInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            LanguageChanged += App_LanguageChanged;

            // AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            _mLanguages.Clear();
            _mLanguages.Add(new CultureInfo("en-US")); // Нейтральная культура для этого проекта
            _mLanguages.Add(new CultureInfo("ru-RU"));

            Language = EasyDb.Properties.Settings.Default.DefaultLanguage;
            var theme = ThemeManager.DetectAppStyle(this);
            SelectedAccent =
                ThemeManager.Accents.FirstOrDefault(af => EasyDb.Properties.Settings.Default.AccentTheme == af.Name)
                ?? theme.Item2;
            SelectedAppTheme =
                ThemeManager.AppThemes.FirstOrDefault(ac => EasyDb.Properties.Settings.Default.BaseTheme == ac.Name)
                ?? theme.Item1;
            ThemeManager.ChangeAppStyle(Current, SelectedAccent, SelectedAppTheme);
        }

        /// <summary>
        /// Defines the LanguageChanged
        /// </summary>
        public static event EventHandler LanguageChanged;

        /// <summary>
        /// Gets or sets the Language
        /// </summary>
        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture)
                {
                    return;
                }

                // 1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                // 2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "en-US":
                        dict.Source = new Uri(
                            string.Format("Localization/lang.{0}.xaml", value.Name),
                            UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Localization/lang.xaml", UriKind.Relative);
                        break;
                }

                // 3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null
                                                    && d.Source.OriginalString.StartsWith("Localization/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Current.Resources.MergedDictionaries.Add(dict);
                }

                // 4. Вызываем евент для оповещения всех окон.
                LanguageChanged(Current, new EventArgs());
            }
        }

        /// <summary>
        /// Gets the Languages
        /// </summary>
        public static List<CultureInfo> Languages
        {
            get
            {
                return _mLanguages;
            }
        }

        /// <summary>
        /// Gets or sets the SelectedAccent
        /// Current application accent color
        /// </summary>
        public static Accent SelectedAccent { get; set; }

        /// <summary>
        /// Gets or sets the SelectedAppTheme
        /// CurrentApplication theme
        /// </summary>
        public static AppTheme SelectedAppTheme { get; set; }

        /// <summary>
        /// Initialize IoC environment
        /// </summary>
        /// <param name="e">args</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceLocator.SetLocatorProvider(() => AutofacServiceLocator.Instance);
            var builder = AutofacServiceLocator.Instance.GetBuilder();
            builder.RegisterModule<NLogModule>();
            builder.RegisterType<DatasourceManager>().As<IDataSourceManager>().SingleInstance();
            builder.RegisterType<DialogCoordinator>().As<IDialogCoordinator>().SingleInstance();
            builder.RegisterType<ChocoController>().As<IChocolateyController>().SingleInstance();
            builder.RegisterType<PasswordStoreSecureWindows>().As<IPasswordStorage>().SingleInstance();
            builder.RegisterType<OdbcManager>().As<IOdbcManager>().SingleInstance();
            builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();
            builder.RegisterType<ConnectionManager>().As<IConnectionManager>().SingleInstance();
            builder.RegisterType<ApplicationEnvironment>().As<IApplicationEnvironment>().SingleInstance();

            // View models
            builder.RegisterType<MainViewModel>().As<MainViewModel>().SingleInstance();
            builder.RegisterType<GeneralSettingsViewModel>().As<GeneralSettingsViewModel>().SingleInstance();
            builder.RegisterType<SettingsWindowViewModel>().As<SettingsWindowViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().As<LoginViewModel>().SingleInstance();
            builder.RegisterType<ChocolateyInstallViewModel>().As<IChocolateyInstallViewModel>().SingleInstance();
            builder.RegisterType<OdbcManagerViewModel>().As<IOdbcManagerViewModel>().SingleInstance();
            builder.RegisterType<DbExplorerViewModel>().As<DbExplorerViewModel>().SingleInstance();
            builder.RegisterType<DatasourceSettingsViewModel>().As<IDataSourceSettingsViewModel>();
            builder.RegisterType<StartUpPageViewModel>().As<IStartUpPageViewModel>().As<StartUpPageViewModel>().SingleInstance();
            AutofacServiceLocator.Instance.ActivateIoc();
        }

        /// <summary>
        /// The App_LanguageChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="Object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void App_LanguageChanged(object sender, EventArgs e)
        {
            EasyDb.Properties.Settings.Default.DefaultLanguage = Language;
            EasyDb.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// The OnDispatcherUnhandledException
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DispatcherUnhandledExceptionEventArgs"/></param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var unhandedWindow = new UnhandledErrorWindow(e.Exception, MainWindow);
            unhandedWindow.ShowDialog();

            e.Handled = true;
        }
    }
}