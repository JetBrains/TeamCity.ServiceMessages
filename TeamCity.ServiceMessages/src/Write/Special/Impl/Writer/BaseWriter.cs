namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class BaseWriter
  {
    private readonly IServiceMessageProcessor myTarget;

    public BaseWriter(IServiceMessageProcessor target)
    {
      myTarget = target;
    }

    public void PostMessage(IServiceMessage message)
    {
      myTarget.AddServiceMessage(message);
    }
  }
}