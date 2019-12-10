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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Service message updater that adds Timestamp to service message according to
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity
    /// </summary>
    public class TimestampUpdater : IServiceMessageUpdater
    {
        private readonly Func<DateTime> _timeService;

        public TimestampUpdater([NotNull] Func<DateTime> timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public IServiceMessage UpdateServiceMessage(IServiceMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (message.DefaultValue != null || message.GetValue("timestamp") != null) return message;
            return new PatchedServiceMessage(message) {{"timestamp", _timeService().ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff", CultureInfo.InvariantCulture) + "+0000"}};
        }
    }
}