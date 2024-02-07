

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
    using System;

    /// <summary>
    /// Service message updater that adds FlowId to service message according to
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity
    /// </summary>
    public class FlowMessageUpdater : IServiceMessageUpdater
    {
        [NotNull] private readonly string _flowId;

        /// <summary>
        /// Constructs updater
        /// </summary>
        /// <param name="flowId">flowId set to all messages</param>
        public FlowMessageUpdater([NotNull] string flowId)
        {
            _flowId = flowId ?? throw new ArgumentNullException(nameof(flowId));
        }

        public IServiceMessage UpdateServiceMessage(IServiceMessage message)
        {
            return message.DefaultValue != null || message.GetValue("flowId") != null ? message : new PatchedServiceMessage(message) { { "flowId", _flowId } };
        }
    }
}