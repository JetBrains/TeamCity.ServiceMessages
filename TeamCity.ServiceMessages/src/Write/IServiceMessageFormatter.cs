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
using JetBrains.TeamCity.ServiceMessages.Read;

namespace JetBrains.TeamCity.ServiceMessages.Write
{
  /// <summary>
  /// Provides service messages serialization for most cases
  /// </summary>
  public interface IServiceMessageFormatter
  {
    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="singleValue">value</param>
    /// <returns>service message string</returns>
    string FormatMessage([NotNull] string messageName, [NotNull] string singleValue);

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="anonymousProperties">anonymous object containing all service message parameters</param>
    /// <returns>service message string</returns>
    string FormatMessage([NotNull] string messageName, [NotNull] object anonymousProperties);

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="properties">params array of service message properties</param>
    /// <returns>service message string</returns>
    string FormatMessage([NotNull] string messageName, [NotNull] params ServiceMessageProperty[] properties);

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="properties">params of service message properties</param>
    /// <returns>service message string</returns>
    string FormatMessage([NotNull] string messageName, [NotNull] IEnumerable<ServiceMessageProperty> properties);

    /// <summary>
    /// Serializes service message
    /// </summary>
    /// <param name="serviceMessage"></param>
    /// <returns></returns>
    string FormatMessage([NotNull] IServiceMessage serviceMessage);

    /// <summary>
    /// Serializes service message from IDictionary
    /// </summary>
    /// <param name="name">service message name</param>
    /// <param name="arguments">arguments</param>
    /// <returns></returns>
    string FormatMessage([NotNull] string name, [NotNull] IEnumerable<KeyValuePair<string, string>> arguments);
  }
}