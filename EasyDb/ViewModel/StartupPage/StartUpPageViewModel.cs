﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using EasyDb.Commands;
using EasyDb.Localization;
using EasyDb.ProjectManagment.Annotations;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.View.StartupPage;
using EasyDb.ViewModel.Interfaces;
using Edb.Environment.Interface;

namespace EasyDb.ViewModel.StartupPage
{
    /// <summary>
    /// Вью-модель стартовой страницы приложения
    /// </summary>
    public class StartUpPageViewModel : PaneBaseViewModel, IStartUpPageViewModel
    {
        private readonly IProjectEnvironment _projectEnvironment;
        private readonly IDataSourceManager _datasourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageViewModel"/> class.
        /// Вью-модель стартовой страницы приложения
        /// </summary>
        /// <param name="projectEnvironment">Project manager instance</param>
        /// <param name="datasourceManager">Datasource manager</param>
        public StartUpPageViewModel(IProjectEnvironment projectEnvironment, [NotNull] IDataSourceManager datasourceManager)
            : base("Startup page")
        {
            _projectEnvironment = projectEnvironment ?? throw new ArgumentNullException(nameof(projectEnvironment));
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
            string res = App.Current.TryFindResource(ResourceKeynames.StartupPageKey) as string;
            if (res != null)
            {
                Title = res;
            }

            CreateEmptyProj = new EDbCommand(() =>
            {
                _projectEnvironment.InitializeNewProject();
            });
        }

        /// <summary>
        /// User controll instance
        /// </summary>
        public override UserControl ViewInstance => new StartUpPageControll();

        /// <summary>
        /// Create empty project
        /// </summary>
        public ICommand CreateEmptyProj { get; set; }
    }
}