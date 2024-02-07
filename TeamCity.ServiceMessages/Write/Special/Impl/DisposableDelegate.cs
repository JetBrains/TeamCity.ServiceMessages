

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;

    internal class DisposableDelegate : IDisposable
    {
        private readonly Action _disposeAction;

        [NotNull]
        public static readonly IDisposable Empty = new DisposableDelegate(() => { });

        public DisposableDelegate([NotNull] Action disposeAction)
        {
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction();
        }
    }
}