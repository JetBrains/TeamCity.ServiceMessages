using System.Collections.Generic;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  [TestFixture]
  public class TeamCityWriterMessageTest : TeamCityWriterBaseTest
  {
    [Test]
    public void TestCustomServiceMessage_Simple()
    {
      DoTest(x => x.WriteRawMessage(new SimpleServiceMessage()), "##teamcity[ThisIsTheSimple 'Default']");
    }

    [Test]
    public void TestCustomServiceMessage_Complex()
    {
      DoTest(x => x.WriteRawMessage(new ComplexServiceMessage()), "##teamcity[ThisIsTheName a='a' b='b' c='c' flowId='1']");
    }

    private class ComplexServiceMessage : IServiceMessage
    {
      public string Name
      {
        get { return "ThisIsTheName"; }
      }

      public string DefaultValue
      {
        get { return null; }
      }

      public IEnumerable<string> Keys
      {
        get { return new[] { "a", "b", "c" }; }
      }

      public string GetValue(string key)
      {
        return key;
      }
    }

    private class SimpleServiceMessage : IServiceMessage
    {
      public string Name
      {
        get { return "ThisIsTheSimple"; }
      }

      public string DefaultValue
      {
        get { return "Default"; }
      }

      public IEnumerable<string> Keys
      {
        get { return new string[0]; }
      }

      public string GetValue(string key)
      {
        return key;
      }
    }
  }
}