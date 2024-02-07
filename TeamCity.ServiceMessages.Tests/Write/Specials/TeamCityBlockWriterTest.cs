

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityBlockWriterTest : TeamCityWriterBaseTest<TeamCityBlockWriter>
    {
        protected override TeamCityBlockWriter Create(IServiceMessageProcessor proc)
        {
            return new TeamCityBlockWriter(proc);
        }

        [Test]
        public void TestOpenBlock()
        {
            DoTest(x => x.OpenBlock("aaa"), "##teamcity[blockOpened name='aaa']");
        }

        [Test]
        public void TestOpenCloseBlock()
        {
            DoTest(x => x.OpenBlock("aaa").Dispose(), "##teamcity[blockOpened name='aaa']", "##teamcity[blockClosed name='aaa']");
        }
    }
}