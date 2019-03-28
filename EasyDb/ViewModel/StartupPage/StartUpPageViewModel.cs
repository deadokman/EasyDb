using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EasyDb.Localization;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.View.StartupPage;
using EasyDb.ViewModel.Interfaces;

namespace EasyDb.ViewModel.StartupPage
{
    /// <summary>
    /// Вью-модель стартовой страницы приложения
    /// </summary>
    public class StartUpPageViewModel : PaneBaseViewModel, IStartUpPageViewModel
    {
        private readonly IProjectManager projectManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageViewModel"/> class.
        /// Вью-модель стартовой страницы приложения
        /// </summary>
        public StartUpPageViewModel(IProjectManager projectManager)
            : base("Startup page")
        {
            this.projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
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