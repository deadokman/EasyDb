﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EasyDb.Localization;
using EasyDb.View.StartupPage;
using EasyDb.ViewModel.Interfaces;

namespace EasyDb.ViewModel.StartupPage
{
    /// <summary>
    /// Вью-модель стартовой страницы приложения
    /// </summary>
    public class StartUpPageViewModel : PaneBaseViewModel, IStartUpPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageViewModel"/> class.
        /// Вью-модель стартовой страницы приложения
        /// </summary>
        public StartUpPageViewModel()
            : base("Startup page")
        {
            string res = App.Current.TryFindResource(ResourceKeynames.StartupPageKey) as string;
            if (res != null)
            {
                Title = res;
            }
        }

        /// <summary>
        /// User controll instance
        /// </summary>
        public override UserControl ViewInstance => new StartUpPageControll();
    }
}