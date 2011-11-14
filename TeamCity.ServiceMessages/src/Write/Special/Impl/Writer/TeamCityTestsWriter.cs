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

using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityTestsWriter : BaseDisposableWriter, ITeamCityTestsWriter
  {
    [CanBeNull]
    private readonly string mySuiteName;

    public TeamCityTestsWriter(BaseWriter target, string suiteName = null) : base(target)
    {
      mySuiteName = suiteName;
      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteStarted") {{"name", mySuiteName}});
    }

    protected override void DisposeImpl()
    {
      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteFinished") { { "name", mySuiteName } });

    }

    public ITeamCityTestsWriter OpenTestSuite(string suiteName)
    {
      return new TeamCityTestsWriter(this, suiteName);
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      return new TeamCityTestWriter(this, testName);
    }
  }
}