using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subble.Core;
using Subble.Core.Config;
using Subble.Core.Logger;
using System.IO;
using static Subble.Core.Func.Option;

namespace ConfigManager.Tests
{
    [TestClass]
    public class ConfigTest
    {
        private ISubbleHost GetHostMock()
        {
            var mock = new Mock<ISubbleHost>();

            return mock.Object;
        }

        [TestMethod]
        public void TestConfigInit()
        {
            var host = GetHostMock();
            var folderName = "TestConfigManager";

            var config = new Config("TestConfigManager", host, None<ILogger>());

            Assert.IsNotNull(config);
            Assert.IsInstanceOfType(config, typeof(IConfigManager));

            const string stringKey = "test.string";
            const string stringValue = "StringValue1ç";
            config.Set(stringKey, stringValue);



            var testStringValue = config.Get<string>(stringKey);
            Assert.AreEqual(stringValue, testStringValue);
        }

    }
}
