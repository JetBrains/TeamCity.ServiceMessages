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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
    using System;

    /// <summary>
    ///     Simple implementation of ServiceMessage
    /// </summary>
    public class ServiceMessagesWriter : IServiceMessageProcessor
    {
        private readonly IServiceMessageFormatter _formatter;
        private readonly Action<string> _printer;

        public ServiceMessagesWriter([NotNull] IServiceMessageFormatter formatter, [NotNull] Action<string> printer)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (printer == null) throw new ArgumentNullException(nameof(printer));
            _formatter = formatter;
            _printer = printer;
        }

        public void AddServiceMessage(IServiceMessage serviceMessage)
        {
            if (serviceMessage == null) throw new ArgumentNullException(nameof(serviceMessage));
            _printer(_formatter.FormatMessage(serviceMessage));
        }
    }
}