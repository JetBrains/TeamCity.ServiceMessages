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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater
{
  /// <summary>
  /// Service message updater that adds FlowId to service message according to
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity
  /// </summary>
  public class FlowMessageUpdater : IServiceMessageUpdater
  {
    private readonly string myFlowId;

    /// <summary>
    /// Custructs updater
    /// </summary>
    /// <param name="flowId">flowId set to all messages</param>
    public FlowMessageUpdater(string flowId)
    {
      myFlowId = flowId;
    }

    /// <summary>
    /// Generates random flowId
    /// </summary>
    public FlowMessageUpdater() : this((DateTime.Now.Ticks % int.MaxValue).ToString())
    {
    }

    public IServiceMessage UpdateServiceMessage(IServiceMessage message)
    {
      if (message.DefaultValue != null) return message;
      return new PatchedServiceMessage(message){{"flowId", myFlowId}};
    }
  }  
}