/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System;
using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Basic implementation of TeamCity service message generation facade
  /// </summary>
  public class TeamCityServiceMessages  : ITeamCityServiceMessages
  {
    public ITeamCityWriter CreateWriter()
    {
      return CreateWriter(Console.Out.WriteLine);
    }

    public ITeamCityWriter CreateWriter(Action<string> destination)
    {
      IServiceMessageProcessor processor = new SpecializedServiceMessagesWriter(
        new ServiceMessageFormatter(), 
        new List<IServiceMessageUpdater>
          {
            new FlowMessageUpdater(),
            new TimestampUpdater()
          }, Console.Out.WriteLine);

      return new TeamCityWriterImpl(
        new TeamCityMessageWriter(processor),
        new TeamCityBlockWriter(processor),
        new TeamCityCompilationBlockWriter(processor),
        new TeamCityTestsWriter(processor),
        new DisposableDelegate(() => { })
        );
    }
  }
}