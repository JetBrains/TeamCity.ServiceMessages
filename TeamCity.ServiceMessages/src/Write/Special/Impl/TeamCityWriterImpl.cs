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
using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Annotations;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  public class TeamCityWriterImpl : ITeamCityWriter, ISubWriter
  {
    private readonly IServiceMessageProcessor myProcessor;
    
    private readonly TeamCityBlockWriter<ITeamCityWriter> myBlockWriter;
    private readonly TeamCityCompilationBlockWriter<ITeamCityWriter> myCompilationWriter;
    private readonly TeamCityTestsWriter myTestsWriter;

    
    private readonly ITeamCityMessageWriter myMessageWriter;    
    private readonly ITeamCityArtifactsWriter myArtifactsWriter;
    private readonly ITeamCityBuildStatusWriter myStatusWriter;
    private readonly IDisposable myDispose;

    private readonly IEnumerable<ISubWriter> myWriteCheck;

    public TeamCityWriterImpl([NotNull] IServiceMessageProcessor processor, 
                              [NotNull] IDisposable dispose)
    {
      myProcessor = processor;
      myMessageWriter = new TeamCityMessageWriter(processor);
      myBlockWriter = new TeamCityBlockWriter<ITeamCityWriter>(processor, d => new TeamCityWriterImpl(processor, d));
      myCompilationWriter = new TeamCityCompilationBlockWriter<ITeamCityWriter>(processor, d => new TeamCityWriterImpl(processor, d));
      myTestsWriter = new TeamCityTestsWriter(processor);
      myArtifactsWriter = new TeamCityArtifactsWriter(processor);
      myStatusWriter = new TeamCityBuildStatusWriter(processor);
      myDispose = dispose;

      myWriteCheck = new ISubWriter[] {myBlockWriter, myCompilationWriter, myTestsWriter};
    }

    public void AssertNoChildOpened()
    {
      foreach (var subWriter in myWriteCheck)
        subWriter.AssertNoChildOpened();
    }

    public ITeamCityWriter OpenCompilationBlock(string compilerName)
    {
      AssertNoChildOpened();
      return myCompilationWriter.OpenCompilationBlock(compilerName);
    }

    public void WriteMessage(string text)
    {
      AssertNoChildOpened();
      myMessageWriter.WriteMessage(text);
    }

    public void WriteWarning(string text)
    {
      AssertNoChildOpened();
      myMessageWriter.WriteWarning(text);
    }

    public void WriteError(string text, string errorDetails)
    {
      AssertNoChildOpened();
      myMessageWriter.WriteError(text, errorDetails);
    }

    public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
    {
      AssertNoChildOpened();
      return myTestsWriter.OpenTestSuite(suiteName);
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      AssertNoChildOpened();
      return myTestsWriter.OpenTest(testName);
    }

    public ITeamCityWriter OpenBlock(string blockName)
    {
      AssertNoChildOpened();
      return myBlockWriter.OpenBlock(blockName);
    }

    public void PublishArtifact(string rules)
    {
      AssertNoChildOpened();
      myArtifactsWriter.PublishArtifact(rules);
    }

    public void Dispose()
    {
      foreach (var subWriter in myWriteCheck)
        subWriter.Dispose();

      myDispose.Dispose();
    }

    public void WriteBuildNumber(string buildNumber)
    {
      AssertNoChildOpened();
      myStatusWriter.WriteBuildNumber(buildNumber);
    }

    public void WriteBuildParameter(string parameterName, string parameterValue)
    {
      AssertNoChildOpened();
      myStatusWriter.WriteBuildParameter(parameterName, parameterValue);
    }

    public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
    {
      AssertNoChildOpened();
      myStatusWriter.WriteBuildStatistics(statisticsKey, statisticsValue);
    }

    public void WriteRawMessage(IServiceMessage message)
    {
      AssertNoChildOpened();
      myProcessor.AddServiceMessage(message);
    }
  }
}