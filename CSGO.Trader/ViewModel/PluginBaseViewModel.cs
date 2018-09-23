using System;
using System.Windows.Controls;
using System.Windows.Input;
using CSGO.Trader.ViewModel.Panes;

namespace EasyDb.ViewModel
{
    public delegate void PluginClosingDelegate(PluginBaseViewModel vm);

    public abstract class PluginBaseViewModel : PaneViewModel
    {
        public PluginBaseViewModel(string title, UserControl mainInterface)
        {
            Title = title;
            _pluginViewInstance = mainInterface;
        }

        public Guid PluginId { get; private set; }


        public UserControl PluginViewInstance
        {
            get { return _pluginViewInstance; }
            set
            {
                _pluginViewInstance = value;
                RaisePropertyChanged(() => PluginViewInstance);
            }
        }

        private UserControl _pluginViewInstance;
        private bool _isInitializing;
        private bool _isNotInitializing;

        /// <summary>
        /// Плагин инициализируется.
        /// </summary>
        public bool IsInitializing
        {
            get { return _isInitializing; }
            set
            {
                _isInitializing = value;
                IsNotInitializing = !value;
                RaisePropertyChanged(() => IsInitializing);
            }
        }

        /// <summary>
        /// Плагин инициализируется.
        /// </summary>
        public bool IsNotInitializing
        {
            get { return _isNotInitializing; }
            set
            {
                _isNotInitializing = value;
                RaisePropertyChanged(() => IsNotInitializing);
            }
        }

        protected void InvokeClosing()
        {
            if (PluginClosing != null)
            {
                PluginClosing.Invoke(this);
            }
        }

        public event PluginClosingDelegate PluginClosing;

        public ICommand CloseCommand { get; set; }
    }
}
