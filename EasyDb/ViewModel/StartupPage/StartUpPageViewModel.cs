using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private readonly IApplicationEnvironment _applicationEnvironment;
        private readonly IDataSourceManager _datasourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageViewModel"/> class.
        /// Вью-модель стартовой страницы приложения
        /// </summary>
        /// <param name="applicationEnvironment">Project manager instance</param>
        /// <param name="datasourceManager">Datasource manager</param>
        public StartUpPageViewModel(IApplicationEnvironment applicationEnvironment, [NotNull] IDataSourceManager datasourceManager)
            : base("Startup page")
        {
            this._applicationEnvironment = applicationEnvironment ?? throw new ArgumentNullException(nameof(applicationEnvironment));
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
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