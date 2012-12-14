using System;
using System.Globalization;
using System.Threading;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
  /// <summary>
  /// Helper class to generate FlowIds
  /// </summary>
  /// //TODO: make as interface and allow to change
  public static class FlowId
  {
    private static int myIds;
    /// <summary>
    /// Generates new unique FlowId
    /// </summary>
    /// <returns></returns>
    [NotNull]
    public static string NewFlowId()
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
