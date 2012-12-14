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

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Write
{
  /// <summary>
  /// Provides service messages serialization for most cases
  /// </summary>
  public class ServiceMessageFormatter : IServiceMessageFormatter
  {
    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="singleValue">value</param>
    /// <returns>service message string</returns>
    public string FormatMessage(string messageName, string singleValue)
    {
      if (string.IsNullOrEmpty(messageName))
        throw new ArgumentNullException("messageName");
      if (singleValue == null)
        throw new ArgumentNullException("singleValue");

      if (ServiceMessageReplacements.Encode(messageName) != messageName)
        throw new ArgumentException("The message name contains illegal characters.", "messageName");

      return string.Format("{2}{0} '{1}'{3}", messageName, ServiceMessageReplacements.Encode(singleValue), ServiceMessageConstants.SERVICE_MESSAGE_OPEN, ServiceMessageConstants.SERVICE_MESSAGE_CLOSE);
    }

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="anonymousProperties">anonymous object containing all service message parameters</param>
    /// <returns>service message string</returns>
    public string FormatMessage(string messageName, object anonymousProperties)
    {
      if (string.IsNullOrEmpty(messageName))
        throw new ArgumentNullException("messageName");
      if (anonymousProperties == null)
        throw new ArgumentNullException("anonymousProperties");

      var properties = anonymousProperties.GetType().GetProperties();
      return FormatMessage(
        messageName,
        properties.Select(x => new ServiceMessageProperty(x.Name, x.GetValue(anonymousProperties, null).ToString())));
    }

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="properties">params array of service message properties</param>
    /// <returns>service message string</returns>
    public string FormatMessage(string messageName, params ServiceMessageProperty[] properties)
    {
      return FormatMessage(messageName, properties.ToList());
    }

    /// <summary>
    /// Serializes service message
    /// </summary>
    /// <param name="serviceMessage">parser service message</param>
    /// <returns></returns>
    public string FormatMessage(IServiceMessage serviceMessage)
    {
      if (serviceMessage.DefaultValue != null)
      {
        return FormatMessage(serviceMessage.Name, serviceMessage.DefaultValue);
      }
      return FormatMessage(serviceMessage.Name, serviceMessage.Keys.Select(key => new ServiceMessageProperty(key, serviceMessage.GetValue(key) ?? "")));
    }

    /// <summary>
    /// Serializes service message from IDictionary
    /// </summary>
    /// <param name="name">service message name</param>
    /// <param name="arguments">arguments</param>
    /// <returns></returns>
    public string FormatMessage(string name, IEnumerable<KeyValuePair<string, string>> arguments)
    {
      return FormatMessage(name, arguments.Select(key => new ServiceMessageProperty(key.Key, key.Value)));
    }

    /// <summary>
    /// Serializes single value service message
    /// </summary>
    /// <param name="messageName">message name</param>
    /// <param name="properties">params of service message properties</param>
    /// <returns>service message string</returns>
    public string FormatMessage(string messageName, IEnumerable<ServiceMessageProperty> properties)
    {
      if (messageName == null)
        throw new ArgumentNullException("messageName");
      if (string.IsNullOrEmpty(messageName))
        throw new ArgumentException("The message name must not be empty", "messageName");
      if (properties == null)
        throw new ArgumentNullException("properties");

      if (ServiceMessageReplacements.Encode(messageName) != messageName)
        throw new ArgumentException("The message name contains illegal characters", "messageName");

      if (ServiceMessageReplacements.Encode(messageName) != messageName)
        throw new ArgumentException("Message name contains illegal characters", "messageName");

      var sb = new StringBuilder();
      sb.Append(ServiceMessageConstants.SERVICE_MESSAGE_OPEN);
      sb.Append(messageName);

      foreach (ServiceMessageProperty property in properties)
      {
        if (string.IsNullOrEmpty(property.Key))
          throw new InvalidOperationException("The property name must not be empty");
        
        if (ServiceMessageReplacements.Encode(property.Key) != property.Key)
          throw new InvalidOperationException(string.Format("The property name “{0}” contains illegal characters", property.Key));

        sb.AppendFormat(" {0}='{1}'", property.Key, ServiceMessageReplacements.Encode(property.Value));
      }
      sb.Append(ServiceMessageConstants.SERVICE_MESSAGE_CLOSE);

      return sb.ToString();
    }
  }
}
