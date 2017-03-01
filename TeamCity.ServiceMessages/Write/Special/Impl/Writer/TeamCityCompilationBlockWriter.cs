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

    public class TeamCityCompilationBlockWriter<TCloseBlock> : BaseWriter, ITeamCityCompilationBlockWriter<TCloseBlock>, ISubWriter
        where TCloseBlock : IDisposable
    {
        private readonly Func<IDisposable, TCloseBlock> _closeBlock;
        private int _isChildOpenned;

        public TeamCityCompilationBlockWriter(IServiceMessageProcessor target, Func<IDisposable, TCloseBlock> closeBlock)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (closeBlock == null) throw new ArgumentNullException(nameof(closeBlock));
            _closeBlock = closeBlock;
        }

        public void AssertNoChildOpened()
        {
            if (_isChildOpenned != 0)
                throw new InvalidOperationException("There is at least one compilation child block opened");
        }

        public void Dispose()
        {
            if (_isChildOpenned != 0)
                throw new InvalidOperationException("Some of compilation child block writers were not disposed");
        }

        public TCloseBlock OpenCompilationBlock(string compilerName)
        {
            if (compilerName == null) throw new ArgumentNullException(nameof(compilerName));
            AssertNoChildOpened();
            PostMessage(new ServiceMessage("compilationStarted") {{"compiler", compilerName}});
            _isChildOpenned++;
            return _closeBlock(new DisposableDelegate(() => CloseBlock(compilerName)));
        }

        private void CloseBlock(string compilerName)
        {
            _isChildOpenned--;
            PostMessage(new ServiceMessage("compilationFinished") {{"compiler", compilerName}});
        }
    }

    public class TeamCityCompilationBlockWriter : TeamCityCompilationBlockWriter<IDisposable>, ITeamCityCompilationBlockWriter
    {
        public TeamCityCompilationBlockWriter(IServiceMessageProcessor target)
            : base(target, x => x)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }
    }
}