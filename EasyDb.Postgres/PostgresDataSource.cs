using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EasyDb.Postgres.Options;
using EDb.Interfaces;
using EDb.Interfaces.Objects;

namespace EasyDb.Postgres
{
    [EdbDatasource("E7B64810-1527-4954-B93B-6C7E46F31E2E", "0.0.1")]
    public class PostgresDataSource : IEdbDatasourceModule
    {
        public string DatabaseName => "PostgreSql";
        public SupportedObjectTypes[] SupportedTypes { get; }

        public byte[] DatabaseIcon
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

        public IDbConnection GetDatabaseConnection { get; }


        public Guid ModuleGuid { get; private set; }

        public Version Version { get; private set; }

        public void SetVersion(Version version)
        {
            Version = version;
        }

        public void SetGuid(Guid guid)
        {
            ModuleGuid = guid;
        }

        public void SetConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public EdbSourceOption[] GetDefaultOptionsObjects()
        {
            return new EdbSourceOption[] { new GeneralOption(), new SshSsl() };
        }
    }
}
