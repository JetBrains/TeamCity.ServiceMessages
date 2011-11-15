using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
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
      DoTest(x => x.OpenBlock("aaa").Dispose(),  "##teamcity[blockOpened name='aaa']", "##teamcity[blockClosed name='aaa']");
    }
  }
}