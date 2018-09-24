using System;
using System.Windows.Controls;
using System.Windows.Input;
using CSGO.Trader.ViewModel.Panes;

namespace EasyDb.ViewModel
{
    public delegate void PaneClosingDelegate(PaneBaseViewModel vm);

    public abstract class PaneBaseViewModel : PaneViewModel
    {
        public PaneBaseViewModel(string title)
        {
            Title = title;
        }

        public Guid PluginId { get; private set; }


        public abstract UserControl ViewInstance { get; }
   

        private UserControl _viewInstance;
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
            if (PaneClosing != null)
            {
                PaneClosing.Invoke(this);
            }
        }

        public event PaneClosingDelegate PaneClosing;

        public ICommand CloseCommand { get; set; }
    }
}
