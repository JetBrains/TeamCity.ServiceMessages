/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  [TestFixture]
  public class TeamCityTestWriterTest : TeamCityWriterBaseTest<ITeamCityTestWriter>
  {
    protected override ITeamCityTestWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityTestWriter(proc, "BadaBumBigBadaBum", new DisposableDelegate(() => { }));
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
      DoTest(x => x.WriteStdOutput("outp4uz"), "##teamcity[testStdOut name='BadaBumBigBadaBum' out='outp4uz' tc:tags='tc:parseServiceMessagesInside']");
    }

    [Test]
    public void TestStdErr()
    {
      DoTest(x => x.WriteErrOutput("outp4ut"), "##teamcity[testStdErr name='BadaBumBigBadaBum' out='outp4ut' tc:tags='tc:parseServiceMessagesInside']");
    }

    [Test]
    public void TestFailed()
    {
      DoTest(x=>x.WriteFailed("aaa", "ooo"), "##teamcity[testFailed name='BadaBumBigBadaBum' message='aaa' details='ooo']");
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