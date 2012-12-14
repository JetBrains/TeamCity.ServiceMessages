using System;
using System.Globalization;
using System.Threading;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Helper class to generate FlowIds
  /// </summary>  
  public class DefaultFlowIdGenerator : IFlowIdGenerator
  {
    private static int myIds;
    /// <summary>
    /// Generates new unique FlowId
    /// </summary>
    public string NewFlowId()
    {
      return
        (
          Interlocked.Increment(ref myIds) << 27
          +
          (Thread.CurrentThread.ManagedThreadId << 21)
          +
          (Environment.TickCount%int.MaxValue)
        )
          .ToString(CultureInfo.InvariantCulture);
    }
  }
}