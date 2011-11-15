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
using JetBrains.TeamCity.ServiceMessages.Annotations;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Basic implementation of TeamCity service message generation facade
  /// </summary>
  public class TeamCityServiceMessages : ITeamCityServiceMessages
  {
    private readonly IServiceMessageFormatter myFormatter;
    private readonly List<IServiceMessageUpdater> myUpdaters;

    public TeamCityServiceMessages()
      : this(new ServiceMessageFormatter(), new IServiceMessageUpdater[] { new FlowMessageUpdater(), new TimestampUpdater(() => DateTime.Now) })
    {
    }

    public TeamCityServiceMessages([NotNull] IServiceMessageFormatter formatter, [NotNull] IEnumerable<IServiceMessageUpdater> updaters)
    {
      myFormatter = formatter;
      myUpdaters = updaters.ToList();
    }

    public void AddServiceMessageUpdater(IServiceMessageUpdater updater)
    {
      myUpdaters.Add(updater);
    }


    public ITeamCityWriter CreateWriter()
    {
      return CreateWriter(Console.Out.WriteLine);
    }

    public ITeamCityWriter CreateWriter(Action<string> destination)
    {
      IServiceMessageProcessor processor = new SpecializedServiceMessagesWriter(myFormatter, myUpdaters.ToList(), destination);

      return new TeamCityWriterImpl(
        processor,
        new DisposableDelegate(() => { })
        );
    }
  }
}