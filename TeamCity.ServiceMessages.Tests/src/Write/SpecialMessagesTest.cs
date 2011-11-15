using System;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
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

  [TestFixture]
  public class TeamCityBuildStatusWriterTest : TeamCityWriterBaseTest<ITeamCityBuildStatusWriter>
  {
    protected override ITeamCityBuildStatusWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityBuildStatusWriter(proc);
    }

    [Test]
    public void TestBuildNumber()
    {
      DoTest(x => x.WriteBuildNumber("100500.5"), "##teamcity[buildNumber '100500.5']");
    }

    [Test]
    public void TestParameter()
    {
      DoTest(x => x.WriteBuildParameter("num", "100500.5"), "##teamcity[setParameter name='num' value='100500.5']");
    }

    [Test]
    public void TestStatistics()
    {
      DoTest(x => x.WriteBuildStatistics("num", "100500.5"), "##teamcity[buildStatisticValue key='num' value='100500.5']");
    }
  }

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

  [TestFixture]
  public class TeamCityTestsWriterTest : TeamCityWriterBaseTest<ITeamCityTestsWriter>
  {
    protected override ITeamCityTestsWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityTestsWriter(proc);
    }

    [Test]
    public void TestSuiteOpenClose()
    {
      DoTest(x => x.OpenTestSuite("suite3").Dispose(), "##teamcity[testSuiteStarted name='suite3']", "##teamcity[testSuiteFinished name='suite3']");
    }


    [Test, ExpectedException]
    public void TestDoNotLetOpenTwoTests()
    {
      DoTest(x =>
               {
                 x.OpenTest("test1");
                 x.OpenTest("test23");
               });
    }

    [Test, ExpectedException]
    public void TestDoNotLetOpenTwoSuites()
    {
      DoTest(x =>
               {
                 x.OpenTestSuite("test1");
                 x.OpenTestSuite("test23");
               });
    }

    [Test, ExpectedException]
    public void TestDoNotLetOpenTwoTestNSuites()
    {
      DoTest(x =>
               {
                 x.OpenTest("test1");
                 x.OpenTestSuite("test1").OpenTest("z3");                 
               });
    }

    [Test]
    public void TestSuiteInSuite()
    {
      DoTest(x =>
               {
                 using (var suite = x.OpenTestSuite("suite3"))
                 {
                   using (var suite2 = suite.OpenTestSuite("suite3.444")) ;
                 }
               },
             "##teamcity[testSuiteStarted name='suite3']",
             "##teamcity[testSuiteStarted name='suite3.444']",
             "##teamcity[testSuiteFinished name='suite3.444']",
             "##teamcity[testSuiteFinished name='suite3']"
        );
    }


    [Test]
    public void TestSuiteInSuiteTest()
    {
      DoTest(x =>
               {
                 using (var suite = x.OpenTestSuite("suite3"))
                 {
                   using (var suite2 = suite.OpenTestSuite("suite3.333"))
                   {
                     using (var test = suite2.OpenTest("test")) ;
                   }
                 }
               },
             "##teamcity[testSuiteStarted name='suite3']",
             "##teamcity[testSuiteStarted name='suite3.333']",
             "##teamcity[testStarted name='test' captureStandardOutput='false']",
             "##teamcity[testFinished name='test']",
             "##teamcity[testSuiteFinished name='suite3.333']",
             "##teamcity[testSuiteFinished name='suite3']"
        );
    }

    [Test]
    public void TestSuiteInSuiteTest2()
    {
      DoTest(x =>
               {
                 using (var suite = x.OpenTestSuite("suite3"))
                 {
                   using (var suite2 = suite.OpenTestSuite("suite3.333"))
                   {
                     using (var test = suite2.OpenTest("test")) ;
                     using (var test = suite2.OpenTest("test2")) ;
                     using (var test = suite2.OpenTest("test3")) ;
                   }
                 }
               },
             "##teamcity[testSuiteStarted name='suite3']",
             "##teamcity[testSuiteStarted name='suite3.333']",
             "##teamcity[testStarted name='test' captureStandardOutput='false']",
             "##teamcity[testFinished name='test']",
             "##teamcity[testStarted name='test2' captureStandardOutput='false']",
             "##teamcity[testFinished name='test2']",
             "##teamcity[testStarted name='test3' captureStandardOutput='false']",
             "##teamcity[testFinished name='test3']",
             "##teamcity[testSuiteFinished name='suite3.333']",
             "##teamcity[testSuiteFinished name='suite3']"
        );
    }

    [Test]
    public void TestSuiteInSuite2Test2()
    {
      DoTest(x =>
               {
                 using (var suite = x.OpenTestSuite("suite3"))
                 {
                   using (var suite2 = suite.OpenTestSuite("suite3.333"))
                   {
                     using (var test = suite2.OpenTest("test")) ;
                     using (var test = suite2.OpenTest("test2")) ;
                     using (var test = suite2.OpenTest("test3")) ;
                   }
                   using (var suite2 = suite.OpenTestSuite("suite2.444"))
                   {
                     using (var test = suite2.OpenTest("testZ")) ;
                   }
                 }
               },
             "##teamcity[testSuiteStarted name='suite3']",
             "##teamcity[testSuiteStarted name='suite3.333']",
             "##teamcity[testStarted name='test' captureStandardOutput='false']",
             "##teamcity[testFinished name='test']",
             "##teamcity[testStarted name='test2' captureStandardOutput='false']",
             "##teamcity[testFinished name='test2']",
             "##teamcity[testStarted name='test3' captureStandardOutput='false']",
             "##teamcity[testFinished name='test3']",
             "##teamcity[testSuiteFinished name='suite3.333']",
             "##teamcity[testSuiteStarted name='suite2.444']",
             "##teamcity[testStarted name='testZ' captureStandardOutput='false']",
             "##teamcity[testFinished name='testZ']",
             "##teamcity[testSuiteFinished name='suite2.444']",
             "##teamcity[testSuiteFinished name='suite3']"
        );
    }

    [Test]
    public void TestTestOpenClose()
    {
      DoTest(x => x.OpenTest("suite3").Dispose(), "##teamcity[testStarted name='suite3' captureStandardOutput='false']", "##teamcity[testFinished name='suite3']");
    }

    [Test]
    public void TestTestOutput()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test"))
                 {
                   test.WriteTestStdOutput("outp4ut");
                 }
               },
               "##teamcity[testStarted name='test' captureStandardOutput='false']",
               "##teamcity[testStdOut name='test' out='outp4ut']",
               "##teamcity[testFinished name='test']"
               );
    }

    [Test]
    public void TestTestError()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test"))
                 {
                   test.WriteTestErrOutput("error3");
                 }
               },
               "##teamcity[testStarted name='test' captureStandardOutput='false']", 
               "##teamcity[testStdErr name='test' out='error3']",
               "##teamcity[testFinished name='test']");
    }

    [Test]
    public void TestTestIgnored()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test"))
                 {
                   test.WriteIgnored();
                 }
               },
               "##teamcity[testStarted name='test' captureStandardOutput='false']",
               "##teamcity[testIgnored name='test']",
               "##teamcity[testFinished name='test']");
    }

    [Test]
    public void TestTestIgnoredWithMessage()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test"))
                 {
                   test.WriteIgnored("some reason");
                 }
               },
               "##teamcity[testStarted name='test' captureStandardOutput='false']",
               "##teamcity[testIgnored name='test' message='some reason']",
               "##teamcity[testFinished name='test']");
    }

    [Test]
    public void TestTestFailed()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test5"))
                 {
                   test.WriteTestFailed("some reason", "details");
                 }
               },
               "##teamcity[testStarted name='test5' captureStandardOutput='false']", 
               "##teamcity[testFailed name='test5' message='some reason' details='details']",
               "##teamcity[testFinished name='test5']");
    }

    [Test]
    public void TestTestDuration()
    {
      DoTest(x =>
               {
                 using (var test = x.OpenTest("test"))
                 {
                   test.WriteDuration(TimeSpan.FromMilliseconds(1000));
                 }
               },
             "##teamcity[testStarted name='test' captureStandardOutput='false']",
             "##teamcity[testFinished name='test' duration='1000']");

    }

  }

}