using System.IO;
using System.Windows.Media.Imaging;

namespace EasyDb.ViewModel.DataSource
{
    using System;
    using System.Windows.Media;

    using EasyDb.Annotations;

    using EDb.Interfaces;

    /// <summary>
    /// Supported source view model item. Display items collection in deriver selection menu
    /// Поддерживаемый источник данных
    /// </summary>
    public class SupportedSourceItem
    {
        /// <summary>
        /// Defines the _sourceModule
        /// </summary>
        private readonly IEdbSourceModule _sourceModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedSourceItem"/> class.
        /// </summary>
        /// <param name="sourceModule">The sourceModule<see cref="EdbDatasourceModule"/></param>
        public SupportedSourceItem(
            [NotNull] IEdbSourceModule sourceModule)
        {
            this._sourceModule = sourceModule ?? throw new ArgumentNullException(nameof(sourceModule));
        }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Иконка базы данных
        /// </summary>
        public ImageSource DatabaseIcon
        {
            get
            {
                using (var memStream = new MemoryStream(_sourceModule.DatabaseIcon))
                {
                    var bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = memStream;
                    bi.EndInit();

                    return bi;
                }
            }
        }

        /// <summary>
        /// Gets the DatabaseName
        /// Имя источника данных
        /// </summary>
        public string DatabaseName => this._sourceModule.DatabaseName;

        /// <summary>
        /// Gets or sets the Module
        /// Драйвер источника данных
        /// </summary>
        public IEdbSourceModule Module => this._sourceModule;
    }
}