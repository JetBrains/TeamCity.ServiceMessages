/*
 * Copyright 2007-2017 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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