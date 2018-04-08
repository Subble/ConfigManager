﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            const string folderName = "TestConfigManager";

            var config = new Config(folderName, host, None<ILogger>());

            Assert.IsNotNull(config);
            Assert.IsInstanceOfType(config, typeof(IConfigManager));

            const string stringKey = "test.string";
            const string stringValue = "StringValue1ç";
            config.Set(stringKey, stringValue);

            var config2 = new Config(folderName, host, None<ILogger>());

            var testStringValue = config2.Get<string>(stringKey);

            testStringValue
                .Some(v => Assert.AreEqual(stringValue, v))
                .None(() => Assert.Fail("No value was found for key:" + stringKey));
        }

    }
}
