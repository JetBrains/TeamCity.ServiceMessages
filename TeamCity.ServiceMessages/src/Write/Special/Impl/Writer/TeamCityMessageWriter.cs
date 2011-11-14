using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityMessageWriter : BaseWriter, ITeamCityMessageWriter
  {
    public TeamCityMessageWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    private void Write([NotNull] string text, string details, [NotNull] string status)
    {
      var msg = new SimpleServiceMessage("message"){{"text", text}, {"status", status}};
      if (!string.IsNullOrEmpty(details))
        msg.Add("errorDetails", details);

      PostMessage(msg);
    }


    public void WriteMessage(string text)
    {
      Write(text, null, "NORMAL");
    }

    public void WriteWarning(string text)
    {
      Write(text, null, "WARNING");
    }

    public void WriteError(string text, string errorDetails)
    {
      Write(text, errorDetails, "ERROR");
    }
  }
}