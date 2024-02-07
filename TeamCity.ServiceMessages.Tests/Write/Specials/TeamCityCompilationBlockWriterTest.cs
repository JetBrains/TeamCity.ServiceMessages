

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityCompilationBlockWriterTest : TeamCityWriterBaseTest<ITeamCityCompilationBlockWriter>
    {
        protected override ITeamCityCompilationBlockWriter Create(IServiceMessageProcessor proc)
        {
            return new TeamCityCompilationBlockWriter(proc);
        }

        [Test]
        public void TestOpenBlock()
        {
            DoTest(x => x.OpenCompilationBlock("aaa"), "##teamcity[compilationStarted compiler='aaa']");
        }

        [Test]
        public void TestOpenCloseBlock()
        {
            DoTest(x => x.OpenCompilationBlock("aaa").Dispose(), "##teamcity[compilationStarted compiler='aaa']", "##teamcity[compilationFinished compiler='aaa']");
        }
    }
}