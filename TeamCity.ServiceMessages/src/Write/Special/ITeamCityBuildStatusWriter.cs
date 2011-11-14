/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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