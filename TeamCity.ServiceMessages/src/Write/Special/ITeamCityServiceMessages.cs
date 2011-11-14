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
  /// Factory interface for specialized service messages generation
  /// Create instance of <see cref="TeamCityServiceMessages"/> to get the implementation of the interface.
  /// </summary>
  public interface ITeamCityServiceMessages
  {
    /// <summary>
    /// Created writer that generates service messages to a Console.Out
    /// </summary>
    /// <remarks>
    /// Implementation does not support multiple-threads. 
    /// If you need to log more me
    /// </remarks>
    /// <returns></returns>
    [NotNull]
    ITeamCityWriter CreateWriter();

    /// <summary>
    /// Creates writer that translates service messages to the given 
    /// delegate. 
    /// </summary>
    /// <param name="destination">generated service messages processor</param>
    /// <returns></returns>
    [NotNull]
    ITeamCityWriter CreateWriter(Action<string> destination);
  }
}