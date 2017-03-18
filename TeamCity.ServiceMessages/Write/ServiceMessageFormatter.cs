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

namespace JetBrains.TeamCity.ServiceMessages.Write
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if !NET35 && !NET40
    using System.Reflection;
#endif
    using System.Text;

    /// <summary>
    ///     Provides service messages serialization for most cases
    /// </summary>
    public class ServiceMessageFormatter : IServiceMessageFormatter
    {
        /// <summary>
        ///     Serializes single value service message
        /// </summary>
        /// <param name="messageName">message name</param>
        /// <param name="singleValue">value</param>
        /// <returns>service message string</returns>
        public string FormatMessage([NotNull] string messageName, [NotNull] string singleValue)
        {
            if (string.IsNullOrEmpty(messageName)) throw new ArgumentNullException(nameof(messageName));
            if (singleValue == null) throw new ArgumentNullException(nameof(singleValue));
            if (ServiceMessageReplacements.Encode(messageName) != messageName) throw new ArgumentException("The message name contains illegal characters.", nameof(messageName));
            return string.Format("{2}{0} '{1}'{3}", messageName, ServiceMessageReplacements.Encode(singleValue), ServiceMessageConstants.ServiceMessageOpen, ServiceMessageConstants.ServiceMessageClose);
        }

        /// <summary>
        ///     Serializes single value service message
        /// </summary>
        /// <param name="messageName">message name</param>
        /// <param name="anonymousProperties">anonymous object containing all service message parameters</param>
        /// <returns>service message string</returns>
        public string FormatMessage([NotNull] string messageName, [NotNull] object anonymousProperties)
        {
            if (string.IsNullOrEmpty(messageName)) throw new ArgumentNullException(nameof(messageName));
            if (anonymousProperties == null) throw new ArgumentNullException(nameof(anonymousProperties));

            var propType = anonymousProperties.GetType();
#if !NET35 && !NET40
            var properties = propType.GetTypeInfo().DeclaredProperties;
#else
            var properties = propType.GetProperties();
#endif
            return FormatMessage(
                messageName,
                properties.Select(x => new ServiceMessageProperty(x.Name, x.GetValue(anonymousProperties, null).ToString())));
        }

        /// <summary>
        ///     Serializes single value service message
        /// </summary>
        /// <param name="messageName">message name</param>
        /// <param name="properties">params array of service message properties</param>
        /// <returns>service message string</returns>
        public string FormatMessage([NotNull] string messageName, [NotNull] params ServiceMessageProperty[] properties)
        {
            if (messageName == null) throw new ArgumentNullException(nameof(messageName));
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            return FormatMessage(messageName, properties.ToList());
        }

        /// <summary>
        ///     Serializes service message
        /// </summary>
        /// <param name="serviceMessage">parser service message</param>
        /// <returns></returns>
        public string FormatMessage(IServiceMessage serviceMessage)
        {
            if (serviceMessage == null) throw new ArgumentNullException(nameof(serviceMessage));
            if (serviceMessage.DefaultValue != null)
            {
                return FormatMessage(serviceMessage.Name, serviceMessage.DefaultValue);
            }

            return FormatMessage(serviceMessage.Name, serviceMessage.Keys.Select(key => new ServiceMessageProperty(key, serviceMessage.GetValue(key) ?? "")));
        }

        /// <summary>
        ///     Serializes service message from IDictionary
        /// </summary>
        /// <param name="name">service message name</param>
        /// <param name="arguments">arguments</param>
        /// <returns></returns>
        public string FormatMessage(string name, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));
            return FormatMessage(name, arguments.Select(key => new ServiceMessageProperty(key.Key, key.Value)));
        }

        /// <summary>
        ///     Serializes single value service message
        /// </summary>
        /// <param name="messageName">message name</param>
        /// <param name="properties">params of service message properties</param>
        /// <returns>service message string</returns>
        public string FormatMessage(string messageName, IEnumerable<ServiceMessageProperty> properties)
        {
            if (messageName == null) throw new ArgumentNullException(nameof(messageName));
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (messageName == null) throw new ArgumentNullException(nameof(messageName));
            if (string.IsNullOrEmpty(messageName)) throw new ArgumentException("The message name must not be empty", nameof(messageName));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            if (ServiceMessageReplacements.Encode(messageName) != messageName)
            {
                throw new ArgumentException("The message name contains illegal characters", nameof(messageName));
            }

            if (ServiceMessageReplacements.Encode(messageName) != messageName)
            {
                throw new ArgumentException("Message name contains illegal characters", nameof(messageName));
            }

            var sb = new StringBuilder();
            sb.Append(ServiceMessageConstants.ServiceMessageOpen);
            sb.Append(messageName);

            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property.Key))
                {
                    throw new InvalidOperationException("The property name must not be empty");
                }

                if (ServiceMessageReplacements.Encode(property.Key) != property.Key)
                {
                    throw new InvalidOperationException($"The property name “{property.Key}” contains illegal characters");
                }

                sb.AppendFormat(" {0}='{1}'", property.Key, ServiceMessageReplacements.Encode(property.Value));
            }

            sb.Append(ServiceMessageConstants.ServiceMessageClose);
            return sb.ToString();
        }
    }
}