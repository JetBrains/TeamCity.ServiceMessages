

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

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
        public void TestBuildProblem()
        {
            DoTest(x => x.WriteBuildProblem("id5", "aaaa"), "##teamcity[buildProblem identity='id5' description='aaaa']");
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