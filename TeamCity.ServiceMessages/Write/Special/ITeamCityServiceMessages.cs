

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Factory interface for specialized service messages generation
    /// Create instance of <see cref="TeamCityServiceMessages" /> to get the implementation of the interface.
    /// </summary>
    public interface ITeamCityServiceMessages
    {
        /// <summary>
        /// Creates a writer that outputs service messages to Console.Out
        /// </summary>
        /// <returns></returns>
        [NotNull]
        ITeamCityWriter CreateWriter();

        /// <summary>
        /// Creates a writer that uses the provided delegate to output service messages
        /// </summary>
        /// <param name="destination">generated service messages processor</param>
        /// <param name="addFlowIdsOnTopLevelMessages">specifies whether messages written without explicitly opening a flow should be marked with a common flow id</param>
        /// <returns></returns>
        [NotNull]
        ITeamCityWriter CreateWriter(Action<string> destination, bool addFlowIdsOnTopLevelMessages = true);

        /// <summary>
        /// Adds user-specific service message updater to the list of service message updaters.
        /// </summary>
        /// <param name="updater">updater instance</param>
        void AddServiceMessageUpdater([NotNull] IServiceMessageUpdater updater);
    }
}