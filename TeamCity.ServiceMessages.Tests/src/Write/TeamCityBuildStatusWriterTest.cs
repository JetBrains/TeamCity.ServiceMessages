using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class TeamCityBuildStatusWriterTest : TeamCityWriterBaseTest<ITeamCityBuildStatusWriter>
  {
    protected override ITeamCityBuildStatusWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityBuildStatusWriter(proc);
    }

    [Test]
    public void TestBuildNumber()
    {
      DoTest(x => x.WriteBuildNumber("100500.5"), "##teamcity[buildNumber '100500.5']");
    }

    [Test]
    public void TestParameter()
    {
      DoTest(x => x.WriteBuildParameter("num", "100500.5"), "##teamcity[setParameter name='num' value='100500.5']");
    }

    [Test]
    public void TestStatistics()
    {
      DoTest(x => x.WriteBuildStatistics("num", "100500.5"), "##teamcity[buildStatisticValue key='num' value='100500.5']");
    }
  }
}