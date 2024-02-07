

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
    using System;

    /// <summary>
    /// Represents information about currently open child blocks or tests or test suites.
    /// </summary>
    public interface ISubWriter : IDisposable
    {
        /// <summary>
        /// This method checks if no child blocks are open.
        /// It is used to check if it is allowed to use current I*Writer
        /// </summary>
        void AssertNoChildOpened();
    }
}