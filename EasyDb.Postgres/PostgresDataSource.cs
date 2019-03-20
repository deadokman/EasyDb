namespace EasyDb.Postgres
{
    using Options;
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
            return new EdbSourceOption[] { new GeneralOption() };
        }

        /// <summary>
        /// Identifier of chocolate ODBC package
        /// </summary>
        public override string ChocolateOdbcPackageId => "psqlodbc";

        /// <summary>
        /// URL to ODBC driver package
        /// </summary>
        public override string ChocolatepackageUrl => "https://chocolatey.org/packages/psqlodbc";

        /// <summary>
        /// ODBC driver name inside operating system
        /// </summary>
        public override string OdbcSystemDriverName => "PostgreSQL ANSI(x64)";

        /// <summary>
        /// Driver name for x32 architecture systems
        /// </summary>
        public override string OdbcSystem32DriverName => "PostgreSQL ANSI(x32)";

        /// <summary>
        /// Query module producer
        /// </summary>
        public override IEdbModuleQueryProducer QueryModuleProducer { get; }
    }
}
