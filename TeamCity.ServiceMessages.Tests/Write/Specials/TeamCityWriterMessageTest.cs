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

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TeamCityWriterMessageTest : TeamCityWriterBaseTest
    {
        [Test]
        public void TestCustomServiceMessage_Complex()
        {
            DoTest(x => x.WriteRawMessage(new ComplexServiceMessage()), "##teamcity[ThisIsTheName a='a' b='b' c='c' flowId='1']");
        }

        [Test]
        public void TestCustomServiceMessage_Simple()
        {
            DoTest(x => x.WriteRawMessage(new SimpleServiceMessage()), "##teamcity[ThisIsTheSimple 'Default']");
        }

        private class ComplexServiceMessage : IServiceMessage
        {
            public string Name
            {
                get { return "ThisIsTheName"; }
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
                return key;
            }
        }

        private class SimpleServiceMessage : IServiceMessage
        {
            public string Name
            {
                get { return "ThisIsTheSimple"; }
            }

            public string DefaultValue
            {
                get { return "Default"; }
            }

            public IEnumerable<string> Keys
            {
                get { return new string[0]; }
            }

            public string GetValue(string key)
            {
                return key;
            }
        }
    }
}