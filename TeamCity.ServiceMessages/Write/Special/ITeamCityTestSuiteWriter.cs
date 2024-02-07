

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
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
        [NotNull]
        ITeamCityTestsSubWriter OpenTestSuite([NotNull] string suiteName);
    }
}