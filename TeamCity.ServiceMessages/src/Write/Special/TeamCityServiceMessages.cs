using System;
using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Basic implementation of TeamCity service message generation facade
  /// </summary>
  public class TeamCityServiceMessages : ITeamCityServiceMessages
  {
    public ITeamCityWriter CreateWriter()
    {
      return CreateWriter(Console.Out.WriteLine);
    }

    public ITeamCityWriter CreateWriter(Action<string> destination)
    {
      IServiceMessageProcessor processor = new SpecializedServiceMessagesWriter(
        new ServiceMessageFormatter(), 
        new List<IServiceMessageUpdater>
          {
            new FlowMessageUpdater(),
            new TimestampUpdater(() => DateTime.Now)
          }, 
        destination);

      return new TeamCityWriterImpl(
        processor,
        new DisposableDelegate(() => { })
        );
    }
  }
}