

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;
    using System.Collections.Generic;

    internal class ValueServiceMessage : IServiceMessage
    {
        public ValueServiceMessage([NotNull] string name, [CanBeNull] string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            DefaultValue = value;
        }

        public string Name { get; }

        public string DefaultValue { get; }

        public IEnumerable<string> Keys
        {
            get { yield break; }
        }

        public string GetValue(string key)
        {
            return null;
        }
    }
}