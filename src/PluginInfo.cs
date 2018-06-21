using Subble.Core.Plugin;
using System;

namespace ConfigManager
{
    public class PluginInfo : IPluginInfo
    {
        public string GUID => "8a26b5f8-3d25-440c-8da9-c3f0bed34232";

        public string Name => "ConfigurationManager";

        public string Creator => "David Pires";

        public string Repository => "https://github.com/Subble/ConfigManager";

        public string Support => "https://github.com/Subble/ConfigManager/issues";

        public string Licence => "MIT";
    }
}
