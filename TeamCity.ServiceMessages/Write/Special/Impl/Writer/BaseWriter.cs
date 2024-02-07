

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class BaseWriter<TProc> where TProc : IServiceMessageProcessor
    {
        // ReSharper disable once InconsistentNaming
        protected readonly TProc myTarget;

        protected BaseWriter([NotNull] TProc target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            myTarget = target;
        }

        protected BaseWriter(BaseWriter<TProc> writer) : this(writer.myTarget)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
        }

        protected void PostMessage(IServiceMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            myTarget.AddServiceMessage(message);
        }
    }

    public class BaseWriter : BaseWriter<IServiceMessageProcessor>
    {
        protected BaseWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }

        protected BaseWriter(BaseWriter<IServiceMessageProcessor> writer)
            : base(writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
        }
    }
}