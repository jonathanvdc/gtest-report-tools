using System;
using System.Collections.Generic;
using System.Linq;

namespace GTest.Report
{
    /// <summary>
    /// Describes a test suite: a collection of test cases.
    /// </summary>
    public sealed class TestSuite
    {
        /// <summary>
        /// Creates a gtest test suite from the given name and sequence of test cases.
        /// </summary>
        /// <param name="Name">The test suite's name.</param>
        /// <param name="TestCases">The sequence of test cases in this suite.</param>
        public TestSuite(string Name, IEnumerable<TestCase> TestCases)
        {
            this.Name = Name;
            this.TestCases = TestCases;
        }

        /// <summary>
        /// Gets this test suite's name.
        /// </summary>
        /// <returns>This test suite's name.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the sequence of test cases in this suite.
        /// </summary>
        /// <returns>The sequence of test cases in this suite.</returns>
        public IEnumerable<TestCase> TestCases { get; private set; }

        /// <summary>
        /// Creates an equivalent test suite whose test cases are sorted by name.
        /// </summary>
        /// <returns>An equivalent test suite whose test cases are sorted by name.</returns>
        public TestSuite OrderByName()
        {
            var testCaseList = TestCases.ToList<TestCase>();
            testCaseList.Sort(TestCaseNameComparer.Instance);
            return new TestSuite(Name, testCaseList);
        }
    }

    /// <summary>
    /// Compares test suites by name.
    /// </summary>
    public sealed class TestSuiteNameComparer : IEqualityComparer<TestSuite>, IComparer<TestSuite>
    {
        private TestSuiteNameComparer() { }

        public static readonly TestSuiteNameComparer Instance = new TestSuiteNameComparer();

        public bool Equals(TestSuite First, TestSuite Second)
        {
            return First.Name == Second.Name;
        }

        public int GetHashCode(TestSuite Case)
        {
            return Case.Name.GetHashCode();
        }

        public int Compare(TestSuite First, TestSuite Second)
        {
            return StringComparer.InvariantCulture.Compare(First.Name, Second.Name);
        }
    }
}