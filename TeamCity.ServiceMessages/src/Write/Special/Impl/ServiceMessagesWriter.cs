using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Simple implementation of ServiceMessage 
  /// </summary>
  public class ServiceMessagesWriter : IServiceMessageProcessor
  {
    private readonly IServiceMessageFormatter myFormatter;    
    private readonly Action<string> myPrinter;

    public ServiceMessagesWriter([NotNull] IServiceMessageFormatter formatter, [NotNull] Action<string> printer)
    {
      myFormatter = formatter;
      myPrinter = printer;
    }

    public void AddServiceMessage(IServiceMessage serviceMessage)
    {
      myPrinter(myFormatter.FormatMessage(serviceMessage));
    }
  }
}