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
  /// This interface provides writers for test messages. 
  /// All, but test ignore messages are required to be reported from 
  /// <pre>##teamcity[testStarted name='testname']</pre> and
  /// <pre>##teamcity[testFinished name='testname' duration='&lt;test_duration_in_milliseconds>']</pre>
  /// messages.
  /// 
  /// All tests reportings are done form this method.  
  /// </summary>
  public interface ITeamCityTestsWriter : ITeamCityTestSuiteWriter
  {
    /// <summary>
    /// To start reporting a test, call OpenTest method. To stop reporing test call Dispose on the given object
    /// </summary>
    /// <param name="testName">test name to be reported</param>
    /// <returns>test output/status reporting handle</returns>
    [NotNull]
    ITeamCityTestWriter OpenTest([NotNull] string testName);
  }

  /// <summary>
  /// Sub inteface for created tests writer for some parent test suite.
  /// </summary>
  public interface ITeamCityTestsSubWriter : ITeamCityTestsWriter, IDisposable
  {
  }
}