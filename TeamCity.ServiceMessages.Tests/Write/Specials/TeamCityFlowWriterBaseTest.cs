

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;

    public abstract class TeamCityFlowWriterBaseTest<T> : TeamCityWriterBaseTest<T>
    {
        protected abstract T Create(IFlowAwareServiceMessageProcessor proc);

        protected sealed override T Create(IServiceMessageProcessor proc)
        {
            var flowIdGenerator = new DefaultFlowIdGenerator();
            return Create(new FlowAwareServiceMessageWriter(flowIdGenerator.NewFlowId(), proc, flowIdGenerator, Enumerable.Empty<IServiceMessageUpdater>()));
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