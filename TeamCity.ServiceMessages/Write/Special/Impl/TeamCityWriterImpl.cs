

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;
    using System.Collections.Generic;
    using Writer;

    public class TeamCityWriterImpl : TeamCityWriterFacade, ISubWriter
    {
        private readonly IEnumerable<ISubWriter> _writeCheck;

        public TeamCityWriterImpl(
            [NotNull] IFlowAwareServiceMessageProcessor processor,
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