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
  public class TeamCityBlockWriter<TCloseBlock> : BaseWriter, ITeamCityBlockWriter<TCloseBlock>, ISubWriter
    where TCloseBlock : IDisposable
  {
    private readonly Func<IDisposable, TCloseBlock> myCloseBlock;
    private int myIsChildOpenned;

    public TeamCityBlockWriter(IServiceMessageProcessor target, Func<IDisposable, TCloseBlock> closeBlock) : base(target)
    {
      myCloseBlock = closeBlock;
    }

    public void AssertNoChildOpened()
    {
      if (myIsChildOpenned != 0)
        throw new InvalidOperationException("There is at least one block opened");
    }

    public void Dispose()
    {
      if (myIsChildOpenned != 0)
        throw new InvalidOperationException("Some of child block writers were not disposed");
    }

    public TCloseBlock OpenBlock(string blockName)
    {
      AssertNoChildOpened();
      PostMessage(new ServiceMessage("blockOpened") {{"name", blockName}});
      myIsChildOpenned++;
      return myCloseBlock(new DisposableDelegate(() => this.CloseBlock(blockName)));
    }

    private void CloseBlock(string blockName)
    {
      myIsChildOpenned--;
      PostMessage(new ServiceMessage("blockClosed") {{"name", blockName}});
    }
  }

  public class TeamCityBlockWriter : TeamCityBlockWriter<IDisposable>
  {
    public TeamCityBlockWriter(IServiceMessageProcessor target) : base(target, x=>x)
    {
    }
  }
}