

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Helper class to generate FlowIds
    /// </summary>
    public class DefaultFlowIdGenerator : IFlowIdGenerator
    {
        private static long ourIds;

        /// <summary>
        /// Generates new unique FlowId
        /// </summary>
        public string NewFlowId()
        {
            return Interlocked.Increment(ref ourIds).ToString(CultureInfo.InvariantCulture);
        }
    }
}