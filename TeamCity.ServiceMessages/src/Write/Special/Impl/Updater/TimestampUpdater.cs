using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
  /// <summary>
  /// Service message updater that adds Timestamp to service message according to
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity
  /// </summary>
  public class TimestampUpdater : IServiceMessageUpdater
  {
    public IServiceMessage UpdateServiceMessage(IServiceMessage message)
    {
      return new PatchedServiceMessage(message) { { "Timestamp", DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.SSSZ") } };
    }
  }
}