using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;
using JetBrains.TeamCity.ServiceMessages.Read;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityBlockWriter : BaseWriter, ITeamCityBlockWriter
  {
    public TeamCityBlockWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    public IDisposable OpenBlock(string blockName)
    {
      PostMessage(new SimpleServiceMessage("blockOpened"){{"name", blockName}});
      return new DisposableDelegate(() => PostMessage(new SimpleServiceMessage("blockClosed") {{"name", blockName}}));
    }
  }

  public class TeamCityCompilationBlockWriter : BaseWriter, ITeamCityCompilationBlockWriter
  {
    public TeamCityCompilationBlockWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    public IDisposable OpenCompilationBlock(string compilerName)
    {
      PostMessage(new SimpleServiceMessage("compilationStarted") { { "compiler", compilerName } });
      return new DisposableDelegate(() => PostMessage(new SimpleServiceMessage("compilationFinished") { { "compiler", compilerName } }));
    }
  }

  public class TeamCityMessageWriter : BaseWriter, ITeamCityMessageWriter
  {
    public TeamCityMessageWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    private void Write([NotNull] string text, string details, [NotNull] string status)
    {
      var msg = new SimpleServiceMessage("message"){{"text", text}, {"status", status}};
      if (!string.IsNullOrEmpty(details))
        msg.Add("errorDetails", details);

      PostMessage(msg);
    }


    public void WriteMessage(string text)
    {
      Write(text, null, "NORMAL");
    }

    public void WriteWarning(string text)
    {
      Write(text, null, "WARNING");
    }

    public void WriteError(string text, string errorDetails)
    {
      Write(text, errorDetails, "ERROR");
    }
  }

  public abstract class BaseDisposableWriter : BaseWriter, IDisposable
  {
    private bool myIsDisposed = false;

    protected BaseDisposableWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    protected BaseDisposableWriter(BaseWriter writer) : base(writer)
    {
    }

    public void Dispose()
    {
      if (myIsDisposed)
        throw new ObjectDisposedException(GetType() + " was allready disaposed");
      
      myIsDisposed = true;
      DisposeImpl();
    }

    protected abstract void DisposeImpl();
  }

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