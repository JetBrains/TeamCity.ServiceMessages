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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityBuildStatusWriter : BaseWriter, ITeamCityBuildStatusWriter
  {
    public TeamCityBuildStatusWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    public void WriteBuildNumber(string buildNumber)
    {
      PostMessage(new ValueServiceMessage("buildNumber", buildNumber));
    }

    public void WriteBuildParameter(string parameterName, string parameterValue)
    {
      //##teamcity[setParameter name='ddd' value='fff']
      PostMessage(new SimpleServiceMessage("setParameter"){{"name", parameterName}, {"value", parameterValue}});
    }

    public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
    {
      //##teamcity[buildStatisticValue key='<valueTypeKey>' value='<value>']
      PostMessage(new SimpleServiceMessage("buildStatisticValue"){{"key", statisticsKey}, {"value", statisticsValue}});
    }
  }
}