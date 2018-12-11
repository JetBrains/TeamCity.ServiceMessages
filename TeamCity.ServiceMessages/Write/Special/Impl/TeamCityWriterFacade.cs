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

    public class TeamCityWriterFacade : ITeamCityWriter
    {
        private readonly ITeamCityArtifactsWriter _artifactsWriter;
        private readonly ITeamCityBlockWriter<ITeamCityWriter> _blockWriter;
        private readonly ITeamCityCompilationBlockWriter<ITeamCityWriter> _compilationWriter;
        private readonly IDisposable _dispose;
        private readonly ITeamCityFlowWriter<ITeamCityWriter> _flowWriter;
        private readonly ITeamCityMessageWriter _messageWriter;
        private readonly IServiceMessageProcessor _processor;
        private readonly ITeamCityBuildStatusWriter _statusWriter;
        private readonly ITeamCityTestsWriter _testsWriter;
        private volatile bool _isDisposed;

        public TeamCityWriterFacade(
            [NotNull] IServiceMessageProcessor processor,
            [NotNull] ITeamCityBlockWriter<ITeamCityWriter> blockWriter,
            [NotNull] ITeamCityCompilationBlockWriter<ITeamCityWriter> compilationWriter,
            [NotNull] ITeamCityTestsWriter testsWriter,
            [NotNull] ITeamCityMessageWriter messageWriter,
            [NotNull] ITeamCityArtifactsWriter artifactsWriter,
            [NotNull] ITeamCityBuildStatusWriter statusWriter,
            [NotNull] ITeamCityFlowWriter<ITeamCityWriter> flowWriter,
            [NotNull] IDisposable disposeCallback)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (blockWriter == null) throw new ArgumentNullException(nameof(blockWriter));
            if (compilationWriter == null) throw new ArgumentNullException(nameof(compilationWriter));
            if (testsWriter == null) throw new ArgumentNullException(nameof(testsWriter));
            if (messageWriter == null) throw new ArgumentNullException(nameof(messageWriter));
            if (artifactsWriter == null) throw new ArgumentNullException(nameof(artifactsWriter));
            if (statusWriter == null) throw new ArgumentNullException(nameof(statusWriter));
            if (flowWriter == null) throw new ArgumentNullException(nameof(flowWriter));
            if (disposeCallback == null) throw new ArgumentNullException(nameof(disposeCallback));
            _processor = processor;
            _blockWriter = blockWriter;
            _compilationWriter = compilationWriter;
            _testsWriter = testsWriter;
            _messageWriter = messageWriter;
            _artifactsWriter = artifactsWriter;
            _statusWriter = statusWriter;
            _flowWriter = flowWriter;
            _dispose = disposeCallback;
        }

        public ITeamCityWriter OpenFlow()
        {
            CheckConsistency();
            return _flowWriter.OpenFlow();
        }

        public void WriteBuildNumber(string buildNumber)
        {
            if (buildNumber == null) throw new ArgumentNullException(nameof(buildNumber));
            CheckConsistency();
            _statusWriter.WriteBuildNumber(buildNumber);
        }

        public void WriteBuildProblem(string identity, string message)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (message == null) throw new ArgumentNullException(nameof(message));
            CheckConsistency();
            _statusWriter.WriteBuildProblem(identity, message);
        }

        public void WriteBuildParameter(string parameterName, string parameterValue)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (parameterValue == null) throw new ArgumentNullException(nameof(parameterValue));
            CheckConsistency();
            _statusWriter.WriteBuildParameter(parameterName, parameterValue);
        }

        public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
        {
            if (statisticsKey == null) throw new ArgumentNullException(nameof(statisticsKey));
            if (statisticsValue == null) throw new ArgumentNullException(nameof(statisticsValue));
            CheckConsistency();
            _statusWriter.WriteBuildStatistics(statisticsKey, statisticsValue);
        }

        public void WriteMessage(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            CheckConsistency();
            _messageWriter.WriteMessage(text);
        }

        public void WriteWarning(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            CheckConsistency();
            _messageWriter.WriteWarning(text);
        }

        public void WriteError(string text, string errorDetails = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            CheckConsistency();
            _messageWriter.WriteError(text, errorDetails);
        }

        public virtual void Dispose()
        {
            _isDisposed = true;
            _dispose.Dispose();
        }

        public void PublishArtifact(string rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            _artifactsWriter.PublishArtifact(rules);
        }

        public ITeamCityWriter OpenBlock(string blockName)
        {
            if (blockName == null) throw new ArgumentNullException(nameof(blockName));
            CheckConsistency();
            return _blockWriter.OpenBlock(blockName);
        }

        public void WriteRawMessage(IServiceMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            CheckConsistency();
            _processor.AddServiceMessage(message);
        }

        public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
        {
            if (suiteName == null) throw new ArgumentNullException(nameof(suiteName));
            CheckConsistency();
            return _testsWriter.OpenTestSuite(suiteName);
        }

        public ITeamCityTestWriter OpenTest(string testName)
        {
            if (testName == null) throw new ArgumentNullException(nameof(testName));
            CheckConsistency();
            return _testsWriter.OpenTest(testName);
        }

        public ITeamCityWriter OpenCompilationBlock(string compilerName)
        {
            if (compilerName == null) throw new ArgumentNullException(nameof(compilerName));
            CheckConsistency();
            return _compilationWriter.OpenCompilationBlock(compilerName);
        }

        protected virtual void CheckConsistency()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TeamCityWriterFacade has already beed disposed");
            }
        }
    }
}