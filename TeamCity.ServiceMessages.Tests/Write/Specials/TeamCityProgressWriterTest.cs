namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityProgressWriterTest : TeamCityWriterBaseTest<ITeamCityProgressWriter>
    {
        protected override ITeamCityProgressWriter Create(IServiceMessageProcessor proc)
        {
            return new TeamCityProgressWriter(proc);
        }

        [Test]
        public void TestProgressMessage()
        {
            DoTest(x => x.WriteProgress("aaaa"), "##teamcity[progressMessage 'aaaa']");
        }

        [Test]
        public void OpenProgressBlock()
        {
            DoTest(x => x.OpenProgress("aaaa").Dispose(),
                "##teamcity[progressStart 'aaaa']",
                "##teamcity[progressFinish 'aaaa']");
        }
    }
}