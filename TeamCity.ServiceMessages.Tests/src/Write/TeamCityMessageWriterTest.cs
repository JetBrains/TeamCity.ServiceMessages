using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class TeamCityMessageWriterTest : TeamCityWriterBaseTest<ITeamCityMessageWriter>
  {
    protected override ITeamCityMessageWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityMessageWriter(proc);
    }

    [Test]
    public void TestNormalMessage()
    {
      DoTest(x => x.WriteMessage("Hello TeamCity World"), "##teamcity[message text='Hello TeamCity World' status='NORMAL']");
    }

    [Test]
    public void TestWarningMessage()
    {
      DoTest(x => x.WriteWarning("Opps"), "##teamcity[message text='Opps' status='WARNING']");
    }

    [Test]
    public void TestErrorMessage()
    {
      DoTest(x => x.WriteError("Opps"), "##teamcity[message text='Opps' status='ERROR']");
    }

    [Test]
    public void TestErrorMessage2()
    {
      DoTest(x=>x.WriteError("Opps", "Es gefaehlt mir gut"), "##teamcity[message text='Opps' status='ERROR' errorDetails='Es gefaehlt mir gut']");
    }
  }
}