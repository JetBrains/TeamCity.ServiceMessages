/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public abstract class BaseDisposableWriter<TProc> : BaseWriter, IDisposable
    where TProc : IServiceMessageProcessor
  {
    private readonly IDisposable myDisposableHandler;
    private volatile bool myIsDisposed = false;

    protected BaseDisposableWriter([NotNull] IServiceMessageProcessor target, [NotNull] IDisposable disposableHandler) : base(target)
    {
      myDisposableHandler = disposableHandler;
    }

    public void Dispose()
    {
      if (myIsDisposed)
        throw new ObjectDisposedException(GetType() + " was allready disaposed");      
      myIsDisposed = true;
      
      DisposeImpl();

      myDisposableHandler.Dispose();
    }

    protected abstract void DisposeImpl();
  }
}