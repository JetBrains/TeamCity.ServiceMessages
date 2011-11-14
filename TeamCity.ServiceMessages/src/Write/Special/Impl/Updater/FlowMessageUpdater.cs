namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
  /// <summary>
  /// Service message updater that adds FlowId to service message according to
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity
  /// </summary>
  public class FlowMessageUpdater : IServiceMessageUpdater
  {
    private readonly string myFlowId;

    /// <summary>
    /// Custructs updater
    /// </summary>
    /// <param name="flowId">flowId set to all messages</param>
    public FlowMessageUpdater(string flowId)
    {
      myFlowId = flowId;
    }

    public IServiceMessage UpdateServiceMessage(IServiceMessage message)
    {
      return new PatchedServiceMessage(message){{"FlowId", myFlowId}};
    }
  }  
}