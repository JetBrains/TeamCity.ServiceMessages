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
    /// <summary>
    /// Service messages for dynamically publish artifacts.
    /// http://confluence.jetbrains.net/display/TCD18/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-PublishingArtifactswhiletheBuildisStillinProgress
    /// </summary>
    /// <remarks>
    /// Implementation is not thread-safe. Create an instance for each thread instead.
    /// </remarks>
    public interface ITeamCityArtifactsWriter
    {
        /// <summary>
        /// attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD18/Build+Artifact
        /// </summary>
        /// <param name="rules"></param>
        void PublishArtifact([NotNull] string rules);
    }
}