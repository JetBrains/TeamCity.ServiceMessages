

namespace JetBrains.TeamCity.ServiceMessages.Read
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provides service messages parsing from stream
    /// </summary>
    public class ServiceMessageParser : IServiceMessageParser
    {
        /// <summary>
        /// Lazy parses service messages from string
        /// </summary>
        /// <param name="text">text to parse</param>
        /// <returns>enumerable of service messages</returns>
        public IEnumerable<IServiceMessage> ParseServiceMessages(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            return ParseServiceMessages(new StringReader(text));
        }

        /// <summary>
        /// Reads stream parsing service messages from it.
        /// </summary>
        /// <param name="reader">stream to parse. Stream will not be closed</param>
        /// <returns>Iterator of service messages</returns>
        public IEnumerable<IServiceMessage> ParseServiceMessages(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var startWith = ServiceMessageConstants.ServiceMessageOpen.ToCharArray();
            while (true)
            {
                var currentSymbol = 0;
                int symbol;
                while ((symbol = reader.Read()) >= 0)
                {
                    var c = (char) symbol;
                    if (c != startWith[currentSymbol])
                    {
                        //This was not a service message, let's try again in the next char
                        currentSymbol = 0;
                    }
                    else
                    {
                        currentSymbol++;
                        if (currentSymbol >= startWith.Length) break;
                    }
                }
                if (symbol < 0) yield break;

                //there was ##teamcity[ parsed
                if (currentSymbol != startWith.Length) yield break;

                var messageName = new StringBuilder();
                while ((symbol = reader.Read()) >= 0 && !char.IsWhiteSpace((char) symbol))
                    messageName.Append((char) symbol);
                if (symbol < 0) yield break;

                while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol))
                {
                }

                if (symbol < 0) yield break;

                if (symbol == '\'')
                {
                    var buffer = new List<char>();
                    while ((symbol = reader.Read()) >= 0)
                    {
                        var ch = (char) symbol;
                        if (ch == '|')
                        {
                            buffer.Add(ch);
                            symbol = reader.Read();
                            if (symbol < 0) yield break;
                            buffer.Add((char) symbol);
                        }
                        else
                        {
                            if (ch == '\'') break;
                            buffer.Add(ch);
                        }
                    }
                    if (symbol < 0) yield break;

                    while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol))
                    {
                    }

                    if (symbol < 0) yield break;

                    if (symbol == ']')
                        yield return new ServiceMessage(messageName.ToString(), ServiceMessageReplacements.Decode(buffer.ToArray()));
                }
                else
                {
                    var paramz = new Dictionary<string, string>();

                    while (true)
                    {
                        var name = new StringBuilder();
                        name.Append((char) symbol);

                        while ((symbol = reader.Read()) >= 0 && symbol != '=')
                            name.Append((char) symbol);
                        if (symbol < 0) yield break;

                        while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol))
                        {
                        }

                        if (symbol < 0) yield break;

                        if (symbol != '\'')
                            break;

                        var buffer = new List<char>();
                        while ((symbol = reader.Read()) >= 0)
                        {
                            var ch = (char) symbol;
                            if (ch == '|')
                            {
                                buffer.Add(ch);
                                symbol = reader.Read();
                                if (symbol < 0) yield break;
                                buffer.Add((char) symbol);
                            }
                            else
                            {
                                if (ch == '\'') break;
                                buffer.Add(ch);
                            }
                        }
                        if (symbol < 0) yield break;
                        paramz[name.ToString().Trim()] = ServiceMessageReplacements.Decode(buffer.ToArray());

                        while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol))
                        {
                        }

                        if (symbol < 0) yield break;
                        if (symbol == ']')
                        {
                            yield return new ServiceMessage(messageName.ToString(), null, paramz);
                            break;
                        }
                    }
                }
            }
        }
    }
}