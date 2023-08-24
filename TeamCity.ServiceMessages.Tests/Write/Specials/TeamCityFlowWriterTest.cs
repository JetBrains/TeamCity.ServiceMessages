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
    using System;
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
            var teamCityFlowWriter = new TeamCityFlowWriter<IDisposable>(
                parentFlowAwareProcessorMock.Object,
                (handler, processor) => Mock.Of<IDisposable>(),
                Mock.Of<IDisposable>());

            teamCityFlowWriter.OpenFlow();

            childFlowAwareProcessorMock.Verify(
                x => x.AddServiceMessage(
                    It.Is<IServiceMessage>(m => m.Keys.All(k => k != "parent"))));
        }
    }
}
