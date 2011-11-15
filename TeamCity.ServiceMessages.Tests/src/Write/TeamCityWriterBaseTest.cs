using System;
using System.Text;
using JetBrains.TeamCity.ServiceMessages.Write;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NUnit.Framework;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  public abstract class TeamCityWriterBaseTest<T> : IServiceMessageProcessor
  {
    private T myWriter;
    private StringBuilder myBuffer;

    protected T Writer { get { return myWriter; } }
    protected string Buffer { get { return myBuffer.ToString(); } }

    [SetUp]
    public virtual void SetUp()
    {
      myBuffer = new StringBuilder();
      myWriter = Create(this);
    }

    protected abstract T Create(IServiceMessageProcessor proc);

    void IServiceMessageProcessor.AddServiceMessage(IServiceMessage serviceMessage)
    {
      myBuffer.AppendLine(new ServiceMessageFormatter().FormatMessage(serviceMessage));
    }

    protected void DoTest(Action<T> action, params string[] golds)
    {
      action(myWriter);

      Func<string, string[]> preprocess = s => s.Split("\r\n".ToCharArray()).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();

      var actual = preprocess(Buffer);
      var actualText = "\r\n" + string.Join("\r\n", actual);
      var expected =  preprocess(string.Join("\r\n", golds));

      if (actual.Count() != expected.Count())
      {
        Assert.Fail("Incorrect number of messages. Was: " + actualText);
      }

      for(int i = 0;  i < actual.Count(); i++)
      {
        Assert.AreEqual(actual[i], expected[i], "Message {0} does not match. Was: {1}", i, actualText);
      }
    }
    
  }
}