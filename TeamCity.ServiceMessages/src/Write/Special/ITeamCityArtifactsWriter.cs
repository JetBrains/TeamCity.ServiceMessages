using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Service messages for dynamically publish artifacts.
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-PublishingArtifactswhiletheBuildisStillinProgress
  /// </summary>
  public interface ITeamCityArtifactsWriter
  {
    /// <summary>
    /// attaches new artifact publishing rules as described in 
    /// http://confluence.jetbrains.net/display/TCD7/Build+Artifact
    /// </summary>
    /// <param name="rules"></param>
    void PublishArtifact([NotNull] string rules);
  }
}