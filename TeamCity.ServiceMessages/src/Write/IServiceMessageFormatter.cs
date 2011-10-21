using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Annotations;

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
  }
}