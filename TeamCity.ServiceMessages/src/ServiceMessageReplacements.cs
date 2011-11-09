using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages
{
  public class ServiceMessageReplacements
  {
    /// <summary>
    /// Performs TeamCity-format escaping of a string.
    /// </summary>
    public static string Encode([NotNull] string value)
    {
      var sb = new StringBuilder(value.Length*2);
      foreach (var ch in value)
      {
        switch(ch)
        {
          case '|'      : sb.Append("||"); break;  //
          case '\''     : sb.Append("|'"); break;  //
          case '\n'     : sb.Append("|n"); break;  //
          case '\r'     : sb.Append("|r"); break;  //
          case ']'      : sb.Append("|]"); break;  //
          case '\u0085' : sb.Append("|x"); break;  //\u0085 (next line)=>|x
          case '\u2028' : sb.Append("|l"); break;  //\u2028 (line separator)=>|l
          case '\u2029' : sb.Append("|p"); break;  //
          default:        sb.Append(ch); break;
        }        
      }
      return sb.ToString();
    }

    /// <summary>
    /// Performs TeamCity-format escaping of a string.
    /// </summary>
    public static string Decode([NotNull] string value)
    {
      return Decode(value, value.Length);
    }

    public static string Decode(IList<char> text)
    {
      return Decode(text, text.Count);
    }

    private static string Decode([NotNull] IEnumerable<char> value, int count)
    {
      var sb = new List<char>(count);

      var escape = false;
      foreach (var ch in value)
      {
        if (!escape)
        {
          if (ch == '|') escape = true; else sb.Add(ch);
        } else
        {
          switch (ch)
          {
            case '|': sb.Add('|'); break;   //
            case '\'': sb.Add('\''); break;  //
            case 'n': sb.Add('\n'); break;  //
            case 'r': sb.Add('\r'); break;  //
            case ']': sb.Add(']'); break;  //
            case 'x': sb.Add('\u0085'); break; //\u0085 (next line)=>|x
            case 'l': sb.Add('\u2028'); break;//\u2028 (line separator)=>|l
            case 'p': sb.Add('\u2029'); break; //
            default: sb.Add('?'); break; // do not thow any exception to make it faster //TODO: no exception on illegal format
          }
        }        
      }
      return new string(sb.ToArray());
    }
  }
}