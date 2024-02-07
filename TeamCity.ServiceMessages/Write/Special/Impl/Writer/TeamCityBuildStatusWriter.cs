

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class TeamCityBuildStatusWriter : BaseWriter, ITeamCityBuildStatusWriter
    {
        public TeamCityBuildStatusWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }

        public void WriteBuildNumber(string buildNumber)
        {
            if (buildNumber == null) throw new ArgumentNullException(nameof(buildNumber));
            PostMessage(new ValueServiceMessage("buildNumber", buildNumber));
        }

        public void WriteBuildProblem(string identity, string message)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (identity.Length >= 60)
            {
                throw new ArgumentOutOfRangeException(nameof(identity), "Value is too big. Only 60 chars are allowed");
            }

            PostMessage(new ServiceMessage("buildProblem") {{"identity", identity}, {"description", message}});
        }

        public void WriteBuildParameter(string parameterName, string parameterValue)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (parameterValue == null) throw new ArgumentNullException(nameof(parameterValue));
            //##teamcity[setParameter name='ddd' value='fff']
            PostMessage(new ServiceMessage("setParameter") {{"name", parameterName}, {"value", parameterValue}});
        }

        public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
        {
            if (statisticsKey == null) throw new ArgumentNullException(nameof(statisticsKey));
            if (statisticsValue == null) throw new ArgumentNullException(nameof(statisticsValue));
            //##teamcity[buildStatisticValue key='<valueTypeKey>' value='<value>']
            PostMessage(new ServiceMessage("buildStatisticValue") {{"key", statisticsKey}, {"value", statisticsValue}});
        }
    }
}