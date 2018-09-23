using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EasyDb.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        private bool _isStorePassword;

        public LoginViewModel()
        {
            AuthCommand = new RelayCommand(() =>
            {
                
            });
        }

        /// <summary>
        /// Steam auth command
        /// </summary>
        public ICommand AuthCommand { get; set; }

        /// <summary>
        /// Steam login
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        /// <summary>
        /// Store password option
        /// </summary>
        public bool IsStorePassword
        {
            get { return _isStorePassword; }
            set
            {
                _isStorePassword = value;
                RaisePropertyChanged(() => IsStorePassword);
            }
        }
    }
}
