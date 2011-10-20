using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.TeamCity.ServiceMessages.Read;
using NUnit.Framework;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Read
{
  [TestFixture]
  public class ServiceMessageParserTest
  {
    [Test]
    public void BrokenStream_0()
    {
      var sb = new StringBuilder();
      for (int c = char.MinValue; c < char.MaxValue; sb.Append((char)c++)) ;
      for (int c = char.MinValue; c < char.MaxValue; sb.Append((char)c++)) ;

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_1()
    {
      var sb = new StringBuilder();
      sb.Append("##te");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_2()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_3()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[]");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_4()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[ ]");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_5()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa ]");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_6()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa '");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_7()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa ");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_8()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_9()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa Z");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_A()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa Z =");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_B()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa Z = '");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_C()
    {
      var sb = new StringBuilder();
      sb.Append("##teamcity[aa Z = 'fddd");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void BrokenStream_D()
    {
      var sb = new StringBuilder(); 
      sb.Append("##teamcity[aa Z = 'fddd '   ");

      Assert.IsFalse(ServiceMessageParser.ParseServiceMessages(new StringReader(sb.ToString())).ToArray().Any());
    }

    [Test]
    public void ShouldParseService_simpleMessage()
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
    public void ShouldParseService_simpleMessages_multi()
    {
      var sr = new StringReader("##teamcity[name 'a'] ##teamcity[name 'a'] ##teamcity[name 'a']  ##teamcity[name 'a']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(4, result.Length);

      foreach (var msg in result)
      {
        Assert.AreEqual("name", msg.Name);
        Assert.AreEqual(0, msg.Keys.Count());
        Assert.AreEqual("a", msg.DefaultValue);
      }
    }

    [Test]
    public void ShouldParseService_simpleMessages_multi2()
    {
      var sr = new StringReader("##teamcity[name 'a']\r ##teamcity[name 'a']\n ##teamcity[name 'a'] \r\n ##teamcity[name 'a']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(4, result.Length);

      foreach (var msg in result)
      {
        Assert.AreEqual("name", msg.Name);
        Assert.AreEqual(0, msg.Keys.Count());
        Assert.AreEqual("a", msg.DefaultValue);
      }
    }

    [Test]
    public void ShouldParseService_simpleMessage_decode()
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
    public void ShouldParseService_complexMessage0()
    {
      var sr = new StringReader("##teamcity[name a='a' b='z']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg.GetValue("a"));
      Assert.AreEqual("z", msg.GetValue("b"));
    }

    [Test]
    public void ShouldParseService_complexMessage1()
    {
      var sr = new StringReader("##teamcity[name a='a']");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(1, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg.GetValue("a"));
    }
    

    [Test]
    public void ShouldParseService_complexMessage2()
    {
      var sr = new StringReader("##teamcity[name    a='a'     b='z'   ]");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg.GetValue("a"));
      Assert.AreEqual("z", msg.GetValue("b"));
    }

    [Test]
    public void ShouldParseService_complexMessage3()
    {
      var sr = new StringReader("  ##teamcity[name a  =  'a'   ]  ");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(1, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string> { "a" }, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("a", msg.GetValue("a"));
    }

    [Test]
    public void ShouldParseService_complexMessage_multi()
    {
      var sr = new StringReader("  ##teamcity[name a='z']##teamcity[name a='z']##teamcity[name a='z']  ");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(3, result.Length);

      foreach (var msg in result)
      {
        Assert.AreEqual("name", msg.Name);
        Assert.AreEqual(1, msg.Keys.Count());
        Assert.AreEqual(new HashSet<string> { "a" }, new HashSet<string>(msg.Keys));
        Assert.AreEqual(null, msg.DefaultValue);
        Assert.AreEqual("z", msg.GetValue("a"));  
      }
    }

    [Test]
    public void ShouldParseService_complexMessage_multi2()
    {
      var sr = new StringReader("  ##teamcity[name a='z']\r  ##teamcity[name a='z']\n##teamcity[name a='z']  ");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(3, result.Length);

      foreach (var msg in result)
      {
        Assert.AreEqual("name", msg.Name);
        Assert.AreEqual(1, msg.Keys.Count());
        Assert.AreEqual(new HashSet<string> { "a" }, new HashSet<string>(msg.Keys));
        Assert.AreEqual(null, msg.DefaultValue);
        Assert.AreEqual("z", msg.GetValue("a"));  
      }
    }

    [Test]
    public void ShouldParseService_multi()
    {
      var sr = new StringReader("  ##teamcity[name a='z']\r##teamcity[zzz 'z']\n##teamcity[name a='z']  ");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(3, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(1, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string> { "a" }, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("z", msg.GetValue("a"));  

      msg = result[1];
      Assert.AreEqual("zzz", msg.Name);
      Assert.AreEqual(0, msg.Keys.Count());
      Assert.AreEqual("z", msg.DefaultValue);

      msg = result[2];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(1, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string> { "a" }, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("z", msg.GetValue("a"));  
    }

    [Test]
    public void ShouldParseService_complexMessage_escaping()
    {
      var sr = new StringReader("##teamcity[name    a='1\"|'|n|r|x|l|p||[|]'     b='2\"|'|n|r|x|l|p||[|]'   ]");
      var result = ServiceMessageParser.ParseServiceMessages(sr).ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string>{"a", "b"}, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("1\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("a"));
      Assert.AreEqual("2\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("b"));
    }

    [Test]
    public void ShouldParseString()
    {
      var result = ServiceMessageParser.ParseServiceMessages("##teamcity[name    a='1\"|'|n|r|x|l|p||[|]'     b='2\"|'|n|r|x|l|p||[|]'   ]").ToArray();
      Assert.AreEqual(1, result.Length);

      var msg = result[0];
      Assert.AreEqual("name", msg.Name);
      Assert.AreEqual(2, msg.Keys.Count());
      Assert.AreEqual(new HashSet<string> { "a", "b" }, new HashSet<string>(msg.Keys));
      Assert.AreEqual(null, msg.DefaultValue);
      Assert.AreEqual("1\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("a"));
      Assert.AreEqual("2\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("b"));
    }
  }
}