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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;
    using System.Globalization;

    public class TeamCityTestWriter : BaseDisposableWriter<IServiceMessageProcessor>, ITeamCityTestWriter
    {
        private readonly string _testName;
        private TimeSpan? _duration;

        public TeamCityTestWriter([NotNull] IServiceMessageProcessor target, [NotNull] string testName, [NotNull] IDisposable disposableHander)
            : base(target, disposableHander)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (testName == null) throw new ArgumentNullException(nameof(testName));
            if (disposableHander == null) throw new ArgumentNullException(nameof(disposableHander));
            _testName = testName;
        }

        public void WriteStdOutput(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            //##teamcity[testStdOut name='testname' out='text' tc:tags='tc:parseServiceMessagesInside']
            PostMessage(new ServiceMessage("testStdOut") {{"name", _testName}, {"out", text}, {"tc:tags", "tc:parseServiceMessagesInside"}});
        }

        public void WriteErrOutput(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            //##teamcity[testStdErr name='testname' out='error text' tc:tags='tc:parseServiceMessagesInside']
            PostMessage(new ServiceMessage("testStdErr") {{"name", _testName}, {"out", text}, {"tc:tags", "tc:parseServiceMessagesInside"}});
        }

        public void WriteIgnored(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            WriteIgnoredImpl(message);
        }

        public void WriteIgnored()
        {
            WriteIgnoredImpl(null);
        }

        public void WriteFailed(string errorMessage, string errorDetails)
        {
            if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
            if (errorDetails == null) throw new ArgumentNullException(nameof(errorDetails));
            PostMessage(new ServiceMessage("testFailed") {{"name", _testName}, {"message", errorMessage}, {"details", errorDetails}});
        }

        public void WriteDuration(TimeSpan span)
        {
            _duration = span;
        }

        public void OpenTest()
        {
            PostMessage(new ServiceMessage("testStarted") {{"name", _testName}, {"captureStandardOutput", "false"}});
        }

        protected override void DisposeImpl()
        {
            var msg = new ServiceMessage("testFinished") {{"name", _testName}};
            if (_duration != null)
                msg.Add("duration", ((long) _duration.Value.TotalMilliseconds).ToString(CultureInfo.InvariantCulture));
            PostMessage(msg);
        }

        private void WriteIgnoredImpl([CanBeNull] string message)
        {
            var msg = new ServiceMessage("testIgnored") {{"name", _testName}};
            if (message != null)
            {
                msg.Add("message", message);
            }

            PostMessage(msg);
        }
    }
}