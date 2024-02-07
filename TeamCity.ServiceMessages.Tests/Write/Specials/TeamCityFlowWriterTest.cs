

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;
    using ServiceMessages.Write.Special.Impl.Writer;

    [TestFixture]
    public class TeamCityFlowWriterTest : TeamCityFlowWriterBaseTest<TeamCityFlowWriter<IDisposable>>
    {
        protected override TeamCityFlowWriter<IDisposable> Create(IFlowAwareServiceMessageProcessor proc)
        {
            return new TeamCityFlowWriter<IDisposable>(proc, (x, _) => x, DisposableDelegate.Empty);
        }

        [Test]
        public void TestOpenBlock()
        {
            DoTest(x => x.OpenFlow(), "##teamcity[flowStarted parent='1' flowId='2']");
        }

        [Test]
        public void TestOpenCloseBlock()
        {
            DoTest(x => x.OpenFlow().Dispose(),
                "##teamcity[flowStarted parent='1' flowId='2']",
                "##teamcity[flowFinished flowId='2']");
        }

        [Test]
        public void ShouldNotAddParentFlowAttributeToMessageAfterOpeningNewFlowIfThereIsNoParentFlow()
        {
            var parentFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            var childFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            parentFlowAwareProcessorMock.SetupGet(x => x.FlowId).Returns((string)null);
            parentFlowAwareProcessorMock.Setup(x => x.ForNewFlow()).Returns(childFlowAwareProcessorMock.Object);
            var teamCityFlowWriter = new TeamCityFlowWriter<IDisposable>(parentFlowAwareProcessorMock.Object, (x, _) => x, DisposableDelegate.Empty);

            teamCityFlowWriter.OpenFlow();

            childFlowAwareProcessorMock.Verify(
                x => x.AddServiceMessage(
                    It.Is<IServiceMessage>(m => m.Keys.All(k => k != "parent"))));
        }

        [Test]
        public void TestCannotOpenMultipleChildFlowsWithSameId()
        {
            var childFlowId = "child_flow_id";
            var parentFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            var childFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            childFlowAwareProcessorMock.SetupGet(x => x.FlowId).Returns(childFlowId);
            parentFlowAwareProcessorMock.Setup(x => x.ForNewFlow()).Returns(childFlowAwareProcessorMock.Object);
            var teamCityFlowWriter = new TeamCityFlowWriter<IDisposable>(parentFlowAwareProcessorMock.Object, (x, _) => x, DisposableDelegate.Empty);

            Assert.Throws<InvalidOperationException>(() =>
            {
                teamCityFlowWriter.OpenFlow();
                teamCityFlowWriter.OpenFlow();
            });
        }

        [Test]
        public void TestDisposeDoesNotThrowExceptionIfAllChildFLowsAreClosed()
        {
            var childFlowId = "child_flow_id";
            var parentFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            var childFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            childFlowAwareProcessorMock.SetupGet(x => x.FlowId).Returns(childFlowId);
            parentFlowAwareProcessorMock.Setup(x => x.ForNewFlow()).Returns(childFlowAwareProcessorMock.Object);
            var teamCityFlowWriter = new TeamCityFlowWriter<IDisposable>(parentFlowAwareProcessorMock.Object, (x, _) => x, DisposableDelegate.Empty);

            Assert.DoesNotThrow(() =>
            {
                var childFlow = teamCityFlowWriter.OpenFlow();
                childFlow.Dispose();

                teamCityFlowWriter.Dispose();
            });
        }

        [Test]
        public void TestDisposeThrowsExceptionIfAChildFlowIsOpen()
        {
            var childFlow1Id = "child_flow_1";
            var childFlow2Id = "child_flow_2";
            var childFlowIds = new Queue<string>();
            childFlowIds.Enqueue(childFlow1Id);
            childFlowIds.Enqueue(childFlow2Id);

            var parentFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            var childFlowAwareProcessorMock = new Mock<IFlowAwareServiceMessageProcessor>();
            childFlowAwareProcessorMock.SetupGet(x => x.FlowId).Returns(() => childFlowIds.Dequeue());
            parentFlowAwareProcessorMock.Setup(x => x.ForNewFlow()).Returns(childFlowAwareProcessorMock.Object);
            var teamCityFlowWriter = new TeamCityFlowWriter<IDisposable>(parentFlowAwareProcessorMock.Object, (x, _) => x, DisposableDelegate.Empty);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                teamCityFlowWriter.OpenFlow();
                teamCityFlowWriter.OpenFlow();

                teamCityFlowWriter.Dispose();
            });

            Assert.IsTrue(exception.Message.Contains(childFlow1Id));
            Assert.IsTrue(exception.Message.Contains(childFlow2Id));
        }
    }
}