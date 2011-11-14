using System;
using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Servivce message acceptor implementation with support of IServiceMessageUpdater chains
  /// </summary>
  public class SpecializedServiceMessagesWriter : IServiceMessageProcessor
  {
    private readonly IServiceMessageFormatter myFormaater;
    private readonly List<IServiceMessageUpdater> myUpdaters;
    private readonly Action<string> myPrinter;

    /// <summary>
    /// Creates generic processor that calls messages updaters and sends output to provided deledate.
    /// </summary>
    /// <param name="formaater"></param>
    /// <param name="updaters"></param>
    /// <param name="printer"></param>
    public SpecializedServiceMessagesWriter([NotNull] IServiceMessageFormatter formaater, [NotNull] List<IServiceMessageUpdater> updaters, [NotNull] Action<string> printer)
    {
      myFormaater = formaater;
      myUpdaters = updaters;
      myPrinter = printer;
    }

    public void AddServiceMessage(IServiceMessage serviceMessage)
    {
      foreach (var updater in myUpdaters)
        serviceMessage = updater.UpdateServiceMessage(serviceMessage);

      myPrinter(myFormaater.FormatMessage(serviceMessage));
    }
  }
}