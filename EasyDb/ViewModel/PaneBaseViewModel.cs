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
            this.Title = title;
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
                return this._isInitializing;
            }

            set
            {
                this._isInitializing = value;
                this.IsNotInitializing = !value;
                RaisePropertyChanged(() => this.IsInitializing);
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
                return this._isNotInitializing;
            }

            set
            {
                this._isNotInitializing = value;
                RaisePropertyChanged(() => this.IsNotInitializing);
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
        /// The InvokeClosing
        /// </summary>
        protected void InvokeClosing()
        {
            if (this.PaneClosing != null)
            {
                this.PaneClosing.Invoke(this);
            }
        }
    }
}