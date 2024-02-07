

namespace JetBrains.TeamCity.ServiceMessages.Write
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class to create complex service message. Use object initilizer to simplify code, i.e.
    /// <code>
    ///   new ServiceMessage("buildProblem") { { "identity", identity}, {"description", message}}
    /// </code>
    /// </summary>
    public class ServiceMessage : IServiceMessage, IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>();

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="message">service message to copy from</param>
        public ServiceMessage(IServiceMessage message)
            : this(message.Name)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            AddRange(message.Keys.ToDictionary(x => x, message.GetValue));
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="name">service message name</param>
        public ServiceMessage([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Name { get; }

        public string DefaultValue => null;

        public IEnumerable<string> Keys => _arguments.Keys;

        public string GetValue(string key)
        {
            string s;
            return _arguments.TryGetValue(key, out s) ? s : null;
        }

        public void Add(string key, string value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            _arguments[key] = value;
        }

        public void AddRange(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var e in values)
            {
                _arguments[e.Key] = e.Value;
            }
        }
    }
}