using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.SandboxEnvironment
{
    using System.Data;
    using System.Windows.Media;

    using EasyDb.Annotations;

    using EDb.Interfaces;
    using EDb.Interfaces.Objects;

    /// <summary>
    /// Proxy EdbModule calls in safe context
    /// </summary>
    public class EdbModuleProxy
    {
        /*
        public EdbModuleProxy([NotNull] IEdbDatasourceModule module)
        {
            this._module = module ?? throw new ArgumentNullException(nameof(module));
        }


        public string DatabaseName { get; }

        public SupportedObjectTypes[] SupportedTypes { get; }

        public ImageSource DatabaseIcon { get; }

        public IDbConnection GetDatabaseConnection { get; }

        public void SetConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public EdbSourceOption[] GetDefaultOptionsObjects()
        {
            throw new NotImplementedException();
        }

        public Guid ModuleGuid { get; }

        public void SetGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Version Version { get; }

        public void SetVersion(Version version)
        {
            throw new NotImplementedException();
        }
        */
    }
}
