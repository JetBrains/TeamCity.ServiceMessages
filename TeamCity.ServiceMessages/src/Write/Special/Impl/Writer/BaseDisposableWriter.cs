using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
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
}