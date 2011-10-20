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

using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write
{
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
      Key = key;
      Value = value;
    }
  }
}