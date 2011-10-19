using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  public static class ServiceMessageParser
  {
    /// <summary>
    /// Reads stream parsing service messages from it.
    /// </summary>
    /// <param name="reader">stream to parse. Stream will not be closed</param>
    /// <returns>Iterator of service messages</returns>
    public static IEnumerable<ServiceMessage> ParseServiceMessages([NotNull] TextReader reader)
    {
      yield break;
    }
  }
}