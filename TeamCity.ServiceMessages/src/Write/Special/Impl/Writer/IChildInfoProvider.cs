using System;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  /// <summary>
  /// Represents information about currently openned child blocks or tests or test suites.
  /// </summary>
  public interface ISubWriter : IDisposable
  {
    /// <summary>
    /// This method performs check if no child blocks are opennd.
    /// It is used to check if it is allowed to use current I*Writer
    /// </summary>
    void AssertNoChildOpened();
  }
}