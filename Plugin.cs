using System.Collections.Generic;
using Subble.Core;
using Subble.Core.Logger;
using Subble.Core.Plugin;

namespace ConfigManager
{
    public class Plugin : ISubblePlugin
    {
        public IPluginInfo Info
            => new PluginInfo();

        public SemVersion Version
            => new SemVersion(0, 0, 1);

        public long LoadPriority => 5;

        public IEnumerable<Dependency> Dependencies
            => new[]
            {
                new Dependency(typeof(ILogger), 0, 0, 1)
            };


        public bool Initialize(ISubbleHost host)
        {
            return true;
        }
    }
}
