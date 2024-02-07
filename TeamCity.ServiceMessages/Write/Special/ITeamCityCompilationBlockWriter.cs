

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Introduces compilation block.
    /// <pre>##teamcity[compilationStarted compiler='&lt;compiler name>']</pre>
    /// and
    /// <pre>##teamcity[compilationFinished compiler='&lt;compiler name>']</pre>
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingCompilationMessages
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityCompilationBlockWriter<out CloseBlock>
        where CloseBlock : IDisposable
    {
        /// <summary>
        /// Generates open compilation block. To close block call Dispose to the given handle
        /// </summary>
        /// <param name="compilerName"></param>
        /// <returns></returns>
        CloseBlock OpenCompilationBlock([NotNull] string compilerName);
    }

    public interface ITeamCityCompilationBlockWriter : ITeamCityCompilationBlockWriter<IDisposable>
    {
    }
}