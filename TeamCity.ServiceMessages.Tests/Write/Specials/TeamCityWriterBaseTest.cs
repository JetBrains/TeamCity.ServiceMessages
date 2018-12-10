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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using ServiceMessages.Write;
    using ServiceMessages.Write.Special;
    using ServiceMessages.Write.Special.Impl;

    public abstract class TeamCityWriterBaseTest<T>
    {
        protected abstract T Create(IServiceMessageProcessor proc);

        protected virtual ToStringProcessor CreateProcessor()
        {
            return new ToStringProcessor();
        }

        protected void DoTestReplacing(Action<T> action, Func<string, string> replace, params string[] golds)
        {
            if (golds == null || golds.Any(x => x == null)) throw new ArgumentNullException("golds");
            DoTestImpl(action, replace, golds);
        }

        protected void DoTest(Action<T> action, params string[] golds)
        {
            DoTestReplacing(action, x => x, golds);
        }

        protected void DoTestWithoutAsseert(Action<T> action)
        {
            DoTestImpl(action, x => x, null);
        }

        private void DoTestImpl(Action<T> action, Func<string, string> replace, string[] golds)
        {
            var proc = CreateProcessor();
            var writer = Create(proc);

            action(writer);

            if (golds == null) return;

            Func<string, string[]> preprocess = s => s.Split("\r\n".ToCharArray()).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            var actual = preprocess(string.Join("\r\n", proc.Buffer.Select(replace).ToArray()));
            var actualText = "\r\n" + string.Join("\r\n", actual);
            var expected = preprocess(string.Join("\r\n", golds));

            if (actual.Length != expected.Length)
                Assert.Fail("Incorrect number of messages. Was: " + actualText);

            for (var i = 0; i < actual.Count(); i++)
                Assert.AreEqual(expected[i], actual[i], "Message {0} does not match. Was: {1}", i, actualText);
        }

        protected class ToStringProcessor : IServiceMessageProcessor
        {
            private readonly List<string> builder = new List<string>();

            public IEnumerable<string> Buffer => builder.ToArray();

            public virtual void AddServiceMessage(IServiceMessage serviceMessage)
            {
                builder.Add(new ServiceMessageFormatter().FormatMessage(serviceMessage));
            }
        }
    }


    public abstract class TeamCityWriterBaseTest : TeamCityFlowWriterBaseTest<ITeamCityWriter>
    {
        protected override ITeamCityWriter Create(IFlowServiceMessageProcessor proc)
        {
            return new TeamCityWriterImpl(proc, DisposableDelegate.Empty);
        }
    }
}