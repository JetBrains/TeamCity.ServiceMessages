/*
 * Copyright 2007-2019 JetBrains s.r.o.
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

    public class TeamCityFlowWriter<TCloseBlock> : BaseDisposableWriter<IFlowAwareServiceMessageProcessor>, ITeamCityFlowWriter<TCloseBlock>, ISubWriter
        where TCloseBlock : IDisposable
    {
        public delegate TCloseBlock CreateWriter([NotNull] IDisposable disposeHandler, [NotNull] IFlowAwareServiceMessageProcessor writer);

        private readonly CreateWriter _closeBlock;
        private int _isChildFlowOpened;

        public TeamCityFlowWriter([NotNull] IFlowAwareServiceMessageProcessor target, [NotNull] CreateWriter closeBlock, [NotNull] IDisposable disposableHandler)
            : base(target, disposableHandler)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (closeBlock == null) throw new ArgumentNullException(nameof(closeBlock));
            if (disposableHandler == null) throw new ArgumentNullException(nameof(disposableHandler));
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
            var flowStartedMessage = new ServiceMessage("flowStarted");
            if (myTarget.FlowId != null)
            {
                flowStartedMessage.Add("parent", myTarget.FlowId);
            }
            processor.AddServiceMessage(flowStartedMessage);

            Interlocked.Increment(ref _isChildFlowOpened);
            return block;
        }

        protected override void DisposeImpl()
        {
            if (_isChildFlowOpened != 0)
                throw new InvalidOperationException("Some of child block writers were not disposed");
        }

        private void CloseBlock([NotNull] IFlowAwareServiceMessageProcessor flowAwareServiceMessageProcessor)
        {
            if (flowAwareServiceMessageProcessor == null) throw new ArgumentNullException(nameof(flowAwareServiceMessageProcessor));
            Interlocked.Decrement(ref _isChildFlowOpened);
            //##teamcity[flowFinished flowId='%lt;new flow id>']
            flowAwareServiceMessageProcessor.AddServiceMessage(new ServiceMessage("flowFinished"));
        }
    }
}
