/*
 * Copyright 2007-2019 JetBrains s.r.o.
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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;
    using System.Collections.Generic;

    public class TeamCityTestSuiteBlock : BaseDisposableWriter<IFlowAwareServiceMessageProcessor>, ITeamCityTestsSubWriter, ISubWriter
    {
        private readonly TeamCityFlowWriter<ITeamCityTestsSubWriter> _flows;
        private Stack<string> _childSuitesOpened = new Stack<string>();
        private Stack<string> _childTestsOpened = new Stack<string>();

        public TeamCityTestSuiteBlock([NotNull] IFlowAwareServiceMessageProcessor target, [NotNull] IDisposable disposableHandler)
            : base(target, disposableHandler)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (disposableHandler == null) throw new ArgumentNullException(nameof(disposableHandler));
            _flows = new TeamCityFlowWriter<ITeamCityTestsSubWriter>(
                target,
                (handler, writer) => new TeamCityTestSuiteBlock(writer, handler),
                DisposableDelegate.Empty);
        }

        public void AssertNoChildOpened()
        {
            if (_childSuitesOpened.Count != 0)
            {
                throw new InvalidOperationException($"TeamCity.ServiceMessages: Expected no test suites open, but found {_childSuitesOpened.Count}: [{string.Join(", ", _childSuitesOpened.ToArray())}]");
            }

            if (_childTestsOpened.Count != 0)
            {
                throw new InvalidOperationException($"TeamCity.ServiceMessages: Expected no tests open, but found {_childTestsOpened.Count}: [{string.Join(", ", _childTestsOpened.ToArray())}]");
            }

            _flows.AssertNoChildOpened();
        }

        public ITeamCityTestsSubWriter OpenFlow()
        {
            AssertNoChildOpened();
            return _flows.OpenFlow();
        }

        public ITeamCityTestsSubWriter OpenTestSuite(string suiteName)
        {
            if (suiteName == null) throw new ArgumentNullException(nameof(suiteName));
            AssertNoChildOpened();
            _childSuitesOpened.Push(suiteName);
            PostMessage(new ServiceMessage("testSuiteStarted") {{"name", suiteName}});

            return new TeamCityTestSuiteBlock(
                myTarget,
                new DisposableDelegate(() =>
                {
                    PostMessage(new ServiceMessage("testSuiteFinished") {{"name", suiteName}});
                    var result = _childSuitesOpened.Pop();
                    if (result != suiteName)
                        PostWarning($"TeamCity.ServiceMessages: Unexpected order of dispose - expected suite {result} to be disposed, but {suiteName} was disposed instead");
                }));
        }

        private void PostWarning(string text)
        {
            var msg = new ServiceMessage("message") {{"text", text}, {"status", "ERROR"}, {"tc:tags", "tc:parseServiceMessagesInside"}};
            PostMessage(msg);
        }

        public ITeamCityTestWriter OpenTest(string testName)
        {
            if (testName == null) throw new ArgumentNullException(nameof(testName));
            AssertNoChildOpened();

            _childTestsOpened.Push(testName);
            var writer = new TeamCityTestWriter(myTarget, testName, new DisposableDelegate(() =>
            {
                var result = _childTestsOpened.Pop();
                if (result != testName)
                    PostWarning($"TeamCity.ServiceMessages: Unexpected order of dispose - expected test {result} to be disposed, but {testName} was disposed instead");
            }));
            writer.OpenTest();
            return writer;
        }

        protected override void DisposeImpl()
        {
            if (_childSuitesOpened.Count != 0)
            {
                throw new InvalidOperationException($"TeamCity.ServiceMessages: Expected all test suite writers to be disposed, but found {_childSuitesOpened.Count}: [{string.Join(", ", _childSuitesOpened.ToArray())}] were not.");
            }

            if (_childTestsOpened.Count != 0)
            {
                throw new InvalidOperationException($"TeamCity.ServiceMessages: Expected all test writers to be disposed, but found {_childTestsOpened.Count}: [{string.Join(", ", _childTestsOpened.ToArray())}] were not.");
            }
            
            _flows.Dispose();
        }
    }
}
