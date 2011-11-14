using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

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
}