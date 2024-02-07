

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// This interface provides writers for test messages.
    /// All, but test ignore messages are required to be reported from
    /// <pre>##teamcity[testStarted name='testname']</pre> and
    /// <pre>##teamcity[testFinished name='testname' duration='&lt;test_duration_in_milliseconds>']</pre>
    /// messages.
    /// All tests reportings are done form this method.
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityTestsWriter : ITeamCityTestSuiteWriter
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
    /// Sub inteface for created tests writer for some parent test suite.
    /// </summary>
    public interface ITeamCityTestsSubWriter : ITeamCityTestsWriter, ITeamCityFlowWriter<ITeamCityTestsSubWriter>, IDisposable
    {
    }
}