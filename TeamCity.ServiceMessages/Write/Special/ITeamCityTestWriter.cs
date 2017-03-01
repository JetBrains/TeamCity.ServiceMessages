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

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
    using System;

    /// <summary>
    ///     Writer interface for generating test information service messages
    /// </summary>
    public interface ITeamCityTestWriter : IDisposable
    {
        /// <summary>
        ///     Attaches test output to the test
        /// </summary>
        /// <param name="text">test output</param>
        void WriteStdOutput([NotNull] string text);

        /// <summary>
        ///     Attaches test error output to the test
        /// </summary>
        /// <param name="text">error output</param>
        void WriteErrOutput([NotNull] string text);

        /// <summary>
        ///     Marks test as ignored
        /// </summary>
        /// <param name="ignoreReason">test ignore reason</param>
        void WriteIgnored([NotNull] string ignoreReason);

        /// <summary>
        ///     Marks test as ignored
        /// </summary>
        void WriteIgnored();

        /// <summary>
        ///     Marks test as failed.
        /// </summary>
        /// <param name="errorMessage">short error message</param>
        /// <param name="errorDetails">detailed error information, i.e. stacktrace</param>
        /// <remarks>
        ///     This method can be called only once.
        /// </remarks>
        void WriteFailed([NotNull] string errorMessage, [NotNull] string errorDetails);

        /// <summary>
        ///     Specifies test duration
        /// </summary>
        /// <remarks>
        ///     TeamCity may compute test duration inself, to provide precise data, you may set the duration explicitly
        /// </remarks>
        /// <param name="duration">time of test</param>
        void WriteDuration(TimeSpan duration);
    }
}