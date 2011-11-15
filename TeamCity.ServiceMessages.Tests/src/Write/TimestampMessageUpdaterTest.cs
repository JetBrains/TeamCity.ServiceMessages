using System;
using System.Globalization;
using System.Linq;
using JetBrains.TeamCity.ServiceMessages.Read;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class TimestampMessageUpdaterTest
  {
    [Test]
    public void TestTimestampUpdated_Simple()
    {
      var upd = new TimestampUpdater(() => DateTime.Now);
      var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple 'message']").Single());

      Assert.AreEqual(message.Name, "simple");
      Assert.AreEqual(message.DefaultValue, "message");
      Assert.AreEqual(message.Keys.Any(), false);
    }

    [Test]
    public void TestTimestampUpdated_Attributes()
    {
      var upd = new TimestampUpdater( () => DateTime.Now);
      var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());

      Assert.AreEqual(message.Name, "simple");
      Assert.AreEqual(message.DefaultValue, null);
      Assert.AreEqual(message.Keys.Contains("a"), true);
      Assert.AreEqual(message.GetValue("a"), "message");      
      Assert.AreEqual(message.Keys.Contains("timestamp"), true);
      Assert.AreEqual(message.Keys.Count(), 2);

      Console.Out.WriteLine(message.GetValue("timestamp"));
    }

    [Test]
    public void TestTimeFormat_DE()
    {
      var upd = new TimestampUpdater(() => new DateTime(2012, 12, 12, 12, 12, 12, 12, CultureInfo.GetCultureInfo("de").Calendar));
      var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());
      var timeStamp = message.GetValue("timestamp");

      Console.Out.WriteLine(timeStamp);

      Assert.AreEqual("2012-12-12T12:12:12.012+01:00", timeStamp);
    }
  }
}