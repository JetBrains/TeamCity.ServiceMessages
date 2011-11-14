using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Helper implementation of IServiceMessage
  /// </summary>
  internal class PatchedServiceMessage : SimpleServiceMessage, IServiceMessage
  {
    public PatchedServiceMessage(IServiceMessage message) : base(message.Name)
    {      
      AddRange(message.Keys.ToDictionary(x => x, message.GetValue));
    }
  }
}