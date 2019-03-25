using System;
using System.Collections.Generic;

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
        private IEnumerable<OdbcDriver> _odbcDrivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcManagerViewModel"/> class.
        /// </summary>
        /// <param name="odbcManager">Odbc drivers repository instance</param>
        public OdbcManagerViewModel([NotNull] IOdbcManager odbcManager)
        {
            odbcManager = odbcManager ?? throw new ArgumentNullException(nameof(odbcManager));
            OdbcDrivers = odbcManager.ListOdbcDrivers();
        }

        /// <summary>
        /// List Odbc drivers
        /// </summary>
        public IEnumerable<OdbcDriver> OdbcDrivers
        {
            get => _odbcDrivers;
            set
            {
                _odbcDrivers = value;
                RaisePropertyChanged(() => OdbcDrivers);
            }
        }
    }
}
