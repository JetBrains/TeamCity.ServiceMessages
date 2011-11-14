using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
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
}