using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class TeamCityArtifactsWriterTest : TeamCityWriterBaseTest<ITeamCityArtifactsWriter>
  {
    protected override ITeamCityArtifactsWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityArtifactsWriter(proc);
    }

    [Test]
    public void SendArtifact()
    {
      DoTest(x => x.PublishArtifact("this is artifact"), "##teamcity[publishArtifacts 'this is artifact']");
    }
  }
}