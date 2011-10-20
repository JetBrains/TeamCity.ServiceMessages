/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  internal class ServiceMessage : IServiceMessage
  {
    private readonly Dictionary<string, string> myProperties;

    public string Name { get; private set; }

    public string DefaultValue { get; private set; }

    public ServiceMessage([NotNull] string name, [CanBeNull] string defaultValue = null, [NotNull] Dictionary<string, string> properties = null)
    {
      Name = name;
      DefaultValue = properties != null ? null : defaultValue;
      myProperties = properties ?? new Dictionary<string, string>();
    }

    public string GetValue(string key)
    {
      string s;
      return myProperties.TryGetValue(key, out s) ? s : null;
    }

    public IEnumerable<string> Keys
    {
      get { return myProperties.Keys; }
    }
  }
}