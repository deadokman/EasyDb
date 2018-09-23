//using System;
//using System.Windows.Input;
//using CSGO.Trader.ViewModel;
//using GalaSoft.MvvmLight.CommandWpf;
//using Trader.Plugin.PluginApi;

//namespace CSGO.Trader.PluginFunctional
//{


//    /// <summary>
//    /// Основная вью-модель плагина
//    /// </summary>
//    public class PluginMarketplaceViewModel : PluginBaseViewModel
//    {
//        private MarketPlacePlugin _pluginInstance;

//        public PluginMarketplaceViewModel(MarketPlacePlugin pluginInstance) 
//            : base(pluginInstance, pluginInstance.PluginUi)
//        {
//            _pluginInstance = pluginInstance;
//            _pluginInstance.PluginReadyForUse += PluginInstanceOnPluginReadyForUse;
//            CloseCommand = new RelayCommand(() =>
//            {
//                _pluginInstance.PluginReadyForUse -= PluginInstanceOnPluginReadyForUse;
//                InvokeClosing();
//            });
//        }

//        private void PluginInstanceOnPluginReadyForUse(object sender, MarketPlacePlugin marketPlacePlugin)
//        {
//            this.IsInitializing = false;
//        }
//    }
//}
