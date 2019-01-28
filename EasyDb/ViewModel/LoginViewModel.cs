namespace EasyDb.ViewModel
{
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Defines the _isStorePassword
        /// </summary>
        private bool _isStorePassword;

        /// <summary>
        /// Defines the _login
        /// </summary>
        private string _login;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            this.AuthCommand = new RelayCommand(() => { });
        }

        /// <summary>
        /// Gets or sets the AuthCommand
        /// Steam auth command
        /// </summary>
        public ICommand AuthCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsStorePassword
        /// Store password option
        /// </summary>
        public bool IsStorePassword
        {
            get
            {
                return this._isStorePassword;
            }

            set
            {
                this._isStorePassword = value;
                RaisePropertyChanged(() => this.IsStorePassword);
            }
        }

        /// <summary>
        /// Gets or sets the Login
        /// Steam login
        /// </summary>
        public string Login
        {
            get
            {
                return this._login;
            }

            set
            {
                this._login = value;
            }
        }
    }
}