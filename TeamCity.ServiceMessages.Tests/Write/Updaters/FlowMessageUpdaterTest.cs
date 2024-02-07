

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Updaters
{
    using System.Linq;
    using NUnit.Framework;
    using ServiceMessages.Read;
    using ServiceMessages.Write.Special.Impl.Updater;

    [TestFixture]
    public class FlowMessageUpdaterTest
    {
        [Test]
        public void TestFlowIdUpdated_Attributes()
        {
            var upd = new FlowMessageUpdater("aaa");
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());

            Assert.AreEqual(message.Name, "simple");
            Assert.AreEqual(message.DefaultValue, null);
            Assert.AreEqual(message.Keys.Contains("a"), true);
            Assert.AreEqual(message.GetValue("a"), "message");
            Assert.AreEqual(message.Keys.Contains("flowId"), true);
            Assert.AreEqual(message.GetValue("flowId"), "aaa");
            Assert.AreEqual(message.Keys.Count(), 2);
        }

        [Test]
        public void TestFlowIdUpdated_Simple()
        {
            var upd = new FlowMessageUpdater("aaa");
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple 'message']").Single());

            Assert.AreEqual(message.Name, "simple");
            Assert.AreEqual(message.DefaultValue, "message");
            Assert.AreEqual(message.Keys.Any(), false);
        }
    }
}