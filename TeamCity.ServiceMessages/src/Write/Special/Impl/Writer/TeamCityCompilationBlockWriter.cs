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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityCompilationBlockWriter<TCloseBlock> : BaseWriter, ITeamCityCompilationBlockWriter<TCloseBlock>, ISubWriter
    where TCloseBlock : IDisposable
  {
    private readonly Func<IDisposable, TCloseBlock> myCloseBlock;
    private int myIsChildOpenned;

    public TeamCityCompilationBlockWriter(IServiceMessageProcessor target, Func<IDisposable, TCloseBlock> closeBlock)
      : base(target)
    {
      myCloseBlock = closeBlock;
    }

    public void AssertNoChildOpened()
    {
      if (myIsChildOpenned != 0)
        throw new InvalidOperationException("There is at least one compilation child block opened");
    }

    public void Dispose()
    {
      if (myIsChildOpenned != 0)
        throw new InvalidOperationException("Some of compilation child block writers were not disposed");
    }

    public TCloseBlock OpenCompilationBlock(string compilerName)
    {
      AssertNoChildOpened();
      PostMessage(new ServiceMessage("compilationStarted") { { "compiler", compilerName } });
      myIsChildOpenned++;
      return myCloseBlock(new DisposableDelegate(() => this.CloseBlock(compilerName)));
    }

    private void CloseBlock(string compilerName)
    {
      myIsChildOpenned--;
      PostMessage(new ServiceMessage("compilationFinished") { { "compiler", compilerName } });
    }
  }

  public class TeamCityCompilationBlockWriter : TeamCityCompilationBlockWriter<IDisposable>, ITeamCityCompilationBlockWriter
  {
    public TeamCityCompilationBlockWriter(IServiceMessageProcessor target) : base(target, x=>x)
    {
    }
  }
}