using System;
using System.Collections.Generic;
using System.Xml;

namespace GTest.Report
{
    public static class Program
    {
        public static void Main(string[] Args)
        {
            if (Args.Length != 1)
            {
                Console.WriteLine("usage: gtest-report-print gtest-report.xml");
                return;
            }

            // Parse the report.
            var reportFileName = Args[0];
            var doc = new XmlDocument();
            doc.Load(reportFileName);
            var report = ReportParser.ParseReport(doc);

            foreach (var suite in report.TestSuites)
            {
                Console.WriteLine(" * " + suite.Name);
                foreach (var testCase in suite.TestCases)
                {
                    Console.Write("   ");
                    Console.Write("✓");
                    Console.Write(" ");
                    Console.Write(testCase.Name);
                    Console.WriteLine();
                }
            }
        }
    }
}