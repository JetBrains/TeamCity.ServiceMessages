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