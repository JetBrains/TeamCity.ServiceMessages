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
    using System.Globalization;

    public class TeamCityTestWriter : BaseDisposableWriter<IServiceMessageProcessor>, ITeamCityTestWriter
    {
        private readonly string _testName;
        private TimeSpan? _duration;

        public TeamCityTestWriter([NotNull] IServiceMessageProcessor target, [NotNull] string testName, [NotNull] IDisposable disposableHandler)
            : base(target, disposableHandler)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (disposableHandler == null) throw new ArgumentNullException(nameof(disposableHandler));
            _testName = testName ?? throw new ArgumentNullException(nameof(testName));
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

        public void WriteImage(string teamCityArtifactUri, string description = "")
        {
            if (string.IsNullOrEmpty(teamCityArtifactUri)) throw new ArgumentException(nameof(teamCityArtifactUri));
            if (description == null) throw new ArgumentNullException(nameof(description));
            var message = new ServiceMessage("testMetadata") {{"testName", _testName}, {"type", "image"}, {"value", teamCityArtifactUri}};
            if (!string.IsNullOrEmpty(description))
            {
                message.Add("name", description);
            }

            PostMessage(message);
        }

        public void WriteFile(string teamCityArtifactUri, string description = "")
        {
            if (string.IsNullOrEmpty(teamCityArtifactUri)) throw new ArgumentException(nameof(teamCityArtifactUri));
            if (description == null) throw new ArgumentNullException(nameof(description));
            var message = new ServiceMessage("testMetadata") {{"testName", _testName}, {"type", "artifact"}, {"value", teamCityArtifactUri}};
            if (!string.IsNullOrEmpty(description))
            {
                message.Add("name", description);
            }

            PostMessage(message);
        }

        public void WriteValue(double value, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            PostMessage(new ServiceMessage("testMetadata") {{"testName", _testName}, {"type", "number"}, {"value", value.ToString(CultureInfo.InvariantCulture)}, {"name", name}});
        }

        public void WriteValue(string value, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            PostMessage(new ServiceMessage("testMetadata") {{"testName", _testName}, {"value", value}, {"name", name}});
        }

        public void WriteLink(string linkUri, string name)
        {
            if (string.IsNullOrEmpty(linkUri)) throw new ArgumentException(nameof(linkUri));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            PostMessage(new ServiceMessage("testMetadata") {{"testName", _testName}, {"type", "link"}, {"value", linkUri}, {"name", name}});
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