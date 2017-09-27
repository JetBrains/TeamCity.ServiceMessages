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

    public class TeamCityMessageWriter : BaseWriter, ITeamCityMessageWriter
    {
        public TeamCityMessageWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }


        public void WriteMessage(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, null, "NORMAL");
        }

        public void WriteWarning(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, null, "WARNING");
        }

        public void WriteError(string text, string errorDetails)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Write(text, errorDetails, "ERROR");
        }

        private void Write([NotNull] string text, string details, [NotNull] string status)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (status == null) throw new ArgumentNullException(nameof(status));
            var msg = new ServiceMessage("message") {{"text", text}, {"status", status}, {"tc:tags", "tc:parseServiceMessagesInside"}};
            if (!string.IsNullOrEmpty(details))
            {
                msg.Add("errorDetails", details);
            }

            PostMessage(msg);
        }
    }
}