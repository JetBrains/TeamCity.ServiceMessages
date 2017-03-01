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

    public class TeamCityArtifactsWriter : BaseWriter, ITeamCityArtifactsWriter
    {
        public TeamCityArtifactsWriter(IServiceMessageProcessor target)
            : base(target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
        }

        public void PublishArtifact(string rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            PostMessage(new ValueServiceMessage("publishArtifacts", rules));
        }
    }
}