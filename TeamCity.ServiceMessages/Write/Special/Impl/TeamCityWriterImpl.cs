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
    using Writer;

    public class TeamCityWriterImpl : TeamCityWriterFacade, ISubWriter
    {
        private readonly IEnumerable<ISubWriter> _writeCheck;

        public TeamCityWriterImpl(
            [NotNull] IFlowServiceMessageProcessor processor,
            [NotNull] IDisposable dispose)
            : this(processor,
                new TeamCityFlowWriter<ITeamCityWriter>(processor, (handler, writer) => new TeamCityWriterImpl(writer, handler), DisposableDelegate.Empty),
                new TeamCityBlockWriter<ITeamCityWriter>(processor, d => new TeamCityWriterImpl(processor, d)),
                new TeamCityCompilationBlockWriter<ITeamCityWriter>(processor, d => new TeamCityWriterImpl(processor, d)),
                new TeamCityTestSuiteBlock(processor, DisposableDelegate.Empty),
                new TeamCityMessageWriter(processor),
                new TeamCityArtifactsWriter(processor),
                new TeamCityBuildStatusWriter(processor),
                dispose)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (dispose == null) throw new ArgumentNullException(nameof(dispose));
        }

        private TeamCityWriterImpl(
            [NotNull] IServiceMessageProcessor processor,
            [NotNull] TeamCityFlowWriter<ITeamCityWriter> flowWriter,
            [NotNull] TeamCityBlockWriter<ITeamCityWriter> blockWriter,
            [NotNull] TeamCityCompilationBlockWriter<ITeamCityWriter> compilationWriter,
            [NotNull] TeamCityTestSuiteBlock testsWriter,
            [NotNull] ITeamCityMessageWriter messageWriter,
            [NotNull] ITeamCityArtifactsWriter artifactsWriter,
            [NotNull] ITeamCityBuildStatusWriter statusWriter,
            [NotNull] IDisposable dispose)
            : base(processor, blockWriter, compilationWriter, testsWriter, messageWriter, artifactsWriter, statusWriter, flowWriter, dispose)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (flowWriter == null) throw new ArgumentNullException(nameof(flowWriter));
            if (blockWriter == null) throw new ArgumentNullException(nameof(blockWriter));
            if (compilationWriter == null) throw new ArgumentNullException(nameof(compilationWriter));
            if (testsWriter == null) throw new ArgumentNullException(nameof(testsWriter));
            if (messageWriter == null) throw new ArgumentNullException(nameof(messageWriter));
            if (artifactsWriter == null) throw new ArgumentNullException(nameof(artifactsWriter));
            if (statusWriter == null) throw new ArgumentNullException(nameof(statusWriter));
            if (dispose == null) throw new ArgumentNullException(nameof(dispose));
            _writeCheck = new ISubWriter[] {blockWriter, compilationWriter, testsWriter, flowWriter};
        }

        public void AssertNoChildOpened()
        {
            foreach (var subWriter in _writeCheck)
            {
                subWriter.AssertNoChildOpened();
            }
        }

        public override void Dispose()
        {
            foreach (var subWriter in _writeCheck)
            {
                subWriter.Dispose();
            }

            base.Dispose();
        }

        protected override void CheckConsistency()
        {
            base.CheckConsistency();
            AssertNoChildOpened();
        }
    }
}