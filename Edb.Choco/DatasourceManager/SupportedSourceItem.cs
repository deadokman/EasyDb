﻿using System.Drawing;
using EDb.Interfaces.iface;

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
        private readonly IEdbDataSource _dataSource;

        /// <summary>
        /// Database icon image
        /// </summary>
        private BitmapImage _image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedSourceItem"/> class.
        /// </summary>
        /// <param name="dataSource">The dataSource<see cref="EdbDataDatasource"/></param>
        public SupportedSourceItem(
            [NotNull] IEdbDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Иконка базы данных
        /// </summary>
        public ImageSource DatabaseImage
        {
            get
            {
                if (_image == null)
                {
                    using (var memStream = new MemoryStream(_dataSource.DatabaseIcon))
                    {
                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = memStream;
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.EndInit();
                        bi.Freeze();
                        _image = bi;
                    }
                }

                return _image;
            }
        }

        /// <summary>
        /// Database icon
        /// </summary>
        public Icon DatabaseIcon
        {
            get
            {
                using (var memStream = new MemoryStream(_dataSource.DatabaseIcon))
                {
                    return new Icon(memStream);
                }
            }
        }

        /// <summary>
        /// Gets the DatasourceName
        /// Имя источника данных
        /// </summary>
        public string DatabaseName => _dataSource.DatasourceName;

        /// <summary>
        /// Gets or sets the Module
        /// Драйвер источника данных
        /// </summary>
        public IEdbDataSource Module => _dataSource;
    }
}