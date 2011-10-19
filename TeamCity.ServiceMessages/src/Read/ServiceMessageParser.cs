using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  public static class ServiceMessageParser
  {
    /// <summary>
    /// Reads stream parsing service messages from it.
    /// </summary>
    /// <param name="reader">stream to parse. Stream will not be closed</param>
    /// <returns>Iterator of service messages</returns>
    public static IEnumerable<ServiceMessage> ParseServiceMessages([NotNull] TextReader reader)
    {
      while (true)
      {
        int currentSymbol = 0;
        var startWith = ServiceMessageConstants.SERVICE_MESSAGE_OPEN.ToCharArray();

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
        //there was ##teamcity[ parsed
        if (currentSymbol != startWith.Length) yield break;

        var messageName = new StringBuilder();
        while ((symbol = reader.Read()) >= 0 && !char.IsWhiteSpace((char) symbol))
          messageName.Append((char) symbol);

        while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol)) ;
        if (symbol < 0) yield break;

        if (symbol == '\'')
        {
          var buffer = new StringBuilder();
          while ((symbol = reader.Read()) >= 0)
          {
            var ch = (char) symbol;
            if (ch == '|')
            {
              buffer.Append(ch);
              symbol = reader.Read();
              if (symbol < 0) yield break;
              buffer.Append((char)symbol);
            }
            else
            {
              if (ch == '\'') break;
              buffer.Append(ch);
            }
          }

          while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char) symbol)) ;
          if (symbol == ']')
            yield return new ServiceMessage(messageName.ToString(), ServiceMessageReplacements.Decode(buffer.ToString()));
        } else
        {
          var paramz = new Dictionary<string, string>();

          while (true)
          {
            var name = new StringBuilder();
            name.Append((char)symbol);
            while ((symbol = reader.Read()) >= 0 && symbol != '=')
              name.Append((char) symbol);
            
            if (symbol < 0) yield break;
            while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char)symbol)) ;
            if (symbol < 0) yield break;

            if (symbol != '\'')
              break;
            
            var buffer = new StringBuilder();
            while ((symbol = reader.Read()) >= 0)
            {
              var ch = (char) symbol;
              if (ch == '|')
              {
                buffer.Append(ch);
                symbol = reader.Read();
                if (symbol < 0) yield break;
                buffer.Append((char)symbol);
              }
              else
              {
                if (ch == '\'') break;
                buffer.Append(ch);
              }
            }

            paramz[name.ToString().Trim()] = ServiceMessageReplacements.Decode(buffer.ToString());

            while ((symbol = reader.Read()) >= 0 && char.IsWhiteSpace((char)symbol)) ;
            if (symbol == ']')
            {
              yield return new ServiceMessage(messageName.ToString(), null, paramz);
              break;
            }

            if (symbol < 0) yield break;
          }

        }
      }
    }
  }
}