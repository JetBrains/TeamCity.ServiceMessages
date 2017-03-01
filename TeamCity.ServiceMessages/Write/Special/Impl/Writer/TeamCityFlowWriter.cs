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
    using System.Threading;

    public class TeamCityFlowWriter<TCloseBlock> : BaseDisposableWriter<IFlowServiceMessageProcessor>, ITeamCityFlowWriter<TCloseBlock>, ISubWriter
        where TCloseBlock : IDisposable
    {
        public delegate TCloseBlock CreateWriter([NotNull] IDisposable disposeHandler, [NotNull] IFlowServiceMessageProcessor writer);

        private readonly CreateWriter _closeBlock;
        private int _isChildFlowOpenned;

        public TeamCityFlowWriter([NotNull] IFlowServiceMessageProcessor target, [NotNull] CreateWriter closeBlock, [NotNull] IDisposable disposableHander)
            : base(target, disposableHander)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (closeBlock == null) throw new ArgumentNullException(nameof(closeBlock));
            if (disposableHander == null) throw new ArgumentNullException(nameof(disposableHander));
            _closeBlock = closeBlock;
        }

        public void AssertNoChildOpened()
        {
            //Opened blocks within another flowID makes no sense.
        }

        public TCloseBlock OpenFlow()
        {
            AssertNoChildOpened();

            var processor = myTarget.ForNewFlow();
            var block = _closeBlock(
                new DisposableDelegate(() => CloseBlock(processor)),
                processor
            );

            //##teamcity[flowStarted flowId='%lt;new flow id>' parent='current flow id']
            processor.AddServiceMessage(new ServiceMessage("flowStarted") {{"parent", myTarget.FlowId}});
            Interlocked.Increment(ref _isChildFlowOpenned);
            return block;
        }

        protected override void DisposeImpl()
        {
            if (_isChildFlowOpenned != 0)
                throw new InvalidOperationException("Some of child block writers were not disposed");
        }

        private void CloseBlock([NotNull] IFlowServiceMessageProcessor flowId)
        {
            if (flowId == null) throw new ArgumentNullException(nameof(flowId));
            Interlocked.Decrement(ref _isChildFlowOpenned);
            //##teamcity[flowFinished flowId='%lt;new flow id>']
            flowId.AddServiceMessage(new ServiceMessage("flowFinished"));
        }
    }
}