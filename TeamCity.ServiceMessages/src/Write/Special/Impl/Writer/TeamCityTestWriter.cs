using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityTestWriter : BaseDisposableWriter, ITeamCityTestWriter 
  {
    private readonly string myTestName;
    private TimeSpan? myDuration;

    public TeamCityTestWriter(BaseWriter target, string testName) : base(target)
    {
      myTestName = testName;
      PostMessage(new SimpleServiceMessage("testStarted") { { "name", testName }, {"captureStandardOutput", "false"} });
    }

    protected override void DisposeImpl()
    {
      var msg = new SimpleServiceMessage("testStarted") { { "name", myTestName }};
      if (myDuration != null)
        msg.Add("duration", ((long) myDuration.Value.TotalMilliseconds).ToString());
      PostMessage(msg);
    }

    public void WriteTestStdOutput(string text)
    {
      //##teamcity[testStdOut name='testname' out='text']
      PostMessage(new SimpleServiceMessage("testStdOut"){{"name", myTestName}, {"out", text}});
    }

    public void WriteTestErrOutput(string text)
    {
      //##teamcity[testStdErr name='testname' out='error text']
      PostMessage(new SimpleServiceMessage("testStdErr") { { "name", myTestName }, { "out", text } });
    }

    public void WriteIgnored(string message)
    {
      PostMessage(new SimpleServiceMessage("testIgnored") { { "name", myTestName }, { "message", message } });
    }

    public void WriteTestFailed(string errorMessage, string errorDetails)
    {
      PostMessage(new SimpleServiceMessage("testFailed"){{"name", myTestName}, {"message", errorMessage}, {"details", errorDetails}});
    }

    public void WriteDuration(TimeSpan span)
    {
      myDuration = span;
    }
  }
}