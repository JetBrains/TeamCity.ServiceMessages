using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests
{
  [TestFixture]
  public class ServiceMessageReplacementsTest
  {
    [Test]
    public void TestEncode_0()
    {
      Assert.AreEqual("", ServiceMessageReplacements.Encode(""));
    }

    [Test]
    public void TestEncode_1()
    {
      Assert.AreEqual("a", ServiceMessageReplacements.Encode("a"));
    }

    [Test]
    public void TestEncode_1_special()
    {
      Assert.AreEqual("|n", ServiceMessageReplacements.Encode("\n"));
      Assert.AreEqual("|r", ServiceMessageReplacements.Encode("\r"));
      Assert.AreEqual("|]", ServiceMessageReplacements.Encode("]"));
      Assert.AreEqual("|'", ServiceMessageReplacements.Encode("'"));
      Assert.AreEqual("||", ServiceMessageReplacements.Encode("|"));
      Assert.AreEqual("|x", ServiceMessageReplacements.Encode("\u0085"));
      Assert.AreEqual("|l", ServiceMessageReplacements.Encode("\u2028"));
      Assert.AreEqual("|p", ServiceMessageReplacements.Encode("\u2029"));
    }

    [Test]
    public void TestDecode_0()
    {
      Assert.AreEqual("", ServiceMessageReplacements.Decode(""));
    }

    [Test]
    public void TestDecode_1()
    {
      Assert.AreEqual("a", ServiceMessageReplacements.Decode("a"));
    }

    [Test]
    public void TestDecode_1_special()
    {
      Assert.AreEqual(ServiceMessageReplacements.Decode("|n"), "\n");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|r"), "\r");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|]"), "]");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|'"), "'");
      Assert.AreEqual(ServiceMessageReplacements.Decode("||"), "|");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|x"), "\u0085");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|l"), "\u2028");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|p"), "\u2029");
    }
  }
}