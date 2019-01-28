using System;
using System.Windows.Input;
using System.Windows.Media;
using EasyDb.Annotations;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace EasyDb.ViewModel.DataSource
{
    /// <summary>
    /// Supported source view model item. Display items collection in deriver selection menu
    /// Поддерживаемый источник данных
    /// </summary>
    public class SupportedSourceItem
    {
        private readonly IEdbDatasourceModule _sourceModule;
        private readonly Func<IEdbDatasourceModule, UserDataSource> _invoke;

        public SupportedSourceItem([NotNull] IEdbDatasourceModule sourceModule, [NotNull] Func<IEdbDatasourceModule, UserDataSource> invokeCfgSourceModule)
        {
            _sourceModule = sourceModule ?? throw new ArgumentNullException(nameof(sourceModule));
            _invoke = invokeCfgSourceModule ?? throw new ArgumentNullException(nameof(invokeCfgSourceModule));
            InvokeCreateSource = new RelayCommand(() => { InvokeConfigure(); });
        }

        /// <summary>
        /// Вызвать конфигурирование источника
        /// </summary>
        public UserDataSource InvokeConfigure()
        {
            return _invoke?.Invoke(_sourceModule);
        }

        /// <summary>
        /// Имя источника данных
        /// </summary>
        public string DatabaseName => _sourceModule.DatabaseName;

        /// <summary>
        /// Иконка базы данных
        /// </summary>
        public ImageSource DatabaseIcon => _sourceModule.DatabaseIcon;

        /// <summary>
        /// Драйвер источника данных
        /// </summary>
        public IEdbDatasourceModule Module { get; set; }

        /// <summary>
        /// Вызвать делегат создания нового источника данных
        /// </summary>
        public ICommand InvokeCreateSource { get; set; }
    }
}
