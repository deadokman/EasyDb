namespace EasyDb.ViewModel.DataSource
{
    using System;
    using System.Windows.Input;
    using System.Windows.Media;

    using EasyDb.Annotations;
    using EasyDb.ViewModel.DataSource.Items;

    using EDb.Interfaces;

    using GalaSoft.MvvmLight.CommandWpf;

    /// <summary>
    /// Supported source view model item. Display items collection in deriver selection menu
    /// Поддерживаемый источник данных
    /// </summary>
    public class SupportedSourceItem
    {
        /// <summary>
        /// Defines the _invoke
        /// </summary>
        private readonly Func<IEdbDatasourceModule, UserDataSource> _invoke;

        /// <summary>
        /// Defines the _sourceModule
        /// </summary>
        private readonly IEdbDatasourceModule _sourceModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedSourceItem"/> class.
        /// </summary>
        /// <param name="sourceModule">The sourceModule<see cref="IEdbDatasourceModule"/></param>
        /// <param name="invokeCfgSourceModule">The invokeCfgSourceModule<see cref="Func{IEdbDatasourceModule, UserDataSource}"/></param>
        public SupportedSourceItem(
            [NotNull] IEdbDatasourceModule sourceModule,
            [NotNull] Func<IEdbDatasourceModule, UserDataSource> invokeCfgSourceModule)
        {
            this._sourceModule = sourceModule ?? throw new ArgumentNullException(nameof(sourceModule));
            this._invoke = invokeCfgSourceModule ?? throw new ArgumentNullException(nameof(invokeCfgSourceModule));
            this.InvokeCreateSource = new RelayCommand(() => { this.InvokeConfigure(); });
        }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Иконка базы данных
        /// </summary>
        public ImageSource DatabaseIcon => this._sourceModule.DatabaseIcon;

        /// <summary>
        /// Gets the DatabaseName
        /// Имя источника данных
        /// </summary>
        public string DatabaseName => this._sourceModule.DatabaseName;

        /// <summary>
        /// Gets or sets the InvokeCreateSource
        /// Вызвать делегат создания нового источника данных
        /// </summary>
        public ICommand InvokeCreateSource { get; set; }

        /// <summary>
        /// Gets or sets the Module
        /// Драйвер источника данных
        /// </summary>
        public IEdbDatasourceModule Module { get; set; }

        /// <summary>
        /// Вызвать конфигурирование источника
        /// </summary>
        /// <returns>The <see cref="UserDataSource"/></returns>
        public UserDataSource InvokeConfigure()
        {
            return this._invoke?.Invoke(this._sourceModule);
        }
    }
}