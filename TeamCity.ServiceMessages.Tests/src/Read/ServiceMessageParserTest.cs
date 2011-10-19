using System.Collections.Generic;
using System.IO;
using JetBrains.TeamCity.ServiceMessages.Read;
using NUnit.Framework;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Read
{
  [TestFixture]
  public class ServiceMessageParserTest
  {

    [Test]
    public void SouldParseService_simpleMessage()
    {
      var sr = new StringReader("##teamcity[name 'a']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(0, msg.Keys.Count());
      Assert.AreEqual("a", msg.DefaultValue);
    }

    [Test]
    public void SouldParseService_simpleMessage_decode()
    {
      var sr = new StringReader("##teamcity[name '\"|'|n|r|x|l|p||[|]']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(0, msg.Keys.Count());
      Assert.AreEqual("\"'\n\r\u0085\u2028\u2029|[]", msg.DefaultValue);
    }

    [Test]
    public void SouldParseService_complexMessage()
    {
      var sr = new StringReader("##teamcity[name a='a' b='z']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg["a"]);
      Assert.AreEqual("z", msg["b"]);
    }

    [Test]
    public void SouldParseService_complexMessage2()
    {
      var sr = new StringReader("##teamcity[name    a='a'     b='z'   ]");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg["a"]);
      Assert.AreEqual("z", msg["b"]);
    }

    [Test]
    public void SouldParseService_complexMessage_escaping()
    {
      var sr = new StringReader("##teamcity[name    a='1\"|'|n|r|x|l|p||[|]'     b='2\"|'|n|r|x|l|p||[|]'   ]");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("1\"'\n\r\u0085\u2028\u2029|[]", msg["a"]);
      Assert.AreEqual("2\"'\n\r\u0085\u2028\u2029|[]", msg["b"]);
    }

 
  }
}