using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
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