namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// <para>
    /// Use <see cref="WriteProgress"/> method to add progress message
    /// <pre>
    ///     ##teamcity[progressMessage '&lt;message text>' ]
    /// </pre>
    /// </para>
    ///
    /// <para>
    /// Use <see cref="OpenProgress"/> method to add progress message
    /// <pre>##teamcity[progressStart '&lt;message>']</pre>
    /// and
    /// <pre>##teamcity[progressFinish '&lt;message>']</pre>
    /// on writer dispose.
    /// </para>
    /// <para>
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingMessagesForBuildLog
    /// </para>
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityProgressWriter
    {
        /// <summary>
        /// Writes normal message
        /// </summary>
        /// <param name="message">text</param>
        void WriteProgress([NotNull] string message);

        /// <summary>
        /// Generates start flow message and returns disposable object to close flow
        /// </summary>
        /// <returns></returns>
        IDisposable OpenProgress(string message);
    }
}