

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class TeamCityArtifactsWriter : BaseWriter, ITeamCityArtifactsWriter
    {
        public TeamCityArtifactsWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }

        public void PublishArtifact(string rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            PostMessage(new ValueServiceMessage("publishArtifacts", rules));
        }
    }
}