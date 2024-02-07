

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Service message updater that adds Timestamp to service message according to
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity
    /// </summary>
    public class TimestampUpdater : IServiceMessageUpdater
    {
        private readonly Func<DateTime> _timeService;

        public TimestampUpdater([NotNull] Func<DateTime> timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public IServiceMessage UpdateServiceMessage(IServiceMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (message.DefaultValue != null || message.GetValue("timestamp") != null) return message;
            return new PatchedServiceMessage(message) {{"timestamp", _timeService().ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff", CultureInfo.InvariantCulture) + "+0000"}};
        }
    }
}