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
using System.Threading;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityFlowWriter<TCloseBlock> : BaseDisposableWriter<IFlowServiceMessageProcessor>, ITeamCityFlowWriter<TCloseBlock>, ISubWriter
    where TCloseBlock : IDisposable
  {
    private readonly CreateWriter myCloseBlock;
    private int myIsChildFlowOpenned;

    public TeamCityFlowWriter([NotNull] IFlowServiceMessageProcessor target, [NotNull] CreateWriter closeBlock, [NotNull] IDisposable disposableHander)
      : base(target, disposableHander)
    {
      myCloseBlock = closeBlock;
    }

    public delegate TCloseBlock CreateWriter([NotNull] IDisposable disposeHandler, [NotNull] IFlowServiceMessageProcessor writer);

    public void AssertNoChildOpened()
    {
      //Opened blocks within another flowID makes no sense.
    }

    protected override void DisposeImpl()
    {      
      if (myIsChildFlowOpenned != 0)
        throw new InvalidOperationException("Some of child block writers were not disposed");
    }

    public TCloseBlock OpenFlow()
    {
      AssertNoChildOpened();

      var processor = myTarget.ForNewFlow();
      var block = myCloseBlock(
        new DisposableDelegate(() => CloseBlock(processor)),
        processor
        );

      //##teamcity[flowStarted flowId='%lt;new flow id>' parent='current flow id']
      processor.AddServiceMessage(new ServiceMessage("flowStarted") { { "parent", myTarget.FlowId } });
      Interlocked.Increment(ref myIsChildFlowOpenned);
      return block;
    }

    private void CloseBlock([NotNull] IFlowServiceMessageProcessor flowId)
    {
      Interlocked.Decrement(ref myIsChildFlowOpenned);      
      //##teamcity[flowFinished flowId='%lt;new flow id>']
      flowId.AddServiceMessage(new ServiceMessage("flowFinished"));      
    }
  }
}
