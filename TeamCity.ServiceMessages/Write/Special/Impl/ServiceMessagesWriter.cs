

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;

    /// <summary>
    /// Simple implementation of ServiceMessage
    /// </summary>
    public class ServiceMessagesWriter : IServiceMessageProcessor
    {
        private readonly IServiceMessageFormatter _formatter;
        private readonly Action<string> _printer;

        public ServiceMessagesWriter([NotNull] IServiceMessageFormatter formatter, [NotNull] Action<string> printer)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (printer == null) throw new ArgumentNullException(nameof(printer));
            _formatter = formatter;
            _printer = printer;
        }

        public void AddServiceMessage(IServiceMessage serviceMessage)
        {
            if (serviceMessage == null) throw new ArgumentNullException(nameof(serviceMessage));
            _printer(_formatter.FormatMessage(serviceMessage));
        }
    }
}