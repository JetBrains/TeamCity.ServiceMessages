

namespace JetBrains.TeamCity.ServiceMessages.Write
{
    using System;

    /// <summary>
    /// Object to represent service message attributes
    /// </summary>
    public struct ServiceMessageProperty
    {
        /// <summary>
        /// Key
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// Value
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Constructor of service message attribute
        /// </summary>
        /// <param name="key">service message key, must not contain escapable symbols, not null</param>
        /// <param name="value">value, notnull</param>
        public ServiceMessageProperty([NotNull] string key, [NotNull] string value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            Key = key;
            Value = value;
        }
    }
}