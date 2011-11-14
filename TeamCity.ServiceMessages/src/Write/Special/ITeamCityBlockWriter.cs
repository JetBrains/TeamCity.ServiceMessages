using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Generates pair of service messages like 
  /// <pre>##teamcity[blockOpened name='&lt;blockName>']</pre>
  /// and 
  /// <pre>##teamcity[blockClosed name='&lt;blockName>']</pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-BlocksofServiceMessages
  /// </summary>
  public interface ITeamCityBlockWriter
  {
    /// <summary>
    /// Generates open block message. To close the block, call Dispose to the given handle
    /// </summary>
    /// <param name="blockName">block name to report</param>
    /// <returns></returns>
    IDisposable OpenBlock([NotNull] string blockName);
  }

  /// <summary>
  /// Introduces compilation block.
  /// <pre>##teamcity[compilationStarted compiler='&lt;compiler name>']</pre>
  /// and
  /// <pre>##teamcity[compilationFinished compiler='&lt;compiler name>']</pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingCompilationMessages
  /// </summary>
  public interface ITeamCityCompilationBlockWriter
  {
    /// <summary>
    /// Generates open compilation block. To close block call Dispose to the given handle
    /// </summary>
    /// <param name="compilerName"></param>
    /// <returns></returns>
    IDisposable OpenCompilationBlock([NotNull] string compilerName);
  }


  /// <summary>
  /// Add a build log entry message
  /// <pre>
  /// ##teamcity[message text='&lt;message text>' errorDetails='&lt;error details>' status='&lt;status value>']
  /// </pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingMessagesForBuildLog
  /// </summary>
  public interface ITeamCityMessageWriter
  {
    /// <summary>
    /// Writes normal message
    /// </summary>
    /// <param name="text">text</param>
    void WriteMessage([NotNull] string text);

    /// <summary>
    /// Writes warning message
    /// </summary>
    /// <param name="text">text</param>
    void WriteWarning([NotNull] string text);

    /// <summary>
    /// Writes error message with details
    /// </summary>
    /// <param name="text">text</param>
    /// <param name="errorDetails">error details</param>
    void WriteError([NotNull] string text, [CanBeNull] string errorDetails);    
  }


  /// <summary>
  /// TeamCity tests loggin in done in test suites. 
  /// To be able to logs tests you first need to open suite
  /// </summary>
  public interface ITeamCityTestSuiteWriter
  {
    /// <summary>
    /// Opens tests suite with give name
    /// <pre>##teamcity[testSuiteStarted name='suite.name']</pre>
    /// To close suite, call Dispose method of a returned logger.
    /// </summary>
    /// <param name="suiteName">suite name</param>
    /// <returns>test logger.</returns>
    ITeamCityTestsWriter OpenTestSuite([NotNull] string suiteName);
  }

  /// <summary>
  /// This interface provides writers for test messages. 
  /// All, but test ignore messages are required to be reported from 
  /// <pre>##teamcity[testStarted name='testname']</pre> and
  /// <pre>##teamcity[testFinished name='testname' duration='&lt;test_duration_in_milliseconds>']</pre>
  /// messages.
  /// 
  /// All tests reportings are done form this method.  
  /// </summary>
  public interface ITeamCityTestsWriter : ITeamCityTestSuiteWriter, IDisposable
  {
    /// <summary>
    /// To start reporting a test, call OpenTest method. To stop reporing test call Dispose on the given object
    /// </summary>
    /// <param name="testName">test name to be reported</param>
    /// <returns>test output/status reporting handle</returns>
    [NotNull]
    ITeamCityTestWriter OpenTest([NotNull] string testName);
  }

  /// <summary>
  /// Writer interface for generating test information service messages
  /// </summary>
  public interface ITeamCityTestWriter : IDisposable
  {
    /// <summary>
    /// Attaches test output to the test
    /// </summary>
    /// <param name="text">test output</param>
    void WriteTestStdOutput([NotNull] string text);

    /// <summary>
    /// Attaches test error output to the test
    /// </summary>
    /// <param name="text">error output</param>
    void WriteTestErrOutput([NotNull] string text);

    /// <summary>
    /// Marks test as ignored
    /// </summary>
    /// <param name="ignoreReason">test ignore reason</param>
    void WriteIgnored([NotNull] string ignoreReason);

    /// <summary>
    /// Marks test as failed.
    /// </summary>
    /// <param name="errorMessage">short error message</param>
    /// <param name="errorDetails">detailed error information, i.e. stacktrace</param>
    /// <remarks>
    /// This method can be called only once.
    /// </remarks>
    void WriteTestFailed([NotNull] string errorMessage, [NotNull] string errorDetails);

    /// <summary>
    /// Specifies test duration
    /// </summary>
    /// <remarks>
    /// TeamCity may compute test duration inself, to provide precise data, you may set the duration explicitly
    /// </remarks>
    /// <param name="duration">time of test</param>
    void WriteDuration(TimeSpan duration);
  }
}