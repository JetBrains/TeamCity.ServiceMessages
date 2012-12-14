using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Helper class to generate FlowIds
  /// </summary>
  public interface IFlowIdGenerator
  {
    /// <summary>
    /// Generates new unique FlowId
    /// </summary>
    /// <returns>next generated flow id</returns>
    [NotNull]
    string NewFlowId();
  }
}
