#if DEBUG
using Xunit;
using Moq;
using Subble.Core;
using Subble.Core.Config;
using Subble.Core.Logger;
using System.IO;
using static Subble.Core.Func.Option;

namespace ConfigManager.Tests
{
    public class ConfigTest
    {
        private ISubbleHost GetHostMock()
        {
            var mock = new Mock<ISubbleHost>();

            mock.SetupGet(h => h.WorkingDirectory).Returns(new DirectoryInfo("."));

            return mock.Object;
        }

        [Fact]
        public void TestConfigInit()
        {
            var host = GetHostMock();

            var config = new Config(host, None<ILogger>());

            Assert.NotNull(config);
            Assert.True(config is IConfigManager);

            const string stringKey = "test.string";
            const string stringValue = "StringValue1ç";
            config.Set(stringKey, stringValue);

            var config2 = new Config(host, None<ILogger>());

            var testStringValue = config2.Get<string>(stringKey);

            testStringValue
                .Some(v => Assert.Equal(stringValue, v))
                .None(() => Assert.True(false, "No value was found for key:" + stringKey));
        }

    }
}
#endif