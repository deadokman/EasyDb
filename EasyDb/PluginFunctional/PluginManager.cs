//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Threading;
//using System.Windows;
//using System.Windows.Threading;
//using CSGO.Trader.PluginFunctional.Interaction;
//using CSGO.Trader.ViewModel;
//using NLog;

//namespace CSGO.Trader.PluginFunctional
//{
//    /// <summary>
//    /// Load marketplace plugin from location and notify listners about collection change
//    /// Single-instance pattern;
//    /// </summary>
//    public sealed class PluginManager
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        private static Logger _logger = LogManager.GetCurrentClassLogger();

//        /// <summary>
//        /// Plugin collection
//        /// </summary>
//        private Dictionary<Guid, MarketPlacePlugin> _internalPlugins;

//        /// <summary>
//        /// External plugins collection
//        /// </summary>
//        private Dictionary<Guid, MarketPlacePlugin> _externalPlugins;

//        private PluginEnvironment _environment;

//        /// <summary>
//        /// Collection of initilized plugin ViewModels
//        /// </summary>
//        public Dictionary<Guid, PluginBaseViewModel> PluginViewModels { get; private set; }

//        private PluginManager()
//        {
//            App.LanguageChanged += AppOnLanguageChanged;
//            _environment = new PluginEnvironment(App.Language);
//            _internalPlugins = new Dictionary<Guid, MarketPlacePlugin>();
//            _externalPlugins = new Dictionary<Guid, MarketPlacePlugin>();
//            PluginViewModels = new Dictionary<Guid, PluginBaseViewModel>();
//        }

//        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
//        {
//            _environment.SetCulture(App.Language);
//        }

//        private static PluginManager _instance;

//        /// <summary>
//        /// Instance
//        /// </summary>
//        public static PluginManager Instance
//        {
//            get { return _instance = _instance ?? (_instance = new PluginManager()); }
//        }

//        /// <summary>
//        /// Load integrated and outside plugins
//        /// </summary>
//        public void InitialLoad()
//        {
////#if DEBUG
////            Thread.Sleep(10000);
////#endif
//            //Load integrated plugins
//            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MarketPlacePlugin)));
//            foreach (var type in types)
//            {
//                var pluginInstance = ProcessType(type);
//                if (pluginInstance != null)
//                {
//                    _internalPlugins.Add(pluginInstance.PluginGuid, pluginInstance);
//                    Application.Current.Dispatcher.Invoke(() =>
//                    {
//                        pluginInstance.InitPlugin(_environment);
//                    });
//                }
//            }

//            ReloadPlugins();
//        }

//        /// <summary>
//        /// Открыть плагин или сделать активным если уже открыт
//        /// </summary>
//        /// <param name="instance"></param>
//        /// <param name="initDataRequiered"> Необходимо инициализировать данные в плагине </param>
//        /// <returns></returns>
//        public PluginBaseViewModel InstancePlugin(MarketPlacePlugin instance, out bool initDataRequiered)
//        {
//            PluginBaseViewModel plugin;
//            initDataRequiered = false;
//            //Если инстанс плагина еще не создан - создать его
//            if (!PluginViewModels.TryGetValue(instance.PluginGuid, out plugin))
//            {
//                instance.InitInterface();
//                initDataRequiered = true;
//                plugin = new PluginMarketplaceViewModel(instance);
//                plugin.IsInitializing = true;
//                instance.SetUp(new Credentials("","", "", EmarketCredentialType.Default));
//                //Подписаться на событие изменения обзора объекта в плагине
//                PluginViewModels.Add(instance.PluginGuid, plugin);
//            }

//            return plugin;
//        }


//        /// <summary>
//        /// Reload plugins from default plugin folder
//        /// </summary>
//        public void ReloadPlugins()
//        {
//            _externalPlugins = new Dictionary<Guid, MarketPlacePlugin>(_internalPlugins);
//            var path = Path.GetFullPath(Properties.Settings.Default.PluginsPath);
//            if (Directory.Exists(path))
//            {
//                var files = Directory.GetFiles(path, "*.dll");
//                foreach (var file in files)
//                {
//                    try
//                    {
//                        var plugs = Assembly.Load(file).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MarketPlacePlugin)))
//                            .Select(ProcessType).Where(i => i != null);
//                        foreach (var marketPlacePlugin in plugs)
//                        {
//                            try
//                            {
//                                _externalPlugins.Add(marketPlacePlugin.PluginGuid, marketPlacePlugin);
//                                Application.Current.Dispatcher.Invoke(() =>
//                                {
//                                    marketPlacePlugin.InitInterface();
//                                });
//                            }
//                            catch (ArgumentException ex)
//                            {
//                                var plug = _externalPlugins[marketPlacePlugin.PluginGuid];
//                                _logger.Error(
//                                    $"Error PLUGIN GUID CONFLICT: {marketPlacePlugin.PluginName} <=> {plug.PluginName} remove one of this plugins");
//                            }
//                            catch (Exception ex)
//                            {
//                                _logger.Log(LogLevel.Error, new Exception($"Ошибка инициализации плагина: {marketPlacePlugin.PluginName}", ex));
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        _logger.Warn($"Error while loading file: {file} \n { ex }");
//                    }
//                }
//            }

//            NotifyCollectionChanged(_externalPlugins.Values.ToArray());
//        }

//        /// <summary>
//        /// Process plugin type
//        /// </summary>
//        private MarketPlacePlugin ProcessType(Type t)
//        {
//            var attr = t.GetCustomAttributes(typeof(PluginConfigurationsAttribute)).ToArray();
//            if (!attr.Any())
//            {
//                _logger.Warn((string)Application.Current.Resources["log_NotImplementedAttr"], t.FullName);
//            }

//            var config = (PluginConfigurationsAttribute) attr.First();
//            MarketPlacePlugin plugin;
//            try
//            {
//                plugin = (MarketPlacePlugin)Activator.CreateInstance(t);
//                plugin.SetGuid(config.PluginGuid);
//                plugin.SetName(config.PluginTitle);
//                return plugin;
//            }
//            catch (Exception ex)
//            {
//                _logger.Error($"Err: \n {ex}");
//                return null;
//            }
//        }

//        private void NotifyCollectionChanged(MarketPlacePlugin[] plugins)
//        {
//            if (PluginCollectionChanged != null)
//            {
//                try
//                {
//                    PluginCollectionChanged.Invoke(plugins);
//                }
//                catch (Exception ex)
//                {
//                    _logger.Error($"Err: \n {ex}");
//                }
//            }

//        }

//        public event PluginCollectionChanged PluginCollectionChanged;
//    }

//}
