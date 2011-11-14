namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityArtifactsWriter : BaseWriter, ITeamCityArtifactsWriter
  {
    public TeamCityArtifactsWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    public void PublishArtifact(string rules)
    {
      PostMessage(new ValueServiceMessage("publishArtifacts", rules));
    }
  }
}