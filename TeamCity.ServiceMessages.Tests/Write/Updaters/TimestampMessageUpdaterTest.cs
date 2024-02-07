

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Updaters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using ServiceMessages.Read;
    using ServiceMessages.Write.Special.Impl.Updater;

    [TestFixture]
    public class TimestampMessageUpdaterTest
    {
#if !NETSTANDARD1_6 && !NETCOREAPP1_0 && !NETCOREAPP2_0
        [Test]
        public void TestTimeFormat_DE()
        {
            var upd = new TimestampUpdater(() => new DateTime(2012, 12, 12, 12, 12, 12, 12, CultureInfo.GetCultureInfo("de").Calendar));
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());
            var timeStamp = message.GetValue("timestamp");

            Assert.NotNull(timeStamp);
            Console.WriteLine(timeStamp);

            var match = Regex.Match(timeStamp, @"^\d{4}-\d{2}-\d{2}T\d{1,2}:\d{2}:\d{2}(\.\d{3})?([-\+]\d{1,2}\d{2})?$");
            Console.WriteLine(match.Value);
            Assert.IsTrue(match.Success);
        }
#endif

        [Test]
        public void TestTimestampUpdated_Attributes()
        {
            var upd = new TimestampUpdater(() => DateTime.Now);
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());

            Assert.AreEqual(message.Name, "simple");
            Assert.AreEqual(message.DefaultValue, null);
            Assert.AreEqual(message.Keys.Contains("a"), true);
            Assert.AreEqual(message.GetValue("a"), "message");
            Assert.AreEqual(message.Keys.Contains("timestamp"), true);
            Assert.AreEqual(message.Keys.Count(), 2);

            Console.WriteLine(message.GetValue("timestamp"));
        }

        [Test]
        public void TestTimestampUpdated_Simple()
        {
            var upd = new TimestampUpdater(() => DateTime.Now);
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple 'message']").Single());

            Assert.AreEqual(message.Name, "simple");
            Assert.AreEqual(message.DefaultValue, "message");
            Assert.AreEqual(message.Keys.Any(), false);
        }
    }
}