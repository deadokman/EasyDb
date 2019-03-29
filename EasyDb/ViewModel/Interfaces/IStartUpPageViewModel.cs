using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
    }
}
