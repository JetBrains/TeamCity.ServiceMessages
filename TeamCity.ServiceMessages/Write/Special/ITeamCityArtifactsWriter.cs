

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    /// <summary>
    /// Service messages for dynamically publish artifacts.
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-PublishingArtifactswhiletheBuildisStillinProgress
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityArtifactsWriter
    {
        /// <summary>
        /// attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD18/Build+Artifact
        /// </summary>
        /// <param name="rules"></param>
        void PublishArtifact([NotNull] string rules);
    }
}