

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    /// <summary>
    /// FlowId aware implementation of ServiceMessagesProcessor
    /// </summary>
    public interface IFlowAwareServiceMessageProcessor : IServiceMessageProcessor
    {
        /// <summary>
        /// Current flow Id
        /// </summary>
        string FlowId { [NotNull] get; }

        /// <summary>
        /// Creates new ServiceMessage updater that uses specified FlowId
        /// </summary>
        /// <returns></returns>
        [NotNull]
        IFlowAwareServiceMessageProcessor ForNewFlow();
    }
}