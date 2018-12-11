/*
 * Copyright 2007-2019 JetBrains s.r.o.
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

namespace JetBrains.TeamCity.ServiceMessages.Read
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Provides service messages parsing from stream
    /// </summary>
    public interface IServiceMessageParser
    {
        /// <summary>
        /// Lazy parses service messages from string
        /// </summary>
        /// <param name="text">text to parse</param>
        /// <returns>enumerable of service messages</returns>
        [NotNull]
        IEnumerable<IServiceMessage> ParseServiceMessages([NotNull] string text);

        /// <summary>
        /// Reads stream parsing service messages from it.
        /// </summary>
        /// <param name="reader">stream to parse. Stream will not be closed</param>
        /// <returns>Iterator of service messages</returns>
        [NotNull]
        IEnumerable<IServiceMessage> ParseServiceMessages([NotNull] TextReader reader);
    }
}