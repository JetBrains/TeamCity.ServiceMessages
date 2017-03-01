/*
 * Copyright 2007-2017 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public abstract class BaseDisposableWriter<TProc> : BaseWriter<TProc>, IDisposable
        where TProc : IServiceMessageProcessor
    {
        private readonly TProc _target;
        private readonly IDisposable _disposableHandler;

        private volatile bool _isDisposed;

        protected BaseDisposableWriter([NotNull] TProc target, [NotNull] IDisposable disposableHandler)
            : base(target)
        {
            if (disposableHandler == null) throw new ArgumentNullException(nameof(disposableHandler));
            _target = target;
            _disposableHandler = disposableHandler;
        }

        public void Dispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType() + " was allready disaposed");
            _isDisposed = true;

            DisposeImpl();

            _disposableHandler.Dispose();
        }

        protected abstract void DisposeImpl();
    }
}