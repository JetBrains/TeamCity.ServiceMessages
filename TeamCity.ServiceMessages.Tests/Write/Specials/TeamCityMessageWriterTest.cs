

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityMessageWriterTest : TeamCityWriterBaseTest<ITeamCityMessageWriter>
    {
        protected override ITeamCityMessageWriter Create(IServiceMessageProcessor proc)
        {
            return new TeamCityMessageWriter(proc);
        }

        [Test]
        public void TestErrorMessage()
        {
            DoTest(x => x.WriteError("Opps"), "##teamcity[message text='Opps' status='ERROR' tc:tags='tc:parseServiceMessagesInside']");
        }

        [Test]
        public void TestErrorMessage2()
        {
            DoTest(x => x.WriteError("Opps", "Es gefaehlt mir gut"), "##teamcity[message text='Opps' status='ERROR' tc:tags='tc:parseServiceMessagesInside' errorDetails='Es gefaehlt mir gut']");
        }

        [Test]
        public void TestNormalMessage()
        {
            DoTest(x => x.WriteMessage("Hello TeamCity World"), "##teamcity[message text='Hello TeamCity World' status='NORMAL' tc:tags='tc:parseServiceMessagesInside']");
        }

        [Test]
        public void TestWarningMessage()
        {
            DoTest(x => x.WriteWarning("Opps"), "##teamcity[message text='Opps' status='WARNING' tc:tags='tc:parseServiceMessagesInside']");
        }
    }
}