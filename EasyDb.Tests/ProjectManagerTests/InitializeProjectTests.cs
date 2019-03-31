using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Autofac.Extras.NLog;
using EasyDb.Model;
using EasyDb.ProjectManagment;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ProjectManagment.ProjectSchema;
using Edb.Environment.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EasyDb.Tests.ProjectManagerTests
{
    [TestFixture]
    public class InitializeProjectTests
    {
        public string TestDir = Path.Combine(Directory.GetCurrentDirectory(), "testFolder");
        public IProjectEnvironment _projEnv;
        public XmlSerializer _historySeri;
        public string ProjFolder { get; set; }
        public string ProjFile { get; set; }

        [OneTimeSetUp]
        public void Initialize()
        {
            _historySeri = new XmlSerializer(typeof(HistoryInformation));
           _projEnv = new ProjectEnvironment(Mock.Of<IDataSourceManager>(), Mock.Of<ILogger>());
            if (Directory.Exists(TestDir))
            {
                Directory.Delete(TestDir, true);
            }

            Directory.CreateDirectory(TestDir);
            ProjFolder = Path.Combine(TestDir, _projEnv.EdbProjectFolder);
            ProjFile = Path.Combine(TestDir, _projEnv.EdbProjectFolder, _projEnv.EdbProjectFile);
        }

        [Order(0)]
        [Test]
        public void TestProjectEnvInitialize()
        {
            var histTestInfo = new HistoryInformation();
            histTestInfo.ProjectsHistory = new List<ProjectHistItem>();
            var ehi = new ProjectHistItem()
            {
                LastAccess = DateTime.Today,
                Pinned = true,
                ProjectFileLocation = TestDir,
                ProjectName = "Some project name"
            };

            histTestInfo.ProjectsHistory.Add(ehi);

            using (var s = File.Create(Path.Combine(TestDir, _projEnv.HistoryInformationFile)))
            {
                _historySeri.Serialize(s, histTestInfo);
            }

            _projEnv.Initialize("./SourceFiles", TestDir);
            var hi = _projEnv.HistoryInformation.ProjectsHistory.FirstOrDefault();
            Assert.IsNotNull(hi);
            Assert.AreEqual(ehi.LastAccess, hi.LastAccess);
            Assert.AreEqual(ehi.Pinned, hi.Pinned);
            Assert.AreEqual(ehi.ProjectFileLocation, hi.ProjectFileLocation);
            Assert.AreEqual(ehi.ProjectName, hi.ProjectName);
        }

        [Order(1)]
        [Test]
        public void TestInitEmptyProject()
        {
            _projEnv.InitializeNewProject(TestDir).Wait();
            Assert.IsTrue(Directory.Exists(ProjFolder));
            Assert.IsTrue(File.Exists(ProjFile));
            Assert.IsNotNull(_projEnv.CurrentProject);
            Assert.AreEqual(TestDir, _projEnv.CurrentProject.ProjectFolderPath);
            Assert.AreEqual(ProjFile, _projEnv.CurrentProject.ProjectFullPath);
        }

        [Order(2)]
        [Test]
        public void TestUpgradeOrjectConfig()
        {
            var udsConf = new UserDatasourceConfiguration()
            {
                ConfigurationGuid = Guid.NewGuid(),
                DatasoureGuid = Guid.NewGuid(),
                Name = "Some conf",
                Comment = "Comment"
            };

            _projEnv.ApplyDatasourceConfig(udsConf);
            Assert.IsTrue(File.Exists(ProjFile));
            var xseri = new XmlSerializer(typeof(EasyDbProject));
            using (var f = File.OpenRead(ProjFile))
            {
                var proj = ((EasyDbProject)xseri.Deserialize(f)).ConfigurationSources.FirstOrDefault();
                Assert.IsNotNull(proj);
                Assert.AreEqual(udsConf.ConfigurationGuid, proj.ConfigurationGuid);
                Assert.AreEqual(udsConf.DatasoureGuid, proj.DatasoureGuid);
                Assert.AreEqual(udsConf.Name, proj.Name);
                Assert.AreEqual(udsConf.Comment, proj.Comment);
            }
        }
    }
}
