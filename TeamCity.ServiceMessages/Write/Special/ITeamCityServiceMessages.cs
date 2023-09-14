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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    /// Factory interface for specialized service messages generation
    /// Create instance of <see cref="TeamCityServiceMessages" /> to get the implementation of the interface.
    /// </summary>
    public interface ITeamCityServiceMessages
    {
        /// <summary>
        /// Creates a writer that outputs service messages to Console.Out
        /// </summary>
        /// <returns></returns>
        [NotNull]
        ITeamCityWriter CreateWriter();

        /// <summary>
        /// Creates a writer that uses the provided delegate to output service messages
        /// </summary>
        /// <param name="destination">generated service messages processor</param>
        /// <param name="addFlowIdsOnTopLevelMessages">specifies whether messages written without explicitly opening a flow should be marked with a common flow id</param>
        /// <returns></returns>
        [NotNull]
        ITeamCityWriter CreateWriter(Action<string> destination, bool addFlowIdsOnTopLevelMessages = true);

        /// <summary>
        /// Adds user-specific service message updater to the list of service message updaters.
        /// </summary>
        /// <param name="updater">updater instance</param>
        void AddServiceMessageUpdater([NotNull] IServiceMessageUpdater updater);
    }
}
