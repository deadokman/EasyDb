using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;
using NLog;

namespace EasyDb.DataSource
{
    /// <summary>
    /// Get supported datasource drivers
    /// </summary>
    public sealed class DataSourceManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Supported datasources collection
        /// </summary>
        private Dictionary<Guid, IEasyDbDataSource> _supportedDataSources;

        private DataSourceManager()
        {
            _supportedDataSources = new Dictionary<Guid, IEasyDbDataSource>();
            UserdefinedDatasources = new ObservableCollection<UserDefinedDataSource>();
            InitialLoad(Path.Combine(Directory.GetCurrentDirectory(), "SourceExtensions"));
        }

        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
        {
        }

        private static DataSourceManager _instance;

        /// <summary>
        /// Instance
        /// </summary>
        public static DataSourceManager Instance
        {
            get { return _instance = _instance ?? (_instance = new DataSourceManager()); }
        }

        /// <summary>
        /// Collection of user defined datasources
        /// </summary>
        public ObservableCollection<UserDefinedDataSource> UserdefinedDatasources { get; set; }

        /// <summary>
        /// Load datasource modules
        /// </summary>
        public void InitialLoad(string datasourceAssembliesPath)
        {
            //Get all assemblies from path
            foreach (var assmFile in Directory.GetFiles(datasourceAssembliesPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(assmFile);
                    var types = assembly.GetTypes().Where(t =>
                        t.IsClass && !t.IsAbstract && t.IsAssignableFrom(typeof(IEasyDbDataSource)));
                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes(typeof(EdbDatasourceAttribute)).ToArray();
                        if (attributes.Length == 0)
                        {
                            _logger.Warn(App.Current.Resources["log_NotImplementedAttr"].ToString(), type.Name, assembly.FullName);
                        }

                        if (attributes.Length > 1)
                        {
                            _logger.Warn(App.Current.Resources["log_NotImplementedAttr"].ToString(), type.Name, assembly.FullName);
                        }

                        var attribute = (EdbDatasourceAttribute)attributes[0];
                        var datasourceInstance = ProcessType(type);
                        if (datasourceInstance != null)
                        {
                            _supportedDataSources.Add(attribute.SourceGuid, datasourceInstance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            // Restore user defined datasource
             
        }

        /// <summary>
        /// Process plugin type
        /// </summary>
        private IEasyDbDataSource ProcessType(Type t)
        {
            try
            {
                return (IEasyDbDataSource)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                _logger.Error($"Err: \n {ex}");
                return null;
            }
        }
    }

}
