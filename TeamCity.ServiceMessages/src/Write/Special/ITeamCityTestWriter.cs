using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
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