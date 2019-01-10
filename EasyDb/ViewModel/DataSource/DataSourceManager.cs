using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Serialization;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;
using NLog;
using File = System.IO.File;

namespace EasyDb.ViewModel.DataSource
{
    /// <summary>
    /// Get supported datasource drivers
    /// </summary>
    public sealed class DatasourceManager
    {
        public delegate void DatasourceData(IEnumerable<IEdbDatasourceModule> datasources, IEnumerable<UserDataSource> userSources);

        /// <summary>
        /// 
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Supported datasources collection
        /// </summary>
        private Dictionary<Guid, IEdbDatasourceModule> _supportedDataSources;

        /// <summary>
        /// Datasource storage filepath
        /// </summary>
        private const string DataSourceStorageFile = "Edb.Datasource";

        private XmlSerializer _xseri;

        private DatasourceManager()
        {
            _supportedDataSources = new Dictionary<Guid, IEdbDatasourceModule>();
            _xseri = new XmlSerializer(typeof(List<UserDataSource>));
            UserdefinedDatasources = new List<UserDataSource>();
        }

        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
        {
        }

        private static DatasourceManager _instance;

        /// <summary>
        /// Instance
        /// </summary>
        public static DatasourceManager Instance
        {
            get { return _instance = _instance ?? (_instance = new DatasourceManager()); }
        }

        /// <summary>
        /// Collection of user defined datasources
        /// </summary>
        public List<UserDataSource> UserdefinedDatasources { get; set; }

        public IEnumerable<IEdbDatasourceModule> SupportedDatasources => _supportedDataSources.Values;

        /// <summary>
        /// Creates new user defined datasource
        /// </summary>
        /// <param name="module">datasource module</param>
        /// <returns>User defined datasource</returns>
        public UserDataSource CreateNewUserdatasource(IEdbDatasourceModule module)
        {
             
            var uds = new UserDataSource { LinkedEdbSourceModule = module, DatasourceGuid = module.ModuleGuid, SettingsObjects = module.GetDefaultOptionsObjects() };
            UserdefinedDatasources.Add(uds);
            return uds;
        }

        /// <summary>
        /// Load datasource modules
        /// </summary>
        public void InitialLoad(string datasourceAssembliesPath)
        {
            List<UserDataSource> serializedSources;

            // Load and serialize XML doc. Create new one if it does not exists;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), DataSourceStorageFile);
            if (File.Exists(filePath))
            {
                try
                {
                    serializedSources = (List<UserDataSource>) _xseri.Deserialize(File.OpenRead(filePath));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error while parsing user defined sources: \n {ex}");
                    serializedSources = new List<UserDataSource>();
                }
            }
            else
            {
                serializedSources = new List<UserDataSource>();
            }

            //Get all assemblies from path
            foreach (var assmFile in Directory.GetFiles(datasourceAssembliesPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(assmFile);
                    var types = assembly.GetTypes().Where(t =>
                        t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IEdbDatasourceModule)));
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
                            datasourceInstance.SetGuid(attribute.SourceGuid);
                            datasourceInstance.SetVersion(attribute.Version);
                            _supportedDataSources.Add(attribute.SourceGuid, datasourceInstance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"err: {ex}");
                    continue;
                }
            }

            // Restore user defined datasource
            foreach (var uds in serializedSources)
            {
                IEdbDatasourceModule dbSourceModule;

                // Check that user defined data source exists in datasource module 
                if (_supportedDataSources.TryGetValue(uds.DatasourceGuid, out dbSourceModule))
                {
                    uds.LinkedEdbSourceModule = dbSourceModule;
                    UserdefinedDatasources.Add(uds);
                }
                else
                {
                    _logger.Warn(Application.Current.Resources["log_NotImplementedAttr"].ToString(), uds.Name);
                }
            }

            if (DatasourceLoaded != null)
            {
                DatasourceLoaded.Invoke(_supportedDataSources.Values, UserdefinedDatasources);
            }
        }

        /// <summary>
        /// Process plugin type
        /// </summary>
        private IEdbDatasourceModule ProcessType(Type t)
        {
            try
            {
                return (IEdbDatasourceModule)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                _logger.Error($"Err: \n {ex}");
                return null;
            }
        }

        public event DatasourceData DatasourceLoaded;
    }

}
