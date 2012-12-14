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
  public class TeamCityTestSuiteBlock : BaseDisposableWriter<IServiceMessageProcessor>, ITeamCityTestsSubWriter, ISubWriter
  {
    private int myIsChildTestOpened;
    private int myIsChildSuiteOpened;

    public TeamCityTestSuiteBlock([NotNull] IServiceMessageProcessor target, [NotNull] IDisposable disposableHandler) : base(target, disposableHandler)
    {            
    }

    public ITeamCityTestsSubWriter OpenFlow()
    {
      throw new NotImplementedException();
    }

    public void AssertNoChildOpened()
    {
      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("There is at least one child test suite opened");
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("There is at least one test suite opened");
    }

    protected override void DisposeImpl()
    {
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("Some child test writers were not disposed.");

      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("Some child test suite writers were not disposed.");
    }

    public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
    {
      AssertNoChildOpened();
      myIsChildSuiteOpened++;
      PostMessage(new ServiceMessage("testSuiteStarted") { { "name", suiteName } });

      return new TeamCityTestSuiteBlock(
        myTarget,        
        new DisposableDelegate(() =>
                                 {
                                   PostMessage(new ServiceMessage("testSuiteFinished") { { "name", suiteName } });
                                   myIsChildSuiteOpened--;
                                 }));
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      AssertNoChildOpened();

      myIsChildTestOpened++;
      var writer = new TeamCityTestWriter(myTarget, testName, new DisposableDelegate(() => { myIsChildTestOpened--; }));
      writer.OpenTest();            
      return writer;
    }
  }

}