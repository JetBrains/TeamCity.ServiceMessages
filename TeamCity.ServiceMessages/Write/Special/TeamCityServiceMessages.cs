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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Impl;
    using Impl.Updater;

    /// <summary>
    ///     Basic implementation of TeamCity service message generation facade
    /// </summary>
    public class TeamCityServiceMessages : ITeamCityServiceMessages
    {
        public TeamCityServiceMessages()
        {
            Formatter = new ServiceMessageFormatter();
            Updaters = new IServiceMessageUpdater[] {new TimestampUpdater(() => DateTime.Now)};
            FlowIdGenerator = new DefaultFlowIdGenerator();
        }

        /// <summary>
        ///     Most specific constructor. Could be used with DI
        /// </summary>
        public TeamCityServiceMessages(
            [NotNull] IServiceMessageFormatter formatter,
            [NotNull] IFlowIdGenerator flowIdGenerator,
            [NotNull] IEnumerable<IServiceMessageUpdater> updaters)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (flowIdGenerator == null) throw new ArgumentNullException(nameof(flowIdGenerator));
            if (updaters == null) throw new ArgumentNullException(nameof(updaters));
            Formatter = formatter;
            FlowIdGenerator = flowIdGenerator;
            Updaters = updaters;
        }

        [NotNull]
        public IServiceMessageFormatter Formatter { get; set; }

        [NotNull]
        public IFlowIdGenerator FlowIdGenerator { get; set; }

        [NotNull]
        public IEnumerable<IServiceMessageUpdater> Updaters { get; set; }

        public void AddServiceMessageUpdater(IServiceMessageUpdater updater)
        {
            if (updater == null) throw new ArgumentNullException(nameof(updater));
            Updaters = Updaters.Union(new[] {updater}).ToArray();
        }

        public ITeamCityWriter CreateWriter()
        {
            return CreateWriter(Console.WriteLine);
        }

        public ITeamCityWriter CreateWriter(Action<string> destination)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            var processor = new FlowServiceMessageWriter(new ServiceMessagesWriter(Formatter, destination), FlowIdGenerator, Updaters.ToList());
            return new TeamCityWriterImpl(
                processor,
                DisposableDelegate.Empty
            );
        }
    }
}