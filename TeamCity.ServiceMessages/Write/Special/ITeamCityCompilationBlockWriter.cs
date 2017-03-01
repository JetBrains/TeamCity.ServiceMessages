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

    /// <summary>
    ///     Introduces compilation block.
    ///     <pre>##teamcity[compilationStarted compiler='&lt;compiler name>']</pre>
    ///     and
    ///     <pre>##teamcity[compilationFinished compiler='&lt;compiler name>']</pre>
    ///     http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingCompilationMessages
    /// </summary>
    /// <remarks>
    ///     Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityCompilationBlockWriter<out CloseBlock>
        where CloseBlock : IDisposable
    {
        /// <summary>
        ///     Generates open compilation block. To close block call Dispose to the given handle
        /// </summary>
        /// <param name="compilerName"></param>
        /// <returns></returns>
        CloseBlock OpenCompilationBlock([NotNull] string compilerName);
    }

    public interface ITeamCityCompilationBlockWriter : ITeamCityCompilationBlockWriter<IDisposable>
    {
    }
}