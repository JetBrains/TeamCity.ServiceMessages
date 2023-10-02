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

    public class TeamCityTestSuiteBlock : BaseDisposableWriter<IFlowAwareServiceMessageProcessor>, ITeamCityTestsSubWriter, ISubWriter
    {
        private readonly TeamCityFlowWriter<ITeamCityTestsSubWriter> _flows;
        private bool _isChildSuiteOpened;
        private bool _isChildTestOpened;
        private string _childSuiteName;
        private string _childTestName;

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
            if (_isChildSuiteOpened)
            {
                throw new InvalidOperationException($"Expected no child test suites to be open, but a child test suite named '{_childSuiteName}' is open");
            }

            if (_isChildTestOpened)
            {
                throw new InvalidOperationException($"Expected no child tests to be open, but a child test named '{_childTestName}' is open");
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

            _isChildSuiteOpened = true;
            _childSuiteName = suiteName;

            PostMessage(new ServiceMessage("testSuiteStarted") {{"name", suiteName}});

            return new TeamCityTestSuiteBlock(
                myTarget,
                new DisposableDelegate(() =>
                {
                    PostMessage(new ServiceMessage("testSuiteFinished") {{"name", suiteName}});
                    _isChildSuiteOpened = false;
                    _childSuiteName = null;
                }));
        }

        public ITeamCityTestWriter OpenTest(string testName)
        {
            if (testName == null) throw new ArgumentNullException(nameof(testName));

            AssertNoChildOpened();

            _isChildTestOpened = true;
            _childTestName = testName;

            var writer = new TeamCityTestWriter(myTarget, testName, new DisposableDelegate(() =>
            {
                _isChildTestOpened = false;
                _childTestName = null;
            }));

            writer.OpenTest();
            return writer;
        }

        protected override void DisposeImpl()
        {
            if (_isChildSuiteOpened)
            {
                throw new InvalidOperationException($"Expected all test suite writers to be disposed, but writer for suite '{_childSuiteName}' was not disposed");
            }

            if (_isChildTestOpened)
            {
                throw new InvalidOperationException($"Expected all test writers to be disposed, but writer for test '{_childTestName}' was not disposed");
            }

            _flows.Dispose();
        }
    }
}