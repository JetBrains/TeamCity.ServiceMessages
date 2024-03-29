

#pragma warning disable 642
namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System;
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityTestSuiteBlockTest : TeamCityFlowWriterBaseTest<TeamCityTestSuiteBlock>
    {
        protected override TeamCityTestSuiteBlock Create(IFlowAwareServiceMessageProcessor proc)
        {
            return new TeamCityTestSuiteBlock(proc, DisposableDelegate.Empty);
        }

        private new void DoTest(Action<ITeamCityTestsWriter> action, params string[] data)
        {
            DoTestReplacing(action, x => x.Replace(" flowId='1'", ""), data);
        }

        [Test]
        public void TestTwoSuitesCannotBeOpenSimultaneously()
        {
            Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTestSuite("suite1");
                x.OpenTestSuite("suite2");
            }));
        }

        [Test]
        public void TestTestAndSuiteCannotBeOpenSimultaneously()
        {
            Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTest("test");
                x.OpenTestSuite("suite");
            }));
        }

        [Test]
        public void TestSuiteAndTestCannotBeOpenSimultaneously()
        {
            Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTestSuite("suite");
                x.OpenTest("test");
            }));
        }

        [Test]
        public void TestTwoTestsCannotBeOpenSimultaneously()
        {
            Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTest("test1");
                x.OpenTest("test2");
            }));
        }

        [Test]
        public void TestDisposeThrowsExceptionIfAChildTestIsOpen()
        {
            var openTestName = "open_test";

            var exception = Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTest(openTestName);
                x.Dispose();
            }));

            Assert.IsTrue(exception.Message.Contains(openTestName));
        }

        [Test]
        public void TestDisposeThrowsExceptionIfAChildTestSuiteIsOpen()
        {
            var openTestSuiteName = "open_suite";

            var exception = Assert.Throws<InvalidOperationException>(() => DoTest(x =>
            {
                x.OpenTestSuite(openTestSuiteName);
                x.Dispose();
            }));

            Assert.IsTrue(exception.Message.Contains(openTestSuiteName));
        }

        [Test]
        public void TestSuiteInSuite()
        {
            DoTest(x =>
                {
                    using (var suite = x.OpenTestSuite("suite3"))
                    {
                        using (var suite2 = suite.OpenTestSuite("suite3.444"))
                        {
                            //NOP
                        }
                    }
                },
                "##teamcity[testSuiteStarted name='suite3']",
                "##teamcity[testSuiteStarted name='suite3.444']",
                "##teamcity[testSuiteFinished name='suite3.444']",
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
                            using (var test = suite2.OpenTest("test"))
                            {
                                ;
                            }
                            using (var test = suite2.OpenTest("test2"))
                            {
                                ;
                            }
                            using (var test = suite2.OpenTest("test3"))
                            {
                                ;
                            }
                        }
                        using (var suite2 = suite.OpenTestSuite("suite2.444"))
                        {
                            using (var test = suite2.OpenTest("testZ"))
                            {
                                ;
                            }
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
        public void TestSuiteInSuiteTest()
        {
            DoTest(x =>
                {
                    using (var suite = x.OpenTestSuite("suite3"))
                    {
                        using (var suite2 = suite.OpenTestSuite("suite3.333"))
                        {
                            using (var test = suite2.OpenTest("test"))
                            {
                                //NOP
                            }
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
                            using (suite2.OpenTest("test"))
                            {
                                ;
                            }
                            using (suite2.OpenTest("test2"))
                            {
                                ;
                            }
                            using (suite2.OpenTest("test3"))
                            {
                                ;
                            }
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
        public void TestSuiteOpenClose()
        {
            DoTest(x => x.OpenTestSuite("suite3").Dispose(), "##teamcity[testSuiteStarted name='suite3']", "##teamcity[testSuiteFinished name='suite3']");
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

        [Test]
        public void TestTestError()
        {
            DoTest(x =>
                {
                    using (var test = x.OpenTest("test"))
                    {
                        test.WriteErrOutput("error3");
                    }
                },
                "##teamcity[testStarted name='test' captureStandardOutput='false']",
                "##teamcity[testStdErr name='test' out='error3' tc:tags='tc:parseServiceMessagesInside']",
                "##teamcity[testFinished name='test']");
        }

        [Test]
        public void TestTestFailed()
        {
            DoTest(x =>
                {
                    using (var test = x.OpenTest("test5"))
                    {
                        test.WriteFailed("some reason", "details");
                    }
                },
                "##teamcity[testStarted name='test5' captureStandardOutput='false']",
                "##teamcity[testFailed name='test5' message='some reason' details='details']",
                "##teamcity[testFinished name='test5']");
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
                        test.WriteStdOutput("outp4ut");
                    }
                },
                "##teamcity[testStarted name='test' captureStandardOutput='false']",
                "##teamcity[testStdOut name='test' out='outp4ut' tc:tags='tc:parseServiceMessagesInside']",
                "##teamcity[testFinished name='test']"
            );
        }
    }
}