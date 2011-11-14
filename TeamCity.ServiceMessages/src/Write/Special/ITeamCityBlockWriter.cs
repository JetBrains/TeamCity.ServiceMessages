using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Generates pair of service messages like 
  /// <pre>##teamcity[blockOpened name='&lt;blockName>']</pre>
  /// and 
  /// <pre>##teamcity[blockClosed name='&lt;blockName>']</pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-BlocksofServiceMessages
  /// </summary>
  public interface ITeamCityBlockWriter
  {
    /// <summary>
    /// Generates open block message. To close the block, call Dispose to the given handle
    /// </summary>
    /// <param name="blockName">block name to report</param>
    /// <returns></returns>
    IDisposable OpenBlock([NotNull] string blockName);
  }
}