using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTest.Report
{
    /// <summary>
    /// Describes a test report matrix: a comparison of test reports.
    /// </summary>
    public sealed class TestReportMatrix
    {
        public TestReportMatrix(IEnumerable<TestReport> Reports)
        {
            var reportList = new List<TestReport>();
            foreach (var item in Reports)
            {
                reportList.Add(item.OrderByName());
            }
            this.Reports = reportList;
        }

        /// <summary>
        /// Gets the list of test reports in this matrix.
        /// </summary>
        /// <returns>The list of test reports in this matrix.</returns>
        public IEnumerable<TestReport> Reports { get; private set; }

        /// <summary>
        /// Creates an HTML node from this report matrix.
        /// </summary>
        /// <param name="Document"></param>
        /// <returns></returns>
        public HtmlNode ToHtml(HtmlDocument Document)
        {
            var reportArray = Reports.ToArray<TestReport>();
            var suiteIndices = new int[reportArray.Length];
            var caseIndices = new int[reportArray.Length];

            var table = Document.CreateElement("table");
            return table;
        }
    }
}