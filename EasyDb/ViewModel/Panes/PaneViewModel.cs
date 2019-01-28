namespace EasyDb.ViewModel.Panes
{
    using System;

    using GalaSoft.MvvmLight;

    /// <summary>
    ///     Defines the <see cref="PaneViewModel" />
    /// </summary>
    public class PaneViewModel : ViewModelBase
    {
        /// <summary>
        ///     Defines the _contentId
        /// </summary>
        private string _contentId;

        /// <summary>
        ///     Defines the _isActive
        /// </summary>
        private bool _isActive;

        /// <summary>
        ///     Defines the _isSelected
        /// </summary>
        private bool _isSelected;

        /// <summary>
        ///     Defines the _title
        /// </summary>
        private string _title;

        /// <summary>
        ///     Gets or sets the ContentId
        /// </summary>
        public string ContentId
        {
            get => this._contentId;

            set
            {
                if (this._contentId != value)
                {
                    this._contentId = value;
                    this.RaisePropertyChanged(() => this.ContentId);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the IconSource
        /// </summary>
        public virtual Uri IconSource { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether IsActive
        /// </summary>
        public bool IsActive
        {
            get => this._isActive;

            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                    this.RaisePropertyChanged(() => this.IsActive);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether IsSelected
        /// </summary>
        public bool IsSelected
        {
            get => this._isSelected;

            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the Title
        /// </summary>
        public string Title
        {
            get => this._title;

            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.RaisePropertyChanged(() => this.Title);
                }
            }
        }
    }
}