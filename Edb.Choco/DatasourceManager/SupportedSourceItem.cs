namespace Edb.Environment.DatasourceManager
{
    using System;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using EDb.Interfaces;
    using EDb.Interfaces.Annotations;

    /// <summary>
    /// Supported source view model item. Display items collection in deriver selection menu
    /// Поддерживаемый источник данных
    /// </summary>
    public class SupportedSourceItem
    {
        /// <summary>
        /// Defines the dataSource
        /// </summary>
        private readonly IEdbDataSource dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedSourceItem"/> class.
        /// </summary>
        /// <param name="dataSource">The dataSource<see cref="EdbDataDatasource"/></param>
        public SupportedSourceItem(
            [NotNull] IEdbDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Иконка базы данных
        /// </summary>
        public ImageSource DatabaseIcon
        {
            get
            {
                using (var memStream = new MemoryStream(this.dataSource.DatabaseIcon))
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
        public string DatabaseName => this.dataSource.DatabaseName;

        /// <summary>
        /// Gets or sets the Module
        /// Драйвер источника данных
        /// </summary>
        public IEdbDataSource Module => this.dataSource;
    }
}