using System;
using System.Globalization;
using System.Threading;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
  /// <summary>
  /// Helper class to generate FlowIds
  /// </summary>
  public static class FlowId
  {
    /// <summary>
    /// Generates new unique FlowId
    /// </summary>
    /// <returns></returns>
    [NotNull]
    public static string NewFlowId()
    {
      return
        (
          (Thread.CurrentThread.ManagedThreadId << 21)
          +
          (Environment.TickCount%int.MaxValue)
        )
          .ToString(CultureInfo.InvariantCulture);
    }
  }
}
