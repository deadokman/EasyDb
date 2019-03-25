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
            get => _contentId;

            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    RaisePropertyChanged(() => ContentId);
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
            get => _isActive;

            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged(() => IsActive);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether IsSelected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;

            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the Title
        /// </summary>
        public string Title
        {
            get => _title;

            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }
    }
}