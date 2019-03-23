﻿using System;
using System.Data;
using EasyDb.Model;

namespace Edb.Environment.Interface
{
    /// <summary>
    /// Easy DB connection link 
    /// </summary>
    public interface IEDbConnectionLink : IDisposable, IDbConnection
    {
        /// <summary>
        /// Reference to real database connection
        /// </summary>
        IDbConnection UnderlyingConnection { get; }

        /// <summary>
        /// Edb datasource GUID
        /// </summary>
        Guid DatasourceGuid { get; }

        /// <summary>
        /// Connection config object
        /// </summary>
        UserDatasourceConfiguration ConnectionConfiguration { get;  }

        /// <summary>
        /// Validates that connection active
        /// </summary>
        /// <returns>Returns true if connection active</returns>
        bool ConnectionActive();
    }
}