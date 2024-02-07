

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

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