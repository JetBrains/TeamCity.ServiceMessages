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

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using ServiceMessages.Read;
    using ServiceMessages.Write;

    [TestFixture]
    public class ServiceMessageFormatterTest
    {
        private static void DoTestParsePresent(string msg)
        {
            Assert.AreEqual(
                msg,
                new ServiceMessageFormatter().FormatMessage(new ServiceMessageParser().ParseServiceMessages(msg).Single())
            );
        }

        [Test]
        public void ErrorOnEmptyField()
        {
            Assert.Throws<InvalidOperationException>(() => new ServiceMessageFormatter().FormatMessage("aaa", new Dictionary<string, string> {{"", ""}}));
        }

        [Test]
        public void ErrorOnEmptyMessage()
        {
            Assert.Throws<ArgumentException>(() => new ServiceMessageFormatter().FormatMessage("", new Dictionary<string, string>()));
        }

        [Test]
        public void ErrorOnEscapingField()
        {
            Assert.Throws<InvalidOperationException>(() => new ServiceMessageFormatter().FormatMessage("aaa", new Dictionary<string, string> {{"\r\n", ""}}));
        }

        [Test]
        public void ErrorOnEscapingMessage()
        {
            Assert.Throws<ArgumentException>(() => new ServiceMessageFormatter().FormatMessage("\r\n", new Dictionary<string, string>()));
        }


        [Test]
        public void SimpleMessage()
        {
            Assert.AreEqual(
                "##teamcity[rulez 'qqq']",
                new ServiceMessageFormatter().FormatMessage("rulez", "qqq"));
        }

        [Test]
        public void SupportAnonymousType()
        {
            Assert.AreEqual(
                "##teamcity[rulez Version='ver' Vika='678' Int='42']",
                new ServiceMessageFormatter().FormatMessage("rulez", new
                {
                    Version = "ver",
                    Vika = "678",
                    Int = 42
                }));
        }

        [Test]
        public void SupportArray()
        {
            Assert.AreEqual(
                "##teamcity[rulez qqq='ppp']",
                new ServiceMessageFormatter().FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp")));
        }

        [Test]
        public void SupportArray2()
        {
            Assert.AreEqual(
                "##teamcity[rulez qqq='ppp' www='xxx']",
                new ServiceMessageFormatter().FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp"), new ServiceMessageProperty("www", "xxx")));
        }

        [Test]
        public void SupportEnumerable()
        {
            Assert.AreEqual(
                "##teamcity[rulez qqq='ppp']",
                new ServiceMessageFormatter().FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp")));
        }

        [Test]
        public void SupportEnumerable2()
        {
            Assert.AreEqual(
                "##teamcity[rulez qqq='ppp' rrr='wqe']",
                new ServiceMessageFormatter().FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp"), new ServiceMessageProperty("rrr", "wqe")));
        }

        [Test]
        public void SupportEscaping()
        {
            Assert.AreEqual(
                "##teamcity[rulez Attribute='\" |' |n |r |x |l |p || |[ |]']",
                new ServiceMessageFormatter().FormatMessage("rulez", new
                {
                    Attribute = "\" ' \n \r \u0085 \u2028 \u2029 | [ ]"
                }));
        }

        [Test]
        public void TestIServiceMessage()
        {
            DoTestParsePresent("##teamcity[rulez qqq='ppp' rrr='wqe']");
        }

        [Test]
        public void TestIServiceMessage2()
        {
            DoTestParsePresent("##teamcity[rulez 'wqe']");
        }

        [Test]
        public void TestIServiceMessageWithNULL()
        {
            Assert.AreEqual(
                "##teamcity[AAA a='' b='' c='']",
                new ServiceMessageFormatter().FormatMessage(new ServiceMessageWithNulls())
            );
        }

        private class ServiceMessageWithNulls : IServiceMessage
        {
            public string Name
            {
                get { return "AAA"; }
            }

            public string DefaultValue
            {
                get { return null; }
            }

            public IEnumerable<string> Keys
            {
                get { return new[] {"a", "b", "c"}; }
            }

            public string GetValue(string key)
            {
                return null;
            }
        }
    }
}