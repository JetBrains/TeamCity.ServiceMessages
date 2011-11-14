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

  public interface ITeamCityTestsWriter : ITeamCityTestSuiteWriter, IDisposable
  {
    ITeamCityTestWriter OpenTest([NotNull] string testName);
    void WriteTestIgnored([NotNull] string testName, [NotNull] string ignoreReason);
  }

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
    void SetIgnored([NotNull] string ignoreReason);

    /// <summary>
    /// Specifies test duration
    /// </summary>
    /// <remarks>
    /// TeamCity may compute test duration inself, to provide precise data, you may set the duration explicitly
    /// </remarks>
    /// <param name="duration">time of test</param>
    void SetDuration(TimeSpan duration);
  }
}