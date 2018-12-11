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

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;

    public abstract class TeamCityFlowWriterBaseTest<T> : TeamCityWriterBaseTest<T>
    {
        protected abstract T Create(IFlowServiceMessageProcessor proc);

        protected sealed override T Create(IServiceMessageProcessor proc)
        {
            return Create(new FlowServiceMessageWriter(proc, new DefaultFlowIdGenerator(), Enumerable.Empty<IServiceMessageUpdater>()));
        }

        protected override ToStringProcessor CreateProcessor()
        {
            return new FlowToStringProcessor();
        }

        protected class FlowToStringProcessor : ToStringProcessor
        {
            private readonly Dictionary<string, string> myFlowToString = new Dictionary<string, string>();

            public override void AddServiceMessage(IServiceMessage serviceMessage)
            {
                if (serviceMessage.DefaultValue != null)
                {
                    base.AddServiceMessage(serviceMessage);
                    return;
                }

                if (serviceMessage.Name == "flowStarted")
                    serviceMessage = new PatchedServiceMessage(serviceMessage) {{"parent", FlowToString(serviceMessage.GetValue("parent"))}};

                var flowId = serviceMessage.GetValue("flowId");
                if (flowId != null)
                    serviceMessage = new PatchedServiceMessage(serviceMessage) {{"flowId", FlowToString(flowId)}};

                base.AddServiceMessage(serviceMessage);
            }

            private string FlowToString(string flowId)
            {
                string textFlow;
                if (!myFlowToString.TryGetValue(flowId, out textFlow))
                {
                    textFlow = (myFlowToString.Count + 1).ToString(CultureInfo.InvariantCulture);
                    myFlowToString[flowId] = textFlow;
                }
                return textFlow;
            }
        }
    }
}