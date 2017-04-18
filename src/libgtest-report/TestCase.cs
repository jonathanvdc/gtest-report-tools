using System;
using System.Collections.Generic;
using System.Linq;

namespace GTest.Report
{
    /// <summary>
    /// Describes a gtest test case.
    /// </summary>
    public sealed class TestCase
    {
        /// <summary>
        /// Creates a gtest test case from the given name and sequence of failures.
        /// </summary>
        /// <param name="Name">The test case's name.</param>
        /// <param name="Failures">The test case's sequence of failures.</param>
        public TestCase(string Name, IEnumerable<string> Failures)
        {
            this.Name = Name;
            this.Failures = Failures;
        }

        /// <summary>
        /// Gets this test case's name.
        /// </summary>
        /// <returns>This test case's name.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the sequence of failures in this test case.
        /// </summary>
        /// <returns>The sequence of failures for this test case.</returns>
        public IEnumerable<string> Failures { get; private set; }

        /// <summary>
        /// Gets a Boolean value that tells if this test case contains any failures.
        /// </summary>
        /// <returns>A Boolean value that tells if this test case contains any failures.</returns>
        public bool HasFailed => Enumerable.Any<string>(Failures);
    }
}