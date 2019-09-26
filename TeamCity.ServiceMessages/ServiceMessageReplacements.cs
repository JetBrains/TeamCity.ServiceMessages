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

namespace JetBrains.TeamCity.ServiceMessages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class ServiceMessageReplacements
    {
        private static char InvalidChar = '?';

        /// <summary>
        /// Performs TeamCity-format escaping of a string.
        /// </summary>
        [NotNull]
        public static string Encode([NotNull] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var sb = new StringBuilder(value.Length * 2);
            foreach (var ch in value)
                switch (ch)
                {
                    case '|':
                        sb.Append("||");
                        break; //
                    case '\'':
                        sb.Append("|'");
                        break; //
                    case '\n':
                        sb.Append("|n");
                        break; //
                    case '\r':
                        sb.Append("|r");
                        break; //
                    case '[':
                        sb.Append("|[");
                        break; //
                    case ']':
                        sb.Append("|]");
                        break; //
                    case '\u0085':
                        sb.Append("|x");
                        break; //\u0085 (next line)=>|x
                    case '\u2028':
                        sb.Append("|l");
                        break; //\u2028 (line separator)=>|l
                    case '\u2029':
                        sb.Append("|p");
                        break; //
                    default:
                        if (ch > 127)
                        {
                            sb.Append($"|0x{(ulong)ch:x4}");
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            return sb.ToString();
        }

        /// <summary>
        /// Performs TeamCity-format escaping of a string.
        /// </summary>
        [NotNull]
        public static string Decode([NotNull] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return Decode(value.ToCharArray());
        }

        public static string Decode([NotNull] char[] value)
        {
            return new string(DecodeChars(value).ToArray());
        }

        private static IEnumerable<char> DecodeChars([NotNull] IEnumerable<char> value)
        {
            var isEscaping = false;
            var unicodeCounter = 0;
            var unicodeSb = new StringBuilder();
            foreach (var ch in value)
            {
                if (unicodeCounter > 0)
                {
                    if (unicodeCounter-- == 5)
                    {
                        if (ch != 'x')
                        {
                            unicodeCounter = 0;
                        }
                    }
                    else
                    {
                        unicodeSb.Append(ch);
                    }
                }

                if (unicodeCounter != 0)
                {
                    continue;
                }

                if (unicodeSb.Length == 4)
                {
                    var unicodeStr = "" + InvalidChar;
                    try
                    {
                        unicodeStr = char.ConvertFromUtf32(int.Parse(unicodeSb.ToString(), NumberStyles.HexNumber));
                    }
                    catch (FormatException)
                    {
                    }

                    unicodeSb.Length = 0;
                    foreach (var c in unicodeStr)
                    {
                        yield return c;
                    }

                    continue;
                }

                if (isEscaping)
                {
                    isEscaping = false;
                    switch (ch)
                    {
                        case '|':
                            yield return '|';
                            break; //
                        case '\'':
                            yield return '\'';
                            break; //
                        case 'n':
                            yield return '\n';
                            break; //
                        case 'r':
                            yield return '\r';
                            break; //
                        case '[':
                            yield return '[';
                            break; //
                        case ']':
                            yield return ']';
                            break; //
                        case 'x':
                            yield return '\u0085';
                            break; //\u0085 (next line)=>|x
                        case 'l':
                            yield return '\u2028';
                            break; //\u2028 (line separator)=>|l
                        case 'p':
                            yield return '\u2029';
                            break; //
                        case '0':
                            unicodeCounter = 5;
                            break;
                        default:
                            yield return InvalidChar;
                            break; // do not throw any exception to make it faster //TODO: no exception on illegal format
                    }

                    continue;
                }

                if (ch == '|')
                {
                    isEscaping = true;
                    continue;
                }

                yield return ch;
            }

            if (isEscaping || unicodeCounter > 0)
            {
                yield return InvalidChar;
            }
        }
    }
}