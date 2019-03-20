using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDb.Model;
using Edb.Environment.Interface;
using EDb.Interfaces.Annotations;

namespace Edb.Environment.Model
{
    public class EDbConnectionLink : IEDbConnectionLink
    {
        private readonly UserDatasourceConfiguration _configuration;
        private readonly IDbConnection _dbConnectionInstance;

        public EDbConnectionLink([NotNull] UserDatasourceConfiguration configuration, [NotNull] IDbConnection dbConnectionInstance)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._dbConnectionInstance = dbConnectionInstance ?? throw new ArgumentNullException(nameof(dbConnectionInstance));
        }

        public void Dispose()
        {
            _dbConnectionInstance.Dispose();
        }

        /// <summary>
        /// Reference to real database connection
        /// </summary>
        public IDbConnection DatabaseConnection { get; set; }

        /// <summary>
        /// Edb datasource GUID
        /// </summary>
        public Guid DatasourceGuid => this.ConnectionConfiguration.DatasoureGuid;

        /// <summary>
        /// Connection config object
        /// </summary>
        public UserDatasourceConfiguration ConnectionConfiguration => _configuration;

        /// <summary>
        /// Validates that connection active
        /// </summary>
        /// <returns>Returns true if connection active</returns>
        public bool ConnectionActive()
        {
            throw new NotImplementedException();
        }
    }
}
