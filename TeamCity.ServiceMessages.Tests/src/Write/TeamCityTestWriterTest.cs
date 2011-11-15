using System;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class TeamCityTestWriterTest : TeamCityWriterBaseTest<ITeamCityTestWriter>
  {
    protected override ITeamCityTestWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityTestWriter(proc, "BadaBumBigBadaBum");
    }

    [Test]
    public void TestDispose()
    {
      DoTest(x => x.Dispose(), "##teamcity[testFinished name='BadaBumBigBadaBum']");      
    }

    [Test, ExpectedException(typeof(ObjectDisposedException))]
    public void TestDisposeDispose()
    {      
      DoTest(x =>
               {
                 x.Dispose(); 
                 x.Dispose();
               }, "Exception");      
    }

    [Test]
    public void TestStdOut()
    {
      DoTest(x => x.WriteTestStdOutput("outp4uz"), "##teamcity[testStdOut name='BadaBumBigBadaBum' out='outp4uz']");
    }

    [Test]
    public void TestStdErr()
    {
      DoTest(x => x.WriteTestErrOutput("outp4ut"), "##teamcity[testStdErr name='BadaBumBigBadaBum' out='outp4ut']");
    }

    [Test]
    public void TestFailed()
    {
      DoTest(x=>x.WriteTestFailed("aaa", "ooo"), "##teamcity[testFailed name='BadaBumBigBadaBum' message='aaa' details='ooo']");
    }

    [Test]
    public void TestIgnored()
    {
      DoTest(x => x.WriteIgnored(), "##teamcity[testIgnored name='BadaBumBigBadaBum']");
    }

    [Test]
    public void TestIgnoredMessage()
    {
      DoTest(x => x.WriteIgnored("qqq"), "##teamcity[testIgnored name='BadaBumBigBadaBum' message='qqq']");
    }
  }
}