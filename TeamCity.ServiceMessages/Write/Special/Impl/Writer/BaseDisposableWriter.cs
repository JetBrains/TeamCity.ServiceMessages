

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public abstract class BaseDisposableWriter<TProc> : BaseWriter<TProc>, IDisposable
        where TProc : IServiceMessageProcessor
    {
        private readonly IDisposable _disposableHandler;
        private volatile bool _isDisposed;

        protected BaseDisposableWriter([NotNull] TProc target, [NotNull] IDisposable disposableHandler)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (disposableHandler == null) throw new ArgumentNullException(nameof(disposableHandler));
            _disposableHandler = disposableHandler;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType() + " was already disposed");
            }

            _isDisposed = true;
            DisposeImpl();
            _disposableHandler.Dispose();
        }

        protected abstract void DisposeImpl();
    }
}