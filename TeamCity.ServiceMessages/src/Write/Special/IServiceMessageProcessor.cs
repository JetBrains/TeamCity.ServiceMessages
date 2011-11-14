using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Abstract service message acceptor
  /// </summary>
  public interface IServiceMessageProcessor
  {
    /// <summary>
    /// Accepts new service message. Default implementation may simply prtint service message to console
    /// </summary>
    /// <param name="serviceMessage">service message to process</param>
    void AddServiceMessage([NotNull] IServiceMessage serviceMessage);
  }
}