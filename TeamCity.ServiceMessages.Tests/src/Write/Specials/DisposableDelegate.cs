using System;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  internal class DisposableDelegate : IDisposable
  {
    private readonly Action myHost;

    public DisposableDelegate(Action host)
    {
      myHost = host;
    }

    public void Dispose()
    {
      myHost();
    }
  }
}