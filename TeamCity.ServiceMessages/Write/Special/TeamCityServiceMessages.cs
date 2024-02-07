

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Impl;
    using Impl.Updater;

    /// <summary>
    /// Basic implementation of TeamCity service message generation facade
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
        /// Most specific constructor. Could be used with DI
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

        public ITeamCityWriter CreateWriter() => CreateWriter(Console.WriteLine, true);

        public ITeamCityWriter CreateWriter(Action<string> destination, bool addFlowIdsOnTopLevelMessages = true)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var rootServiceMessageFlowId = addFlowIdsOnTopLevelMessages ? FlowIdGenerator.NewFlowId() : null;
            var processor = new FlowAwareServiceMessageWriter(
                rootServiceMessageFlowId,
                new ServiceMessagesWriter(Formatter, destination),
                FlowIdGenerator,
                Updaters.ToList());

            return new TeamCityWriterImpl(
                processor,
                DisposableDelegate.Empty
            );
        }
    }
}