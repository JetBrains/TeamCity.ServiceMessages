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
  /// Specialized service messages writer facade. Contains all methods for generating different service messages.
  /// 
  /// Do not forget to dispose this interface after you finished using it. 
  /// 
  /// To get the instance of the interface, call <code>new JetBrains.TeamCity.ServiceMessages.Write.Special.TeamCityServiceMessages().CreateWriter</code>
  /// </summary>
  /// <remarks>
  /// Implementation is not thread-safe. Create an instance for each thread instead.
  /// </remarks>
  public interface ITeamCityWriter : ITeamCityBlockWriter<ITeamCityWriter>, ITeamCityMessageWriter, ITeamCityTestsWriter, ITeamCityCompilationBlockWriter<ITeamCityWriter>, ITeamCityArtifactsWriter, ITeamCityBuildStatusWriter, IDisposable
  {
    /// <summary>
    ///  Allows to send bare service message
    /// </summary>
    /// <param name="message">message to send</param>
    void WriteRawMessage([NotNull] IServiceMessage message);
  }

}