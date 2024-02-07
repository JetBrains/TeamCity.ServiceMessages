

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Generates pair of service messages to open/close block, for example:
    /// <pre>##teamcity[blockOpened name='&lt;blockName>']</pre>
    /// and
    /// <pre>##teamcity[blockClosed name='&lt;blockName>']</pre>
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-BlocksofServiceMessages
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityBlockWriter<out CloseBlock>
        where CloseBlock : IDisposable
    {
        /// <summary>
        /// Generates open block message. To close the block, call Dispose to the given handle
        /// </summary>
        /// <param name="blockName">block name to report</param>
        /// <returns></returns>
        [NotNull]
        CloseBlock OpenBlock([NotNull] string blockName);
    }
}