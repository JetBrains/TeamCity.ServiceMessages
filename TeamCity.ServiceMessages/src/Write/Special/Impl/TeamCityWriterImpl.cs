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

using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  public class TeamCityWriterImpl : ITeamCityWriter
  {
    private readonly IServiceMessageProcessor myProcessor;
    private readonly ITeamCityMessageWriter myMessageWriter;
    private readonly ITeamCityBlockWriter myBlockWriter;
    private readonly ITeamCityCompilationBlockWriter myCompilationWriter;
    private readonly ITeamCityTestsWriter myTestsWriter;
    private readonly ITeamCityArtifactsWriter myArtifactsWriter;
    private readonly ITeamCityBuildStatusWriter myStatusWriter;
    private readonly IDisposable myDispose;

    public TeamCityWriterImpl([NotNull] IServiceMessageProcessor processor, 
                              [NotNull] ITeamCityMessageWriter messageWriter,
                              [NotNull] ITeamCityBlockWriter blockWriter,
                              [NotNull] ITeamCityCompilationBlockWriter compilationWriter,
                              [NotNull] ITeamCityTestsWriter testsWriter, 
                              [NotNull] ITeamCityArtifactsWriter artifactsWriter,
                              [NotNull] ITeamCityBuildStatusWriter statusWriter,
                              [NotNull] IDisposable dispose)
    {
      myProcessor = processor;
      myMessageWriter = messageWriter;
      myBlockWriter = blockWriter;
      myCompilationWriter = compilationWriter;
      myTestsWriter = testsWriter;
      myArtifactsWriter = artifactsWriter;
      myStatusWriter = statusWriter;
      myDispose = dispose;
    }

    public IDisposable OpenCompilationBlock(string compilerName)
    {
      return myCompilationWriter.OpenCompilationBlock(compilerName);
    }

    public void WriteMessage(string text)
    {
      myMessageWriter.WriteMessage(text);
    }

    public void WriteWarning(string text)
    {
      myMessageWriter.WriteWarning(text);
    }

    public void WriteError(string text, string errorDetails)
    {
      myMessageWriter.WriteError(text, errorDetails);
    }

    public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
    {
      return myTestsWriter.OpenTestSuite(suiteName);
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      return myTestsWriter.OpenTest(testName);
    }

    public IDisposable OpenBlock(string blockName)
    {
      return myBlockWriter.OpenBlock(blockName);
    }

    public void PublishArtifact(string rules)
    {
      myArtifactsWriter.PublishArtifact(rules);
    }

    public void Dispose()
    {
      myDispose.Dispose();
    }

    public void WriteBuildNumber(string buildNumber)
    {
      myStatusWriter.WriteBuildNumber(buildNumber);
    }

    public void WriteBuildParameter(string parameterName, string parameterValue)
    {
      myStatusWriter.WriteBuildParameter(parameterName, parameterValue);
    }

    public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
    {
      myStatusWriter.WriteBuildStatistics(statisticsKey, statisticsValue);
    }

    public void WriteRawMessage(IServiceMessage message)
    {
      myProcessor.AddServiceMessage(message);
    }
  }
}