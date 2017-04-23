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
        /// <param name="Document">The HTML document to which the node is to be added.</param>
        /// <returns></returns>
        public HtmlNode ToHtml(HtmlDocument Document)
        {
            // First, construct:
            //
            //     * a sorted dictionary that maps report names to report results, and
            //     * a sorted set of all suite name/case name pairs.
            var allResults = new SortedDictionary<string, Dictionary<Tuple<string, string>, TestCase>>();
            var allSuiteAndCaseNames = new SortedSet<Tuple<string, string>>();
            foreach (var item in Reports)
            {
                var reportResults = GetResults(item);
                allResults.Add(item.Name, reportResults);
                allSuiteAndCaseNames.UnionWith(reportResults.Keys);
            }

            var table = Document.CreateElement("table");
            table.Attributes.Add("class", "resultTable");
            var headerRow = Document.CreateElement("tr");
            headerRow.AppendChild(Document.CreateElement("th"));
            foreach (var resultPair in allResults)
            {
                var headerCell = Document.CreateElement("th");
                headerCell.InnerHtml = resultPair.Key;
                headerRow.AppendChild(headerCell);
            }
            table.AppendChild(headerRow);

            string suiteName = "";
            foreach (var namePair in allSuiteAndCaseNames)
            {
                // Create a new row whenever the suite name changes.
                if (suiteName != namePair.Item1)
                {
                    suiteName = namePair.Item1;
                    var suiteNameRow = Document.CreateElement("tr");
                    var suiteNameCell = Document.CreateElement("td");
                    suiteNameCell.Attributes.Add("class", "suiteName");
                    suiteNameCell.InnerHtml = suiteName;
                    suiteNameRow.AppendChild(suiteNameCell);
                    table.AppendChild(suiteNameRow);
                }
                
                var resultRow = Document.CreateElement("tr");
                var caseNameCell = Document.CreateElement("td");
                caseNameCell.Attributes.Add("class", "testName");
                caseNameCell.InnerHtml = namePair.Item2;
                resultRow.AppendChild(caseNameCell);
                foreach (var resultPair in allResults)
                {
                    var resultCell = Document.CreateElement("td");
                    TestCase resultCase;
                    if (resultPair.Value.TryGetValue(namePair, out resultCase))
                    {
                        if (resultCase.HasFailed)
                        {
                            resultCell.Attributes.Add("class", "failedTest");
                            resultCell.InnerHtml = "&#10007; Failed";
                        }
                        else
                        {
                            resultCell.Attributes.Add("class", "successfulTest");
                            resultCell.InnerHtml = "&#10003; Success";
                        }
                        resultCell.InnerHtml += " <i class=\"testDuration\">(" + resultCase.Duration.TotalMilliseconds + "ms)</i>";
                    }
                    else
                    {
                        resultCell.Attributes.Add("class", "naTest");
                        resultCell.InnerHtml = "N/A";
                    }
                    resultRow.AppendChild(resultCell);
                }
                table.AppendChild(resultRow);
            }

            return table;
        }

        private static Dictionary<Tuple<string, string>, TestCase> GetResults(TestReport Report)
        {
            var results = new Dictionary<Tuple<string, string>, TestCase>();
            foreach (var testSuite in Report.TestSuites)
            {
                foreach (var testCase in testSuite.TestCases)
                {
                    var key = new Tuple<string, string>(testSuite.Name, testCase.Name);
                    results.Add(key, testCase);
                }
            }
            return results;
        }
    }
}