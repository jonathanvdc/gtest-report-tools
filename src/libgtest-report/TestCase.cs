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
        /// <param name="Duration">The amount of time spent in the test case.</param>
        /// <param name="Failures">The test case's sequence of failures.</param>
        public TestCase(string Name, TimeSpan Duration, IEnumerable<string> Failures)
        {
            this.Name = Name;
            this.Duration = Duration;
            this.Failures = Failures;
        }

        /// <summary>
        /// Gets this test case's name.
        /// </summary>
        /// <returns>This test case's name.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the amount of time spent in this test case.
        /// </summary>
        /// <returns>The amount of time spent in this test case.</returns>
        public TimeSpan Duration { get; private set; }

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

    /// <summary>
    /// Compares test cases by name.
    /// </summary>
    public sealed class TestCaseNameComparer : IEqualityComparer<TestCase>, IComparer<TestCase>
    {
        private TestCaseNameComparer() { }

        public static readonly TestCaseNameComparer Instance = new TestCaseNameComparer();

        public bool Equals(TestCase First, TestCase Second)
        {
            return First.Name == Second.Name;
        }

        public int GetHashCode(TestCase Case)
        {
            return Case.Name.GetHashCode();
        }

        public int Compare(TestCase First, TestCase Second)
        {
            return StringComparer.InvariantCulture.Compare(First.Name, Second.Name);
        }
    }
}