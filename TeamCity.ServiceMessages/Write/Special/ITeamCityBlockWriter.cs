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
    ///     Generates pair of service messages to open/close block, for example:
    ///     <pre>##teamcity[blockOpened name='&lt;blockName>']</pre>
    ///     and
    ///     <pre>##teamcity[blockClosed name='&lt;blockName>']</pre>
    ///     http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-BlocksofServiceMessages
    /// </summary>
    /// <remarks>
    ///     Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityBlockWriter<out CloseBlock>
        where CloseBlock : IDisposable
    {
        /// <summary>
        ///     Generates open block message. To close the block, call Dispose to the given handle
        /// </summary>
        /// <param name="blockName">block name to report</param>
        /// <returns></returns>
        [NotNull]
        CloseBlock OpenBlock([NotNull] string blockName);
    }
}