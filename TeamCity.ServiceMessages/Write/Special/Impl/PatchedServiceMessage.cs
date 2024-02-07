

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;
    using System.Linq;

    /// <summary>
    /// Helper implementation of IServiceMessage
    /// </summary>
    internal class PatchedServiceMessage : ServiceMessage
    {
        public PatchedServiceMessage([NotNull] IServiceMessage message)
            : base(message.Name)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            AddRange(message.Keys.ToDictionary(x => x, message.GetValue));
        }
    }
}