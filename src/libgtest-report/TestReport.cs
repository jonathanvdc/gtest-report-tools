using System;
using System.Collections.Generic;
using System.Linq;

namespace GTest.Report
{
    /// <summary>
    /// Describes a test report: a collection of test suites.
    /// </summary>
    public sealed class TestReport
    {
        /// <summary>
        /// Creates a gtest test report from the given name and sequence of test suites.
        /// </summary>
        /// <param name="Name">The test report's name.</param>
        /// <param name="TestSuites">The sequence of test suites in this run.</param>
        public TestReport(string Name, IEnumerable<TestSuite> TestSuites)
        {
            this.Name = Name;
            this.TestSuites = TestSuites;
        }

        /// <summary>
        /// Gets this test report's name.
        /// </summary>
        /// <returns>This test report's name.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the sequence of test suites in this run.
        /// </summary>
        /// <returns>The sequence of test suites in this run.</returns>
        public IEnumerable<TestSuite> TestSuites { get; private set; }
    }
}