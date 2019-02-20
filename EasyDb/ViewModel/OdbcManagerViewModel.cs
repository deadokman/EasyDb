using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ViewModel
{
    using EasyDb.Annotations;
    using EasyDb.ViewModel.Interfaces;

    using Edb.Environment.Interface;
    using Edb.Environment.Model;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// ODBC manager view model
    /// </summary>
    public class OdbcManagerViewModel : ViewModelBase, IOdbcManagerViewModel
    {
        private readonly IOdbcRepository _odbcRepository;

        private IEnumerable<OdbcDriver> _odbcDrivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcManagerViewModel"/> class.
        /// </summary>
        /// <param name="odbcRepository">Odbc drivers repository instance</param>
        public OdbcManagerViewModel([NotNull] IOdbcRepository odbcRepository)
        {
            this._odbcRepository = odbcRepository ?? throw new ArgumentNullException(nameof(odbcRepository));
            OdbcDrivers = this._odbcRepository.ListOdbcDrivers();
        }

        /// <summary>
        /// List Odbc drivers
        /// </summary>
        public IEnumerable<OdbcDriver> OdbcDrivers
        {
            get => this._odbcDrivers;
            set
            {
                this._odbcDrivers = value;
                this.RaisePropertyChanged(() => OdbcDrivers);
            }
        }
    }
}
