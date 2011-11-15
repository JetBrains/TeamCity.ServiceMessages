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
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityTestsWriter : BaseDisposableWriter, ITeamCityTestsSubWriter
  {
    [CanBeNull]
    private readonly string mySuiteName;

    private int myIsChildTestOpened;
    private int myIsChildSuiteOpened;

    public TeamCityTestsWriter(IServiceMessageProcessor target, string suiteName = null) : base(target)
    {
      mySuiteName = suiteName;
      OpenSuite();
    }

    private void OpenSuite()
    {
      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteStarted") {{"name", mySuiteName}});
    }

    protected override void DisposeImpl()
    {
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("Some child test writers were not disposed.");

      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("Some child test suite writers were not disposed.");

      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteFinished") { { "name", mySuiteName } });
    }

    public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
    {
      AssertOpenChildBlock();

      var writer = new TeamCityTestsWriter(myTarget, suiteName);
      myIsChildSuiteOpened++;
      writer.Disposed += delegate { myIsChildSuiteOpened--; };
      return writer;
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      AssertOpenChildBlock();

      var writer = new TeamCityTestWriter(myTarget, testName);
      writer.OpenTest();
      myIsChildTestOpened++;
      writer.Disposed += delegate { myIsChildTestOpened--; };
      return writer;
    }

    private void AssertOpenChildBlock()
    {
      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("There is at least one child test suite opened");
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("There is at least one test suite opened");
    }
  }

}