using System.Collections.Generic;
using System.IO;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  /// <summary>
  /// Provides service messages parsing from stream
  /// </summary>
  public interface IServiceMessageParser
  {
    /// <summary>
    /// Lazy parses service messages from string
    /// </summary>
    /// <param name="text">text to parse</param>
    /// <returns>enumerable of service messages</returns>
    IEnumerable<IServiceMessage> ParseServiceMessages([NotNull] string text);

    /// <summary>
    /// Reads stream parsing service messages from it.
    /// </summary>
    /// <param name="reader">stream to parse. Stream will not be closed</param>
    /// <returns>Iterator of service messages</returns>
    IEnumerable<IServiceMessage> ParseServiceMessages([NotNull] TextReader reader);
  }
}