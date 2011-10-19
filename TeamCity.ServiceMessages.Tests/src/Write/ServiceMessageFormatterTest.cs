using JetBrains.TeamCity.ServiceMessages.Write;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class ServiceMessageFormatterTest
  {
    [Test]
    public void SupportAnonymousType()
    {
      Assert.AreEqual(
        "##teamcity[rulez Version='ver' Vika='678' Int='42']",
        ServiceMessageFormatter.FormatMessage("rulez", new
                                                         {
                                                           Version = "ver",
                                                           Vika = "678",
                                                           Int = 42
                                                         }));
    }

    [Test]
    public void SupportEscaping()
    {
      Assert.AreEqual(
        "##teamcity[rulez Attribute='\" |' |n |r |x |l |p || [ |]']",
        ServiceMessageFormatter.FormatMessage("rulez", new
                                                         {
                                                           Attribute = "\" ' \n \r \u0085 \u2028 \u2029 | [ ]",
                                                         }));
    }
    
 
    [Test]
    public void SimpleMessage()
    {
      Assert.AreEqual(
        "##teamcity[rulez 'qqq']",
        ServiceMessageFormatter.FormatMessage("rulez", "qqq"));
    }

    [Test]
    public void SupportArray()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp']",
        ServiceMessageFormatter.FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp")));
    }

    [Test]
    public void SupportEnumerable()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp']",
        ServiceMessageFormatter.FormatMessage("rulez", new [] {new ServiceMessageProperty("qqq", "ppp")}));
    }
  }
}