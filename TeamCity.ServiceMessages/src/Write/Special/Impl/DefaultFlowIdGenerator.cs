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
using System.Globalization;
using System.Threading;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  /// <summary>
  /// Helper class to generate FlowIds
  /// </summary>  
  public class DefaultFlowIdGenerator : IFlowIdGenerator
  {
    private int myIds;
    /// <summary>
    /// Generates new unique FlowId
    /// </summary>
    public string NewFlowId()
    {
      return
        (
          Interlocked.Increment(ref myIds) << 27
          +
          (Thread.CurrentThread.ManagedThreadId << 21)
          +
          (Environment.TickCount%int.MaxValue)
        )
          .ToString(CultureInfo.InvariantCulture);
    }
  }
}