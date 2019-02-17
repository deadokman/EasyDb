using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.Tests.EnvironmentTests
{
    using Edb.Environment;
    using Edb.Environment.Interface;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests on OdbcRepository
    /// </summary>
    [TestClass]
    public class OdbcDriversExplorerTest
    {
        public IOdbcRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            _repository = new OdbcRepository();
        }

        /// <summary>
        /// Tests drivers list
        /// </summary>
        [TestMethod]
        public void ListOdbcDeriversTest()
        {
            var drivers = this._repository.ListOdbcDrivers();
            Assert.IsTrue(drivers.Any());
        }
    }
}
