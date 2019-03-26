using EasyDb.Postgres.QueryProducing;
using EDb.Interfaces.iface;

namespace EasyDb.Postgres
{
    using System;

    using Options;
    using EDb.Interfaces;
    using EDb.Interfaces.Objects;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="PostgresDataSource" />
    /// </summary>
    [EdbDatasource("E7B64810-1527-4954-B93B-6C7E46F31E2E", "0.0.1")]
    public class PostgresDataSource : EdbDataDatasource
    {
        private const string PasswordTag = "$%PasswordTag%$";

        // PostgreSQL
        private readonly string _connectionStringTemplate = "Driver={5};Server={0};Port={1};Database={2};\r\nUid={3};Pwd={4}";

        /// <summary>
        /// Gets the DatasourceName
        /// </summary>
        public override string DatasourceName => "PostgreSql";

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
        public override string OdbcSystemDriverName => "PostgreSQL Unicode(x64)";

        /// <summary>
        /// Driver name for x32 architecture systems
        /// </summary>
        public override string OdbcSystem32DriverName => "PostgreSQL Unicode";

        /// <summary>
        /// Creates connection string for datasoure
        /// </summary>
        /// <param name="options">Datasource options</param>
        /// <returns>Returns connection string</returns>
        public override string IntroduceConnectionString(EdbSourceOption[] options, out string passwordTag)
        {
            var generalOptions = options.OfType<GeneralOption>().FirstOrDefault();
            if (generalOptions != null)
            {
                passwordTag = PasswordTag;
                // Driver={PostgreSQL};Server={0};Port={1};Database={2};\r\nUid={3};Pwd={4}
                return string.Format(
                    _connectionStringTemplate,
                    generalOptions.Host,
                    generalOptions.Port,
                    generalOptions.Database,
                    generalOptions.User,
                    PasswordTag,
                    "{" + OdbcSystemDriverName + "}");
            }

            throw new Exception($"Cannot fild supported option type {nameof(GeneralOption)}");
        }

        /// <summary>
        /// Database query producer
        /// </summary>
        /// <returns>Instance of database query producer</returns>
        public override IEdbDataSourceQueryProducer GetQueryProducer()
        {
            return new PostgresQueryProducer();
        }
    }
}
