namespace EasyDb.ViewModel
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    using EasyDb.ViewModel.Panes;

    /// <summary>
    /// The PaneClosingDelegate
    /// </summary>
    /// <param name="vm">The vm<see cref="PaneBaseViewModel"/></param>
    public delegate void PaneClosingDelegate(PaneBaseViewModel vm);

    /// <summary>
    /// Defines the <see cref="PaneBaseViewModel" />
    /// </summary>
    public abstract class PaneBaseViewModel : PaneViewModel
    {
        /// <summary>
        /// Defines the _isInitializing
        /// </summary>
        private bool _isInitializing;

        /// <summary>
        /// Defines the _isNotInitializing
        /// </summary>
        private bool _isNotInitializing;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaneBaseViewModel"/> class.
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        public PaneBaseViewModel(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Defines the PaneClosing
        /// </summary>
        public event PaneClosingDelegate PaneClosing;

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsInitializing
        /// Плагин инициализируется.
        /// </summary>
        public bool IsInitializing
        {
            get
            {
                return _isInitializing;
            }

            set
            {
                _isInitializing = value;
                IsNotInitializing = !value;
                RaisePropertyChanged(() => IsInitializing);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsNotInitializing
        /// Плагин инициализируется.
        /// </summary>
        public bool IsNotInitializing
        {
            get
            {
                return _isNotInitializing;
            }

            set
            {
                _isNotInitializing = value;
                RaisePropertyChanged(() => IsNotInitializing);
            }
        }

        /// <summary>
        /// Gets the PluginId
        /// </summary>
        public Guid PluginId { get; private set; }

        /// <summary>
        /// Gets the ViewInstance
        /// </summary>
        public abstract UserControl ViewInstance { get; }

        /// <summary>
        /// Close view
        /// </summary>
        public void Close()
        {
            Cleanup();
            InvokeClosing();
        }

        /// <summary>
        /// The InvokeClosing
        /// </summary>
        protected void InvokeClosing()
        {
            PaneClosing?.Invoke(this);
        }
    }
}