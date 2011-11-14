using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Factory interface for specialized service messages generation
  /// Create instance of <see cref="TeamCityServiceMessages"/> to get the implementation of the interface.
  /// </summary>
  public interface ITeamCityServiceMessages
  {
    /// <summary>
    /// Created writer that generates service messages to a Console.Out
    /// </summary>
    /// <remarks>
    /// Implementation does not support multiple-threads. 
    /// If you need to log more me
    /// </remarks>
    /// <returns></returns>
    [NotNull]
    ITeamCityWriter CreateWriter();

    /// <summary>
    /// Creates writer that translates service messages to the given 
    /// delegate. 
    /// </summary>
    /// <param name="destination">generated service messages processor</param>
    /// <returns></returns>
    [NotNull]
    ITeamCityWriter CreateWriter(Action<string> destination);
  }
}