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

  /// <summary>
  /// Introduces compilation block.
  /// <pre>##teamcity[compilationStarted compiler='&lt;compiler name>']</pre>
  /// and
  /// <pre>##teamcity[compilationFinished compiler='&lt;compiler name>']</pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingCompilationMessages
  /// </summary>
  public interface ITeamCityCompilationBlockWriter
  {
    /// <summary>
    /// Generates open compilation block. To close block call Dispose to the given handle
    /// </summary>
    /// <param name="compilerName"></param>
    /// <returns></returns>
    IDisposable OpenCompilationBlock([NotNull] string compilerName);
  }


  /// <summary>
  /// Add a build log entry message
  /// <pre>
  /// ##teamcity[message text='&lt;message text>' errorDetails='&lt;error details>' status='&lt;status value>']
  /// </pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingMessagesForBuildLog
  /// </summary>
  public interface ITeamCityMessageWriter
  {
    /// <summary>
    /// Writes normal message
    /// </summary>
    /// <param name="text">text</param>
    void WriteMessage([NotNull] string text);

    /// <summary>
    /// Writes warning message
    /// </summary>
    /// <param name="text">text</param>
    void WriteWarning([NotNull] string text);

    /// <summary>
    /// Writes error message with details
    /// </summary>
    /// <param name="text">text</param>
    /// <param name="errorDetails">error details</param>
    void WriteError([NotNull] string text, [CanBeNull] string errorDetails);    
  }
}