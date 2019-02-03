namespace EasyDb.Postgres.Options
{
    using EDb.Interfaces;

    /// <summary>
    /// Defines the <see cref="SshSsl" />
    /// </summary>
    public class SshSsl : EdbSourceOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SshSsl"/> class.
        /// </summary>
        public SshSsl()
        {
            OptionsDefinitionName = "Ssl/Ssh";
        }
    }
}
