using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EasyDb.Commands;
using EasyDb.Localization;
using EasyDb.ProjectManagment;
using EasyDb.ProjectManagment.Annotations;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ProjectManagment.Eventing;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.View.StartupPage;
using EasyDb.ViewModel.Interfaces;
using Edb.Environment.CommunicationArgs;
using Edb.Environment.DatasourceManager;
using Edb.Environment.Interface;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;

namespace EasyDb.ViewModel.StartupPage
{
    /// <summary>
    /// Вью-модель стартовой страницы приложения
    /// </summary>
    public class StartUpPageViewModel : PaneBaseViewModel, IStartUpPageViewModel
    {
        private readonly IProjectEnvironment _projectEnvironment;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IDataSourceManager _datasourceManager;
        private readonly IMessenger _messenger;
        private IEnumerable<SupportedSourceItem> _supportedDatasources;
        private ICollectionView _collectionItemsView;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageViewModel"/> class.
        /// Вью-модель стартовой страницы приложения
        /// </summary>
        /// <param name="projectEnvironment">Project manager instance</param>
        /// <param name="dialogCoordinator">Dialog coordinator</param>
        /// <param name="datasourceManager">Datasource manager</param>
        /// <param name="messanger">MVVM event messanger</param>
        public StartUpPageViewModel(
            [NotNull] IProjectEnvironment projectEnvironment,
            [NotNull] IDialogCoordinator dialogCoordinator,
            [NotNull] IDataSourceManager datasourceManager,
            [NotNull] IMessenger messanger)
            : base("Startup page")
        {
            _projectEnvironment = projectEnvironment ?? throw new ArgumentNullException(nameof(projectEnvironment));
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
            _messenger = messanger ?? throw new ArgumentNullException(nameof(messanger));
            _messenger.Register<ProjectEnvironmentInitialized>(
                this,
                (pi) =>
                {
                    FillProjectsHistory(_projectEnvironment.HistoryInformation);
                });

            string res = App.Current.TryFindResource(ResourceKeynames.StartupPageKey) as string;
            if (res != null)
            {
                Title = res;
            }

            SupportedDatasources = datasourceManager.SupportedDatasources;
            _messenger.Register<DatasourcesInitialized>(this, (ds) =>
                {
                    SupportedDatasources = ds.SupportedSources;
                });

            CreateEmptyProj = new EDbCommand(() =>
                {
                    InvokeWithProjectDialog(() => { _projectEnvironment.InitializeNewProject(); });
                });

            PinCommand = new EDbCommand<ProjectHistoryViewModelItem>((ph) =>
            {
                ph.IsPinned = !ph.IsPinned;
                SetHistoryGroup(ph);
                _projectEnvironment.StoreHistoryInformation();
                ProjectHistoryCollectionView.Refresh();
            });

            HistoryClickCommand = new EDbCommand<ProjectHistoryViewModelItem>((ph) =>
            {
                ph.LastAccess = DateTime.Today.Date;
                _projectEnvironment.StoreHistoryInformation();
                LoadProjectFromPath(ph.FolderPath);
            });
        }

        /// <summary>
        /// Project history items
        /// </summary>
        public ICollectionView ProjectHistoryCollectionView
        {
            get => _collectionItemsView;
            set
            {
                _collectionItemsView = value;
                RaisePropertyChanged(() => ProjectHistoryCollectionView);
            }
        }

        /// <summary>
        /// Pin history command
        /// </summary>
        public ICommand PinCommand { get; set; }

        /// <summary>
        /// Click on history item
        /// </summary>
        public ICommand HistoryClickCommand { get; set; }

        /// <summary>
        /// The collection of the supported datasource items
        /// </summary>
        public IEnumerable<SupportedSourceItem> SupportedDatasources
        {
            get => _supportedDatasources;
            set
            {
                _supportedDatasources = value;
                RaisePropertyChanged(() => SupportedDatasources);
            }
        }

        /// <summary>
        /// User controll instance
        /// </summary>
        public override UserControl ViewInstance => new StartUpPageControll();

        /// <summary>
        /// Create empty project
        /// </summary>
        public ICommand CreateEmptyProj { get; set; }

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public override void Cleanup()
        {
            SupportedDatasources = null;
            _messenger.Unregister<DatasourcesInitialized>(this);
            base.Cleanup();
        }

        private async void LoadProjectFromPath(string pathToProjectFolder)
        {
            var res = await InvokeWithProjectDialog(() =>
                {
                    return _projectEnvironment.TryLoadProject(pathToProjectFolder);
                });

            if (!res.ProjectFileLocatedSuccessfully || !res.ProjectFileLocatedSuccessfully)
            {
                throw new Exception(res.ExceptionMessage ?? "Not found project");
            }
        }

        private void SetHistoryGroup(ProjectHistoryViewModelItem phvmi)
        {
            var today = DateTime.Today;
            var startOfWeek = DateTime.Today.AddDays(-1 * ((int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1));
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            if (phvmi.HistItem.Pinned)
            {
                phvmi.GroupName = Application.Current.Resources[ResourceKeynames.StpGroupPinnedKey].ToString();
                return;
            }

            if (phvmi.HistItem.LastAccess.Date == today.Date)
            {
                phvmi.GroupName = Application.Current.Resources[ResourceKeynames.StpGroupTodayKey].ToString();
                return;
            }

            if (phvmi.HistItem.LastAccess.Date >= startOfWeek.Date && phvmi.HistItem.LastAccess.Date < today.Date)
            {
                phvmi.GroupName = Application.Current.Resources[ResourceKeynames.StpGroupLastWeekKey].ToString();
                return;
            }

            if (phvmi.HistItem.LastAccess.Date >= startOfMonth.Date && phvmi.HistItem.LastAccess.Date < startOfWeek.Date)
            {
                phvmi.GroupName = Application.Current.Resources[ResourceKeynames.StpGroupLastMonthKey].ToString();
                return;
            }

            if (phvmi.HistItem.LastAccess.Date < startOfMonth.Date)
            {
                phvmi.GroupName = Application.Current.Resources[ResourceKeynames.StpGroupOlderKey].ToString();
            }
        }

        private async void InvokeWithProjectDialog(Action invoke)
        {
            var title = App.Current.Resources[ResourceKeynames.StpOpeningProjectDialogTitleKey].ToString();
            var message = string.Format(App.Current.Resources[ResourceKeynames.StpOpeningProjectDialogMessageKey].ToString(), _projectEnvironment.CurrentProject?.ProjName);
            var ctrl = await _dialogCoordinator.ShowProgressAsync(this, title, message, false);
            ctrl.SetIndeterminate();
            try
            {
                invoke.Invoke();
                await ctrl.CloseAsync();
            }
            catch (Exception)
            {
                await ctrl.CloseAsync();
            }
        }

        private async Task<T> InvokeWithProjectDialog<T>(Func<Task<T>> invoke)
        {
            var title = App.Current.Resources[ResourceKeynames.StpOpeningProjectDialogTitleKey].ToString();
            var message = string.Format(App.Current.Resources[ResourceKeynames.StpOpeningProjectDialogMessageKey].ToString(), _projectEnvironment.CurrentProject?.ProjName);
            var ctrl = await _dialogCoordinator.ShowProgressAsync(this, title, message, false);
            ctrl.SetIndeterminate();
            try
            {
                var result = await invoke.Invoke();
                await ctrl.CloseAsync();
                return result;
            }
            catch (Exception)
            {
                await ctrl.CloseAsync();
                return default(T);
            }
        }

        /// <summary>
        /// Fill projects History information
        /// </summary>
        /// <param name="histItems">Projects history container</param>
        private void FillProjectsHistory(HistoryInformation histItems)
        {
            if (histItems?.ProjectsHistory == null)
            {
                return;
            }

            var viewModelItems = histItems.ProjectsHistory
                .Select(phi => new ProjectHistoryViewModelItem(phi)).ToList();

            foreach (var phvmi in viewModelItems)
            {
                SetHistoryGroup(phvmi);
            }

            var view = CollectionViewSource.GetDefaultView(viewModelItems);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ProjectHistoryViewModelItem.GroupName)));
            view.SortDescriptions.Add(new SortDescription(nameof(ProjectHistoryViewModelItem.LastAccess), ListSortDirection.Descending));
            ProjectHistoryCollectionView = view;
        }
    }
}