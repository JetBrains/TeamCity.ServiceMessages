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
    using System.Text;

    public class ServiceMessageReplacements
    {
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
                        sb.Append(ch);
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

        [NotNull]
        public static string Decode([NotNull] char[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var i = 0;
            var sb = value;
            var escape = false;
            foreach (var ch in value)
                if (!escape)
                {
                    if (ch == '|') escape = true;
                    else sb[i++] = ch;
                }
                else
                {
                    switch (ch)
                    {
                        case '|':
                            sb[i++] = '|';
                            break; //
                        case '\'':
                            sb[i++] = '\'';
                            break; //
                        case 'n':
                            sb[i++] = '\n';
                            break; //
                        case 'r':
                            sb[i++] = '\r';
                            break; //
                        case '[':
                            sb[i++] = '[';
                            break; //
                        case ']':
                            sb[i++] = ']';
                            break; //
                        case 'x':
                            sb[i++] = '\u0085';
                            break; //\u0085 (next line)=>|x
                        case 'l':
                            sb[i++] = '\u2028';
                            break; //\u2028 (line separator)=>|l
                        case 'p':
                            sb[i++] = '\u2029';
                            break; //
                        default:
                            sb[i++] = '?';
                            break; // do not thow any exception to make it faster //TODO: no exception on illegal format
                    }
                    escape = false;
                }
            return new string(sb, 0, i);
        }
    }
}