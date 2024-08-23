namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class TeamCityProgressWriter : BaseWriter, ITeamCityProgressWriter
    {
        public TeamCityProgressWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }

        public void WriteProgress(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            PostMessage(new ValueServiceMessage("progressMessage", message));
        }

        public IDisposable OpenProgress(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            PostMessage(new ValueServiceMessage("progressStart", message));
            return new DisposableDelegate(() => PostMessage(new ValueServiceMessage("progressFinish", message)));
        }
    }
}