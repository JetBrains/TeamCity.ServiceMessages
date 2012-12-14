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
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Starts another flowId reporting starting. This call would emmit
  /// <pre>##teamcity[flowStarted flowId='%lt;new flow id>' parent='current flow id']</pre>
  /// and 
  /// <pre>##teamcity[flowFinished flowId='%lt;new flow id>']</pre>
  /// on writer dispose
  /// </summary>
  /// <remarks>
  /// Implementation is not thread-safe. Create an instance for each thread instead.
  /// </remarks>
  public interface ITeamCityFlowWriter<out CloseBlock>
    where CloseBlock : IDisposable
  {
    /// <summary>
    /// Generates start flow message and returns disposable object to close flow
    /// </summary>
    /// <returns></returns>
    [NotNull]
    CloseBlock OpenFlow();
  }
}