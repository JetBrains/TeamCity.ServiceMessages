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
  public class TeamCityTestWriter : BaseDisposableWriter, ITeamCityTestWriter 
  {
    private readonly string myTestName;
    private TimeSpan? myDuration;

    public TeamCityTestWriter(IServiceMessageProcessor target, string testName) : base(target)
    {
      myTestName = testName;      
    }

    public void OpenTest()
    {
      PostMessage(new SimpleServiceMessage("testStarted") { { "name", myTestName }, { "captureStandardOutput", "false" } });
    }

    protected override void DisposeImpl()
    {
      var msg = new SimpleServiceMessage("testFinished") { { "name", myTestName }};
      if (myDuration != null)
        msg.Add("duration", ((long) myDuration.Value.TotalMilliseconds).ToString());
      PostMessage(msg);
    }

    public void WriteStdOutput(string text)
    {
      //##teamcity[testStdOut name='testname' out='text']
      PostMessage(new SimpleServiceMessage("testStdOut"){{"name", myTestName}, {"out", text}});
    }

    public void WriteErrOutput(string text)
    {
      //##teamcity[testStdErr name='testname' out='error text']
      PostMessage(new SimpleServiceMessage("testStdErr") { { "name", myTestName }, { "out", text } });
    }

    public void WriteIgnored(string message)
    {
      WriteIgnoredImpl(message);
    }

    public void WriteIgnored()
    {
      WriteIgnoredImpl(null);
    }

    private void WriteIgnoredImpl([CanBeNull] string message)
    {
      var msg = new SimpleServiceMessage("testIgnored") { { "name", myTestName }};
      if (message != null)
        msg.Add("message", message);
      PostMessage(msg);
    }

    public void WriteFailed(string errorMessage, string errorDetails)
    {
      PostMessage(new SimpleServiceMessage("testFailed"){{"name", myTestName}, {"message", errorMessage}, {"details", errorDetails}});
    }

    public void WriteDuration(TimeSpan span)
    {
      myDuration = span;
    }
  }
}