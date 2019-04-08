using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ViewModel.StartupPage;
using Edb.Environment.DatasourceManager;

namespace EasyDb.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс
    /// </summary>
    public interface IStartUpPageViewModel
    {
        /// <summary>
        /// Create empty project
        /// </summary>
        ICommand CreateEmptyProj { get; set; }

        /// <summary>
        /// The list of supported source items
        /// </summary>
        IEnumerable<SupportedSourceItem> SupportedDatasources { get; }

        /// <summary>
        /// Collection of project gistory items
        /// </summary>
        ICollectionView ProjectHistoryCollectionView { get; set; }

        /// <summary>
        /// Pin history command
        /// </summary>
        ICommand PinCommand { get; set; }

        /// <summary>
        /// Click on history item
        /// </summary>
        ICommand HistoryClickCommand { get; set; }

        /// <summary>
        /// Открыть новый проект
        /// </summary>
        ICommand OpenProjectCmd { get; set; }
    }
}
