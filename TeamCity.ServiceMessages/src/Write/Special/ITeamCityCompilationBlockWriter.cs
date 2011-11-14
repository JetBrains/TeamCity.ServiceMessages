using System;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special
{
  /// <summary>
  /// Introduces compilation block.
  /// <pre>##teamcity[compilationStarted compiler='&lt;compiler name>']</pre>
  /// and
  /// <pre>##teamcity[compilationFinished compiler='&lt;compiler name>']</pre>
  /// http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ReportingCompilationMessages
  /// </summary>
  public interface ITeamCityCompilationBlockWriter
  {
    /// <summary>
    /// Generates open compilation block. To close block call Dispose to the given handle
    /// </summary>
    /// <param name="compilerName"></param>
    /// <returns></returns>
    IDisposable OpenCompilationBlock([NotNull] string compilerName);
  }
}