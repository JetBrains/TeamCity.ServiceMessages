using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  public class TeamCityWriterImpl : ITeamCityWriter
  {
    private readonly ITeamCityMessageWriter myMessageWriter;
    private readonly ITeamCityBlockWriter myBlockWriter;
    private readonly ITeamCityCompilationBlockWriter myCompilationWriter;
    private readonly ITeamCityTestsWriter myTestsWriter;
    private readonly IDisposable myDispose;

    public TeamCityWriterImpl([NotNull] ITeamCityMessageWriter messageWriter,
                              [NotNull] ITeamCityBlockWriter blockWriter,
                              [NotNull] ITeamCityCompilationBlockWriter compilationWriter,
                              [NotNull] ITeamCityTestsWriter testsWriter, 
                              [NotNull] IDisposable dispose)
    {
      myMessageWriter = messageWriter;
      myBlockWriter = blockWriter;
      myCompilationWriter = compilationWriter;
      myTestsWriter = testsWriter;
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

    public void Dispose()
    {
      myDispose.Dispose();
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      return myTestsWriter.OpenTest(testName);
    }

    public IDisposable OpenBlock(string blockName)
    {
      return myBlockWriter.OpenBlock(blockName);
    }
  }
}