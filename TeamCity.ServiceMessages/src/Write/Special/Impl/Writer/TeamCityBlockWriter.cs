using System;
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
}