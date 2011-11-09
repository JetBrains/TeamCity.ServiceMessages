using System;
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
      var sb = new StringBuilder(value.Length);
      var buff = value.ToCharArray();
      int i = 0;
      for (; i < buff.Length-1; i++)
      {
        var ch = buff[i];
        if (ch != '|')
        {
          sb.Append(ch);
          continue;
        }

        ch = buff[++i];
        switch (ch)
        {
          case '|': sb.Append('|'); break;   //
          case '\'': sb.Append('\''); break;  //
          case 'n': sb.Append('\n'); break;  //
          case 'r': sb.Append('\r'); break;  //
          case ']': sb.Append(']'); break;  //
          case 'x': sb.Append('\u0085'); break; //\u0085 (next line)=>|x
          case 'l': sb.Append('\u2028'); break;//\u2028 (line separator)=>|l
          case 'p': sb.Append('\u2029'); break; //
          default:
            throw new ArgumentException("Unexpected escape sequence \"{0}\".", "|" + ch);
        }
      }
      if (i < buff.Length) sb.Append(buff[i]);      
      return sb.ToString();
    }
  }
}