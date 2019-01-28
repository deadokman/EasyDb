using System.Collections.Generic;
using EasyDb.CustomControls;
using EDb.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyDb.Tests.Controltest
{
    public class TestOptions : EdbSourceOption
    {
        [OptionDisplayName("test1", "PropNameOneSetterVal")]
        public string PropOneSetter { get; set; } = "TestVal1";

        [OptionDisplayName("test2", "PropNameOneGetterVal")]
        public string PropOneGetter { get; private set; } = "TestVal2";

        [OptionDisplayName("test3", "PropNameTwoGetterVal")]
        public bool PropTwoGetter { get; private set; } = true;
    }

    [TestClass]
    public class UserDatasouceSettingsControlTest
    {
        private UserDatasouceSettingsControl _testTarget;
        public UserDatasouceSettingsControlTest()
        {
            _testTarget = new UserDatasouceSettingsControl();
        }

        [TestMethod]
        public void ValidateOptionsBuildTest()
        {
            var tClass = new TestOptions();
            var result = UserDatasouceSettingsControl.FormatDatasourceOptions(tClass, tClass.GetType(), new Dictionary<string, string>() { {  "test3", "TestResourceSet" } });
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("PropNameOneSetterVal", result[0].OptionName);
            Assert.AreEqual(false, result[0].IsReadOnly);
            Assert.AreEqual("TestVal1", result[0].Value);
            Assert.AreEqual("System.String", result[0].OptionEditType);
            //
            Assert.AreEqual("PropNameOneGetterVal", result[1].OptionName);
            // Private setter sould be read only
            Assert.AreEqual(true, result[1].IsReadOnly);
            Assert.AreEqual("TestVal2", result[1].Value);
            Assert.AreEqual("System.String", result[1].OptionEditType);
            //
            Assert.AreEqual("TestResourceSet", result[2].OptionName);
            Assert.AreEqual(true, result[2].IsReadOnly);
            Assert.AreEqual(true, result[2].Value);
            Assert.AreEqual("System.Boolean", result[2].OptionEditType);
        }
    }
}
