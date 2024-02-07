

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    /// <summary>
    /// Base interface for service message updaters
    /// </summary>
    public interface IServiceMessageUpdater
    {
        /// <summary>
        /// Thus method is called to update service message instance.
        /// </summary>
        /// <param name="message">service message</param>
        /// <returns>updated service message</returns>
        [NotNull]
        IServiceMessage UpdateServiceMessage([NotNull] IServiceMessage message);
    }
}