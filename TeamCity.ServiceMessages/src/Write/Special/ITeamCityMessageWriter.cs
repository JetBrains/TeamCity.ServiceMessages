using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
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