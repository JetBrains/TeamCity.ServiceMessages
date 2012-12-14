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

    [Test]
    public void TestBuildProblem()
    {
      DoTest(x => x.WriteBuildProblem("id5", "aaaa"), "##teamcity[buildProblem identity='id5' description='aaaa']");
    }
  }
}