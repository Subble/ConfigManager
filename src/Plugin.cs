using System;
using System.Collections.Generic;
using Subble.Core;
using Subble.Core.Config;
using Subble.Core.Logger;
using Subble.Core.Plugin;

namespace ConfigManager
{
    public class Plugin : ISubblePlugin
    {
        public IPluginInfo Info
            => new PluginInfo();

        public SemVersion Version
            => new SemVersion(0, 1, 0);

        public long LoadPriority => 5;

        public IEnumerable<Dependency> Dependencies
            => new[]
            {
                new Dependency(typeof(ILogger), 0, 0, 1)
            };


        public bool Initialize(ISubbleHost host)
        {
            if (host is null)
                return false;

            var logger = host.ServiceContainer.GetService<ILogger>();

            var config = new Config(host, logger);
            return host.ServiceContainer.RegisterService<IConfigManager>(config, Version);
        }
    }
}
