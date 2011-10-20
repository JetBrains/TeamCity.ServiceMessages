using System.Linq;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages
{
  public static class ServiceMessageReplacements
  {
    /// <summary>
    /// Performs TeamCity-format escaping of a string.
    /// </summary>
    public static string Encode([NotNull] string value)
    {
      return Replacements.Aggregate(value, (val, rep) => val.Replace(rep.From, rep.To));
    }

    /// <summary>
    /// Performs TeamCity-format escaping of a string.
    /// </summary>
    public static string Decode([NotNull] string value)
    {
      return Replacements.Reverse().Aggregate(value, (val, rep) => val.Replace(rep.To, rep.From));
    }

    private static readonly EscapeData[] Replacements = {
                                                          new EscapeData("|", "||"),
                                                          new EscapeData("\'", "|'"),
                                                          new EscapeData("\n", "|n"),
                                                          new EscapeData("\r", "|r"),
                                                          new EscapeData("]", "|]"),
                                                          new EscapeData("\u0085", "|x"), //\u0085 (next line)=>|x
                                                          new EscapeData("\u2028", "|l"),//\u2028 (line separator)=>|l
                                                          new EscapeData("\u2029", "|p"),//\u2028 (line separator)=>|l
                                                        };

    private struct EscapeData
    {
      public readonly string From;
      public readonly string To;

      public EscapeData(string @from, string to)
      {
        From = @from;
        To = to;
      }
    }
  }
}