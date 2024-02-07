

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    /// <summary>
    /// Interface for writing build-related messages
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingBuildNumber
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityBuildStatusWriter
    {
        /// <summary>
        /// Generates service message to update build number.
        /// </summary>
        /// <param name="buildNumber"></param>
        void WriteBuildNumber([NotNull] string buildNumber);


        /// <summary>
        /// Generates build problem service message
        /// </summary>
        /// <param name="identity">problem unique identity, no more than 60 chars</param>
        /// <param name="description">problem message</param>
        void WriteBuildProblem([NotNull] string identity, [NotNull] string description);

        /// <summary>
        /// Generates service message to update build parameter
        /// http://confluence.jetbrains.net/display/TCD18/Configuring+Build+Parameters
        /// </summary>
        /// <param name="parameterName">
        /// parameter name, could start with env. for environment, system. for system property,
        /// otherwise it would be config parameter
        /// </param>
        /// <param name="parameterValue">value</param>
        void WriteBuildParameter([NotNull] string parameterName, [NotNull] string parameterValue);


        /// <summary>
        /// Generates service message to report build statistics values
        /// http://confluence.jetbrains.net/display/TCD18/Customizing+Statistics+Charts#CustomizingStatisticsCharts-customCharts
        /// </summary>
        /// <param name="statisticsKey">statistics report key</param>
        /// <param name="statisticsValue">statistics report values</param>
        void WriteBuildStatistics([NotNull] string statisticsKey, [NotNull] string statisticsValue);
    }
}