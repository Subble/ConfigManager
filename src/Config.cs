using Subble.Core;
using Subble.Core.Config;
using Subble.Core.Func;
using System.IO;
using System.Collections.Generic;
using Subble.Core.Logger;
using System.Linq;

using static Subble.Core.Logger.SubbleLog;
using static Subble.Core.Events.EventsType.Core;
using static Subble.Core.Func.Option;
using static System.Convert;
using System;

namespace ConfigManager
{
    public class Config : IConfigManager
    {
        private readonly ISubbleHost _host;
        private readonly Option<ILogger> _logger;
        private readonly Dictionary<string, string> _store;

        public Config(ISubbleHost host, Option<ILogger> logger)
        {
            _host = host;

            ConfigFilePath = BuildFilePath(host.WorkingDirectory.FullName);
            _logger = logger;
            _store = LoadStore(ReadSettingsFile(ConfigFilePath));
        }

        public string ConfigFilePath { get; }

        public bool Delete(string key)
        {
            if (!_store.ContainsKey(key))
                return false;

            _store.Remove(key);
            SaveConfiguration();

            return true;
        }

        public Option<T> Get<T>(string key) where T: IConvertible
        {
            if (!_store.ContainsKey(key))
                return None<T>();

            var value = ChangeType(_store[key], typeof(T));
            return Some(value).ToTyped<T>();
        }

        public bool IsAvailable(string key)
            => _store.ContainsKey(key);

        public void Set<T>(string key, T value) where T: IConvertible
        {
            var convertType = ChangeType(value, typeof(string)) as string;
            if(convertType is null)
                throw new InvalidCastException($"Type ${typeof(T)} convert instance to null string");

            _store[key] = convertType;
            SaveConfiguration();
        }

        private string BuildFilePath(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, ".config");
        }

        private string[] ReadSettingsFile(string file)
        {
            if (!File.Exists(file))
                return new string[0];

            return File.ReadAllLines(file);
        }

        private void SaveConfiguration()
        {
            string ToConfigEntry(string k, string v) => $"{k}={v}";
            var configEntries = _store.Select(e => ToConfigEntry(e.Key, e.Value));
            File.WriteAllLines(ConfigFilePath, configEntries);
        }

        private Dictionary<string, string> LoadStore(string[] lines)
        {
            var store = new Dictionary<string, string>();

            foreach (var entry in lines)
            {
                var items = entry.Split('=');

                //Skip lines without assignments
                if (items.Length < 2) continue;

                var key = items[0];

                if (store.ContainsKey(key))
                {
                    LogWarning($"Settings file contains duplicate value: {key}");
                    continue;
                }

                //Calculate value, make sure that the value isn't empty
                var keyLength = key.Length + 1;
                var value = entry.Length > keyLength ? entry.Substring(key.Length + 1) : "";

                store.Add(key, value);
            }

            return store;
        }

        /// <summary>
        /// Logs to the system a warning
        /// </summary>
        /// <param name="message">the message to warn</param>
        private void LogWarning(string message)
        {
            void DefaultLog() => _host.EmitEvent<ILog>(LOG, "IConfigManager", GetWarningLog(message));

            _logger
                .None(DefaultLog)
                .Some(l => l.LogWarning("IConfigManager", message));
        }

        public T Get<T>(string key, T defaultValue) where T : IConvertible
        {
            // try to get value
            var res = Get<T>(key);

            // if there are no value, the default one is set
            res.None(() => Set(key, defaultValue));

            // return the value
            return res.HasValue(out var val) ? val : defaultValue;
        }
    }
}
