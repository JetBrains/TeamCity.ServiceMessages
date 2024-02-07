

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class TeamCityMessageWriter : BaseWriter, ITeamCityMessageWriter
    {
        public TeamCityMessageWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }


        public void WriteMessage(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, null, "NORMAL");
        }

        public void WriteWarning(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, null, "WARNING");
        }

        public void WriteError(string text, string errorDetails)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, errorDetails, "ERROR");
        }

        private void Write([NotNull] string text, string details, [NotNull] string status)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (status == null) throw new ArgumentNullException(nameof(status));
            var msg = new ServiceMessage("message") {{"text", text}, {"status", status}, {"tc:tags", "tc:parseServiceMessagesInside"}};
            if (!string.IsNullOrEmpty(details))
            {
                msg.Add("errorDetails", details);
            }

            PostMessage(msg);
        }
    }
}