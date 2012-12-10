using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  public class TeamCityWriterFacade : ITeamCityWriter
  {
    private readonly IServiceMessageProcessor myProcessor;

    private readonly ITeamCityBlockWriter<ITeamCityWriter> myBlockWriter;
    private readonly ITeamCityCompilationBlockWriter<ITeamCityWriter> myCompilationWriter;
    private readonly ITeamCityTestsWriter myTestsWriter;
    private readonly ITeamCityMessageWriter myMessageWriter;
    private readonly ITeamCityArtifactsWriter myArtifactsWriter;
    private readonly ITeamCityBuildStatusWriter myStatusWriter;
    private readonly IDisposable myDispose;

    public TeamCityWriterFacade([NotNull] IServiceMessageProcessor processor,
                                [NotNull] ITeamCityBlockWriter<ITeamCityWriter> blockWriter,
                                [NotNull] ITeamCityCompilationBlockWriter<ITeamCityWriter> compilationWriter,
                                [NotNull] ITeamCityTestsWriter testsWriter,
                                [NotNull] ITeamCityMessageWriter messageWriter,
                                [NotNull] ITeamCityArtifactsWriter artifactsWriter,
                                [NotNull] ITeamCityBuildStatusWriter statusWriter,
                                [NotNull] IDisposable dispose)
    {
      myProcessor = processor;
      myBlockWriter = blockWriter;
      myCompilationWriter = compilationWriter;
      myTestsWriter = testsWriter;
      myMessageWriter = messageWriter;
      myArtifactsWriter = artifactsWriter;
      myStatusWriter = statusWriter;
      myDispose = dispose;
    }

    protected virtual void CheckConsistency() {}

    public void WriteBuildNumber(string buildNumber)
    {
      CheckConsistency();
      myStatusWriter.WriteBuildNumber(buildNumber);
    }

    public void WriteBuildProblem(string identity, string message)
    {
      CheckConsistency();
      myStatusWriter.WriteBuildProblem(identity, message);
    }

    public void WriteBuildParameter(string parameterName, string parameterValue)
    {
      CheckConsistency();
      myStatusWriter.WriteBuildParameter(parameterName, parameterValue);
    }

    public void WriteBuildStatistics(string statisticsKey, string statisticsValue)
    {
      CheckConsistency();
      myStatusWriter.WriteBuildStatistics(statisticsKey, statisticsValue);
    }

    public void WriteMessage(string text)
    {
      CheckConsistency();
      myMessageWriter.WriteMessage(text);
    }

    public void WriteWarning(string text)
    {
      CheckConsistency();
      myMessageWriter.WriteWarning(text);
    }

    public void WriteError(string text, string errorDetails = null)
    {
      CheckConsistency();
      myMessageWriter.WriteError(text, errorDetails);
    }

    public virtual void Dispose()
    {
      myDispose.Dispose();
    }

    public void PublishArtifact(string rules)
    {
      CheckConsistency();
      myArtifactsWriter.PublishArtifact(rules);
    }

    public ITeamCityWriter OpenBlock(string blockName)
    {
      CheckConsistency();
      return myBlockWriter.OpenBlock(blockName);
    }

    public void WriteRawMessage(IServiceMessage message)
    {
      CheckConsistency();
      myProcessor.AddServiceMessage(message);
    }

    public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
    {
      CheckConsistency();
      return myTestsWriter.OpenTestSuite(suiteName);
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      CheckConsistency();
      return myTestsWriter.OpenTest(testName);
    }

    public ITeamCityWriter OpenCompilationBlock(string compilerName)
    {
      CheckConsistency();
      return myCompilationWriter.OpenCompilationBlock(compilerName);
    }
  }
}