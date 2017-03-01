/*
 * Copyright 2007-2017 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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