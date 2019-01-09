using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public Icon DatabaseIcon { get; }
        public IDbConnection GetDatabaseConnection { get; }

        ImageSource IEdbDatasourceModule.DatabaseIcon => throw new NotImplementedException();

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
