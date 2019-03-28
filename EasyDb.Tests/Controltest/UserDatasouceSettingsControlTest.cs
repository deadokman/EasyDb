using System.Collections.Generic;
using EasyDb.CustomControls;
using EDb.Interfaces;

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
}
