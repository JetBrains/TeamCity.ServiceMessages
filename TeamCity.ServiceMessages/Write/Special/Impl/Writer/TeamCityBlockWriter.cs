

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    public class TeamCityBlockWriter<TCloseBlock> : BaseWriter, ITeamCityBlockWriter<TCloseBlock>, ISubWriter
        where TCloseBlock : IDisposable
    {
        private readonly Func<IDisposable, TCloseBlock> _closeBlock;
        private int _isChildOpenned;

        public TeamCityBlockWriter(IServiceMessageProcessor target, Func<IDisposable, TCloseBlock> closeBlock)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (closeBlock == null) throw new ArgumentNullException(nameof(closeBlock));
            _closeBlock = closeBlock;
        }

        public void AssertNoChildOpened()
        {
            if (_isChildOpenned != 0)
            {
                throw new InvalidOperationException("There is at least one block opened");
            }
        }

        public void Dispose()
        {
            if (_isChildOpenned != 0)
            {
                throw new InvalidOperationException("Some of child block writers were not disposed");
            }
        }

        public TCloseBlock OpenBlock(string blockName)
        {
            if (blockName == null) throw new ArgumentNullException(nameof(blockName));
            AssertNoChildOpened();
            PostMessage(new ServiceMessage("blockOpened") {{"name", blockName}});
            _isChildOpenned++;
            return _closeBlock(new DisposableDelegate(() => CloseBlock(blockName)));
        }

        private void CloseBlock(string blockName)
        {
            if (blockName == null) throw new ArgumentNullException(nameof(blockName));
            _isChildOpenned--;
            PostMessage(new ServiceMessage("blockClosed") {{"name", blockName}});
        }
    }

    public class TeamCityBlockWriter : TeamCityBlockWriter<IDisposable>
    {
        public TeamCityBlockWriter(IServiceMessageProcessor target)
            : base(target, x => x)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }
    }
}