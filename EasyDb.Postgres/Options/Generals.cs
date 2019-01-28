namespace EasyDb.Postgres.Options
{
    using System.ComponentModel.DataAnnotations;

    using EDb.Interfaces;

    /// <summary>
    ///     Основные настройки для работы с Postgres
    /// </summary>
    public class GeneralOption : EdbSourceOption
    {
        private string _database;

        private string _host;

        private int _port;

        private string _user;

        public GeneralOption()
        {
            this.OptionsDefinitionName = "General";
            this._port = 5432;
        }

        [Required(ErrorMessage = "Database name required")]
        [OptionDisplayName("opt_def_database", "Database:")]
        public string Database
        {
            get => this._database;
            set
            {
                this._database = value;
                this.OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Hostname required")]
        [OptionDisplayName("opt_def_hostname", "Hostname:")]
        [RegularExpression(
            @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])|((([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$",
            ErrorMessage = "Should be IP or valid hostname")]
        public string Host
        {
            get => this._host;
            set
            {
                this._host = value;
                this.OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Password required")]
        [OptionDisplayName("opt_def_password", "Password:")]
        [PasswordField]
        public string Password { get; set; }

        [Required(ErrorMessage = "Port required")]
        [OptionDisplayName("opt_def_port", "Port:")]
        [RegularExpression("^[0-9]{1,7}$")]
        public int Port
        {
            get => this._port;
            set
            {
                this._port = value;
                this.OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Username required")]
        [OptionDisplayName("opt_def_user", "User:")]
        public string User
        {
            get => this._user;
            set
            {
                this._user = value;
                this.OnPropertyChanged();
            }
        }
    }
}