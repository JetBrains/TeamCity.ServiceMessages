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

using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
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