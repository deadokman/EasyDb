namespace EasyDb.Postgres.Options
{
    using EDb.Interfaces;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Основные настройки для работы с Postgres
    /// </summary>
    public class GeneralOption : EdbSourceOption
    {
        /// <summary>
        /// Defines the _database
        /// </summary>
        private string _database;

        /// <summary>
        /// Defines the _host
        /// </summary>
        private string _host;

        /// <summary>
        /// Defines the _port
        /// </summary>
        private int _port;

        /// <summary>
        /// Defines the _user
        /// </summary>
        private string _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralOption"/> class.
        /// </summary>
        public GeneralOption()
        {
            OptionsDefinitionName = "General";
            _port = 5432;
        }

        /// <summary>
        /// Gets or sets the Database
        /// </summary>
        [Required(ErrorMessage = "Database name required")]
        [OptionDisplayName("opt_def_database", "Database:")]
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Host
        /// </summary>
        [Required(ErrorMessage = "Hostname required")]
        [OptionDisplayName("opt_def_hostname", "Hostname:")]
        [RegularExpression(@"^(^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))$)|((([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$",
            ErrorMessage = "Should be IP or valid hostname", MatchTimeoutInMilliseconds = 1000)]
        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the User
        /// </summary>
        [Required(ErrorMessage = "Username required")]
        [OptionDisplayName("opt_def_user", "User:")]
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [OptionDisplayName("opt_def_password", "Password:")]
        [PasswordField]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        [Required(ErrorMessage = "Port required")]
        [OptionDisplayName("opt_def_port", "Port:")]
        [RegularExpression("^[0-9]{1,7}$")]
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }
    }
}
