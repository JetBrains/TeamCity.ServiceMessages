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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Updater;

    /// <summary>
    /// Service message acceptor implementation that is aware of service message flows and supports IServiceMessageUpdater chains
    /// </summary>
    public class FlowAwareServiceMessageWriter : IFlowAwareServiceMessageProcessor
    {
        private readonly IFlowIdGenerator _generator;
        private readonly IServiceMessageProcessor _processor;
        private readonly List<IServiceMessageUpdater> _updaters;

        /// <summary>
        /// Creates generic processor that calls messages updaters and sends output to provided delegate.
        /// </summary>
        /// <param name="serviceMessageFlowId">id of the flow assigned to messages written through this processor, can be null</param>
        /// <param name="processor">writer of service messages objects</param>
        /// <param name="generator">flow id generator that is called to create next flowId</param>
        /// <param name="updaters">service message updaters, i.e. timestamp updater</param>
        public FlowAwareServiceMessageWriter(
            string serviceMessageFlowId,
            [NotNull] IServiceMessageProcessor processor,
            [NotNull] IFlowIdGenerator generator,
            [NotNull] IEnumerable<IServiceMessageUpdater> updaters)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (generator == null) throw new ArgumentNullException(nameof(generator));
            if (updaters == null) throw new ArgumentNullException(nameof(updaters));

            FlowId = serviceMessageFlowId;
            _processor = processor;
            _generator = generator;
            _updaters = AddFlowIdUpdater(updaters.ToList());
        }

        public void AddServiceMessage(IServiceMessage serviceMessage)
        {
            if (serviceMessage == null) throw new ArgumentNullException(nameof(serviceMessage));
            _processor.AddServiceMessage(_updaters.Aggregate(serviceMessage, (current, updater) => updater.UpdateServiceMessage(current)));
        }

        public string FlowId { get; }

        /// <summary>
        /// Creates new ServiceMessage updater that uses specified FlowId
        /// </summary>
        public IFlowAwareServiceMessageProcessor ForNewFlow()
        {
            return new FlowAwareServiceMessageWriter(
                _generator.NewFlowId(),
                _processor,
                _generator,
                _updaters
            );
        }

        [NotNull]
        private List<IServiceMessageUpdater> AddFlowIdUpdater([NotNull] List<IServiceMessageUpdater> updaters)
        {
            if (FlowId == null)
            {
                return updaters;
            }

            return updaters.Where(x => !(x is FlowMessageUpdater))
                .Union(new[] {new FlowMessageUpdater(FlowId)})
                .ToList();
        }
    }
}
