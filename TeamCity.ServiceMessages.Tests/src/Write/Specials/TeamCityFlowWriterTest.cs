using System;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  [TestFixture]
  public class TeamCityFlowWriterTest : TeamCityFlowWriterBaseTest<TeamCityFlowWriter<IDisposable>>
  {
    protected override TeamCityFlowWriter<IDisposable> Create(IFlowServiceMessageProcessor proc)
    {
      return new TeamCityFlowWriter<IDisposable>(proc, (x ,_)=>x);
    }

    [Test]
    public void TestOpenBlock()
    {
      DoTest(x => x.OpenFlow(), "##teamcity[flowStarted parent='1' flowId='2']");
    }

    [Test]
    public void TestOpenCloseBlock()
    {
      DoTest(x => x.OpenFlow().Dispose(), 
        "##teamcity[flowStarted parent='1' flowId='2']", 
        "##teamcity[flowFinished flowId='2']");
    }
  }
}