using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// FlowId aware implementation of ServiceMessagesProcessor
  /// </summary>
  public interface IFlowServiceMessageProcessor : IServiceMessageProcessor
  {
    /// <summary>
    /// Current flow Id
    /// </summary>
    string FlowId { get; }

    /// <summary>
    /// Creates new ServiceMessage updater that uses specified FlowId
    /// </summary>
    /// <returns></returns>
    [NotNull]
    IFlowServiceMessageProcessor ForNewFlow();
  }
}