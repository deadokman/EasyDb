namespace EasyDb.Postgres
{
    using EasyDb.Postgres.Options;
    using EDb.Interfaces;
    using EDb.Interfaces.Objects;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="PostgresDataSource" />
    /// </summary>
    [EdbDatasource("E7B64810-1527-4954-B93B-6C7E46F31E2E", "0.0.1")]
    public class PostgresDataSource : EdbDatasourceModule
    {
        /// <summary>
        /// Gets the DatabaseName
        /// </summary>
        public override string DatabaseName => "PostgreSql";

        /// <summary>
        /// Gets the SupportedTypes
        /// </summary>
        public override SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Gets the DatabaseIcon
        /// </summary>
        public override byte[] DatabaseIcon
        {
            get
            {
                using (var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("EasyDb.Postgres.PostgresImage.png"))
                {
                    if (stream != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }

                    return new byte[0];
                }
            }
        }

        /// <summary>
        /// The GetOptions
        /// </summary>
        /// <returns>The <see cref="EdbSourceOption[]"/></returns>
        public override EdbSourceOption[] GetOptions()
        {
            return new EdbSourceOption[] { new GeneralOption(), new SshSsl() };
        }

        public override string ChocolateOdbcPackageId { get; }

        public override string ChocolatepackageUrl { get; }
    }
}
