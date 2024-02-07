

namespace JetBrains.TeamCity.ServiceMessages.Read
{
    using System;
    using System.Collections.Generic;

    internal class ServiceMessage : IServiceMessage
    {
        private readonly Dictionary<string, string> _properties;

        public ServiceMessage([NotNull] string name, [CanBeNull] string defaultValue = null, [CanBeNull] Dictionary<string, string> properties = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            DefaultValue = properties != null ? null : defaultValue;
            _properties = properties ?? new Dictionary<string, string>();
        }

        public string Name { get; }

        public string DefaultValue { get; }

        public string GetValue(string key)
        {
            string s;
            return _properties.TryGetValue(key, out s) ? s : null;
        }

        public IEnumerable<string> Keys => _properties.Keys;
    }
}