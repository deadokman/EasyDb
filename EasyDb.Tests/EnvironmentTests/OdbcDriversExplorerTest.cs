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
    /// Tests on OdbcManager
    /// </summary>
    [TestClass]
    public class OdbcDriversExplorerTest
    {
        public IOdbcManager Manager;

        [TestInitialize]
        public void SetUp()
        {
            Manager = new OdbcManager();
        }

        /// <summary>
        /// Tests drivers list
        /// </summary>
        [TestMethod]
        public void ListOdbcDeriversTest()
        {
            var drivers = this.Manager.ListOdbcDrivers();
            Assert.IsTrue(drivers.Any());
        }
    }
}
