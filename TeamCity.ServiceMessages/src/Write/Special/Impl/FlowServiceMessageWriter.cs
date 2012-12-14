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

using System.Collections.Generic;
using System.Linq;
using JetBrains.TeamCity.ServiceMessages.Annotations;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Servivce message acceptor implementation with support of IServiceMessageUpdater chains
  /// </summary>
  public class FlowServiceMessageWriter : IFlowServiceMessageProcessor
  {
    private readonly IServiceMessageProcessor myProcessor;
    private readonly IFlowIdGenerator myGenerator;
    private readonly List<IServiceMessageUpdater> myUpdaters;

    /// <summary>
    /// Creates generic processor that calls messages updaters and sends output to provided deledate.
    /// </summary>
    /// <param name="processor">writer of service messages objects</param>
    /// <param name="generator">flow id generator that is called to create next flowId</param>
    /// <param name="updaters">service message updaters, i.e. timestamp updater</param>
    public FlowServiceMessageWriter([NotNull] IServiceMessageProcessor processor, 
                                    [NotNull] IFlowIdGenerator generator,
                                    [NotNull] IEnumerable<IServiceMessageUpdater> updaters)
    {
      myProcessor = processor;
      myGenerator = generator;
      myUpdaters = IncludeFlowId(updaters);      
    }

    public void AddServiceMessage(IServiceMessage serviceMessage)
    {
      myProcessor.AddServiceMessage(myUpdaters.Aggregate(serviceMessage, (current, updater) => updater.UpdateServiceMessage(current)));      
    }

    public string FlowId
    {
      get { return myUpdaters.OfType<FlowMessageUpdater>().First().FlowId; }
    }

    /// <summary>
    /// Creates new ServiceMessage updater that uses specified FlowId
    /// </summary>
    public IFlowServiceMessageProcessor ForNewFlow()
    {
      return new FlowServiceMessageWriter(
        myProcessor,
        myGenerator, 
        IncludeFlowId(myUpdaters)
        );
    }

    [NotNull]
    private List<IServiceMessageUpdater> IncludeFlowId([NotNull] IEnumerable<IServiceMessageUpdater> updaters)
    {
      return updaters.Where(x => !(x is FlowMessageUpdater))
                     .Union(new[] {new FlowMessageUpdater(myGenerator)})
                     .ToList();
    }
  }
}