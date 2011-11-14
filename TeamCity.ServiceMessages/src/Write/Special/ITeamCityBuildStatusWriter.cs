using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Interface for writing build-related messages
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingBuildNumber
  /// </summary>
  public interface ITeamCityBuildStatusWriter
  {
    /// <summary>
    /// Generates service message to update build number.
    /// </summary>
    /// <param name="buildNumber"></param>
    void WriteBuildNumber([NotNull] string buildNumber);


    /// <summary>
    /// Generates service message to update build parameter
    /// 
    /// http://confluence.jetbrains.net/display/TCD7/Configuring+Build+Parameters
    /// </summary>
    /// <param name="parameterName">parameter name, could start with env. or system. </param>
    /// <param name="parameterValue">value</param>
    /// 
    void WriteBuildParameter([NotNull] string parameterName, [NotNull] string parameterValue);


    /// <summary>
    /// Generates service message to report build statistics values
    /// 
    /// http://confluence.jetbrains.net/display/TCD7/Customizing+Statistics+Charts#CustomizingStatisticsCharts-customCharts
    /// </summary>
    /// <param name="statisticsKey">statistics report key</param>
    /// <param name="statisticsValue">statistics report values</param>
    void WriteBuildStatistics([NotNull] string statisticsKey, [NotNull] string statisticsValue);
  }
}