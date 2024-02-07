

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Specialized service messages writer facade. Contains all methods for generating different service messages.
    /// Do not forget to dispose this interface after you finished using it.
    /// To get the instance of the interface, call
    /// <code>new JetBrains.TeamCity.ServiceMessages.Write.Special.TeamCityServiceMessages().CreateWriter</code>
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityWriter : ITeamCityBlockWriter<ITeamCityWriter>, ITeamCityFlowWriter<ITeamCityWriter>, ITeamCityMessageWriter, ITeamCityTestsWriter, ITeamCityCompilationBlockWriter<ITeamCityWriter>, ITeamCityArtifactsWriter, ITeamCityBuildStatusWriter, IDisposable
    {
        /// <summary>
        /// Allows sending bare service message
        /// </summary>
        /// <param name="message">message to send</param>
        void WriteRawMessage([NotNull] IServiceMessage message);
    }
}