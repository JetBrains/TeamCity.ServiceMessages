

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Starts another flowId reporting starting. This call would emmit
    /// <pre>##teamcity[flowStarted flowId='%lt;new flow id>' parent='current flow id']</pre>
    /// and
    /// <pre>##teamcity[flowFinished flowId='%lt;new flow id>']</pre>
    /// on writer dispose
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityFlowWriter<out TCloseBlock>
        where TCloseBlock : IDisposable
    {
        /// <summary>
        /// Generates start flow message and returns disposable object to close flow
        /// </summary>
        /// <returns></returns>
        [NotNull]
        TCloseBlock OpenFlow();
    }
}