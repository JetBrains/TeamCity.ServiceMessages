using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
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