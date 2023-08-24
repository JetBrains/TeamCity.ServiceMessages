namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using ServiceMessages.Write;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;

    [TestFixture]
    public class FlowAwareServiceMessageWriterTest
    {
        [Test]
        public void ShouldNotAddFlowIdAttributeToServiceMessageWhenFlowIdIsNotSet()
        {
            var processorMock = new Mock<IServiceMessageProcessor>();
            var flowAwareWriter = new FlowAwareServiceMessageWriter(
                null,
                processorMock.Object,
                Mock.Of<IFlowIdGenerator>(),
                new List<IServiceMessageUpdater>());

            var serviceMessage = new ServiceMessage("foo");
            flowAwareWriter.AddServiceMessage(serviceMessage);

            processorMock.Verify(x => x.AddServiceMessage(
                It.Is<IServiceMessage>(m => m.Keys.All(k => k != "flowId"))));
        }

        [Test]
        public void ShouldAddFlowIdAttributeToServiceMessageWhenFlowIdIsSet()
        {
            string flowId = "123";
            var processorMock = new Mock<IServiceMessageProcessor>();
            var flowAwareWriter = new FlowAwareServiceMessageWriter(
                flowId,
                processorMock.Object,
                Mock.Of<IFlowIdGenerator>(),
                new List<IServiceMessageUpdater>());

            var serviceMessage = new ServiceMessage("foo");
            flowAwareWriter.AddServiceMessage(serviceMessage);

            processorMock.Verify(x => x.AddServiceMessage(
                It.Is<IServiceMessage>(m => m.GetValue("flowId") == flowId)));
        }
    }
}
