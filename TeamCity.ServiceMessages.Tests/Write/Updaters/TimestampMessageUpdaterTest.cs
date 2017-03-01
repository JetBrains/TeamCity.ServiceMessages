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
        [Test]
        public void TestTimeFormat_DE()
        {
            var upd = new TimestampUpdater(() => new DateTime(2012, 12, 12, 12, 12, 12, 12, CultureInfo.GetCultureInfo("de").Calendar));
            var message = upd.UpdateServiceMessage(new ServiceMessageParser().ParseServiceMessages("##teamcity[simple a='message']").Single());
            var timeStamp = message.GetValue("timestamp");

            Assert.NotNull(timeStamp);
            Console.Out.WriteLine(timeStamp);

            var match = Regex.Match(timeStamp, @"^\d{4}-\d{2}-\d{2}T\d{1,2}:\d{2}:\d{2}(\.\d{3})?([-\+]\d{1,2}\d{2})?$");
            Console.Out.WriteLine(match.Value);
            Assert.IsTrue(match.Success);
        }

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

            Console.Out.WriteLine(message.GetValue("timestamp"));
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