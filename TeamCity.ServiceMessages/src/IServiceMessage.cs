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

namespace JetBrains.TeamCity.ServiceMessages
{
  /// <summary>
  /// Object representation of TeamCity service message
  /// </summary>
  public interface IServiceMessage
  {
    /// <summary>
    /// Service message name, i.e. messageName in ##teamcity[messageName 'ddd']. 
    /// </summary>
    [NotNull]
    string Name { get; }

    /// <summary>
    /// For one-value service messages returns value, i.e. 'aaa' for ##teamcity[message 'aaa']
    /// or <code>null</code> otherwise, i.e. ##teamcity[message aa='aaa']
    /// </summary>
    [CanBeNull]
    string DefaultValue { get; }

    /// <summary>
    /// Emptry for one-value service messages, i.e. ##teamcity[message 'aaa'], returns all keys otherwise
    /// </summary>
    IEnumerable<string> Keys { get; }

    /// <summary>
    /// Return a value for keys or <code>null</code>
    /// </summary>
    /// <param name="key">Key to check for value</param>
    /// <returns>value of available or <code>null</code></returns>
    [CanBeNull]
    string GetValue([NotNull] string key);
  }
}