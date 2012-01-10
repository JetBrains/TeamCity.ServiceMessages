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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Servivce message acceptor implementation with support of IServiceMessageUpdater chains
  /// </summary>
  public class SpecializedServiceMessagesWriter : IServiceMessageProcessor
  {
    private readonly IServiceMessageFormatter myFormatter;
    private readonly List<IServiceMessageUpdater> myUpdaters;
    private readonly Action<string> myPrinter;

    /// <summary>
    /// Creates generic processor that calls messages updaters and sends output to provided deledate.
    /// </summary>
    /// <param name="formatter"></param>
    /// <param name="updaters"></param>
    /// <param name="printer"></param>
    public SpecializedServiceMessagesWriter([NotNull] IServiceMessageFormatter formatter, [NotNull] List<IServiceMessageUpdater> updaters, [NotNull] Action<string> printer)
    {
      myFormatter = formatter;
      myUpdaters = updaters;
      myPrinter = printer;
    }

    public void AddServiceMessage(IServiceMessage serviceMessage)
    {
      foreach (var updater in myUpdaters)
        serviceMessage = updater.UpdateServiceMessage(serviceMessage);

      myPrinter(myFormatter.FormatMessage(serviceMessage));
    }
  }
}