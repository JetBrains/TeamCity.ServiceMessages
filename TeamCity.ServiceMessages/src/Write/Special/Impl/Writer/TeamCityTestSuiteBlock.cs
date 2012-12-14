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
  public class TeamCityTestSuiteBlock : BaseDisposableWriter<IFlowServiceMessageProcessor>, ITeamCityTestsSubWriter, ISubWriter
  {
    private readonly TeamCityFlowWriter<ITeamCityTestsSubWriter> myFlows;
    private int myIsChildTestOpened;
    private int myIsChildSuiteOpened;

    public TeamCityTestSuiteBlock([NotNull] IFlowServiceMessageProcessor target, [NotNull] IDisposable disposableHandler) : base(target, disposableHandler)
    {            
      myFlows = new TeamCityFlowWriter<ITeamCityTestsSubWriter>(
        target, 
        (handler, writer) => new TeamCityTestSuiteBlock(writer, handler), 
        DisposableNOP.Instance);
    }

    public ITeamCityTestsSubWriter OpenFlow()
    {
      AssertNoChildOpened();
      return myFlows.OpenFlow();
    }

    public void AssertNoChildOpened()
    {
      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("There is at least one child test suite opened");
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("There is at least one test suite opened");
      
      myFlows.AssertNoChildOpened();
    }

    protected override void DisposeImpl()
    {
      if (myIsChildTestOpened != 0)
        throw new InvalidOperationException("Some child test writers were not disposed.");

      if (myIsChildSuiteOpened != 0)
        throw new InvalidOperationException("Some child test suite writers were not disposed.");

      myFlows.Dispose();
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