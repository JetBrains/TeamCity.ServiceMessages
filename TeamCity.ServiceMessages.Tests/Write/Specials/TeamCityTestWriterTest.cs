/*
 * Copyright 2007-2017 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System;
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityTestWriterTest : TeamCityWriterBaseTest<ITeamCityTestWriter>
    {
        protected override ITeamCityTestWriter Create(IServiceMessageProcessor proc)
        {
            return new TeamCityTestWriter(proc, "BadaBumBigBadaBum", DisposableDelegate.Empty);
        }

        [Test]
        public void TestDispose()
        {
            DoTest(x => x.Dispose(), "##teamcity[testFinished name='BadaBumBigBadaBum']");
        }

        [Test]
        public void TestDisposeDispose()
        {
            Assert.Throws<ObjectDisposedException>(() => DoTest(x =>
            {
                x.Dispose();
                x.Dispose();
            }, "Exception"));
        }

        [Test]
        public void TestFailed()
        {
            DoTest(x => x.WriteFailed("aaa", "ooo"), "##teamcity[testFailed name='BadaBumBigBadaBum' message='aaa' details='ooo']");
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

        [Test]
        public void TestStdErr()
        {
            DoTest(x => x.WriteErrOutput("outp4ut"), "##teamcity[testStdErr name='BadaBumBigBadaBum' out='outp4ut' tc:tags='tc:parseServiceMessagesInside']");
        }

        [Test]
        public void TestStdOut()
        {
            DoTest(x => x.WriteStdOutput("outp4uz"), "##teamcity[testStdOut name='BadaBumBigBadaBum' out='outp4uz' tc:tags='tc:parseServiceMessagesInside']");
        }

        [Test]
        public void TestWriteTextValue()
        {
            DoTest(x => x.WriteValue("strVal", "myVal"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' value='strVal' name='myVal']");
        }

        [Test]
        [TestCase(1.0d, "1")]
        [TestCase(0.0d, "0")]
        [TestCase(-1.0d, "-1")]
        [TestCase(1.33d, "1.33")]
        [TestCase(-1.33d, "-1.33")]
        [TestCase(0.33d, "0.33")]
        public void TestWriteNumber(double value, string expectedValueInMessage)
        {
            DoTest(x => x.WriteValue(value, "myVal"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='number' value='" + expectedValueInMessage + "' name='myVal']");
        }

        [Test]
        public void TestWriteLink()
        {
            DoTest(x => x.WriteLink("http://abc.com", "abc"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='link' value='http://abc.com' name='abc']");
        }

        [Test]
        public void TestWriteFile()
        {
            DoTest(x => x.WriteFile("abc.txt", "abc"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='artifact' value='abc.txt' name='abc']");
        }

        [Test]
        public void TestWriteFileWithoutDescription()
        {
            DoTest(x => x.WriteFile("abc.txt"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='artifact' value='abc.txt']");
        }

        [Test]
        public void TestWriteImage()
        {
            DoTest(x => x.WriteImage("abc.jpg", "abc"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='image' value='abc.jpg' name='abc']");
        }

        [Test]
        public void TestWriteImageWithoutDescription()
        {
            DoTest(x => x.WriteImage("abc.jpg"), "##teamcity[testMetadata testName='BadaBumBigBadaBum' type='image' value='abc.jpg']");
        }
    }
}