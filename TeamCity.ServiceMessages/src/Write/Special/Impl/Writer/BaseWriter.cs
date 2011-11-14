namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class BaseWriter
  {
    private readonly IServiceMessageProcessor myTarget;

    protected BaseWriter(IServiceMessageProcessor target)
    {
      myTarget = target;
    }

    protected BaseWriter(BaseWriter writer) : this(writer.myTarget)
    {      
    }

    protected void PostMessage(IServiceMessage message)
    {
      myTarget.AddServiceMessage(message);
    }
  }
}