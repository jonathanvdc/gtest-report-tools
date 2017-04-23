using DotArguments;
using DotArguments.Attributes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using static System.Console;
using static System.IO.File;
using static System.IO.Path;

namespace GTest.Report
{
    public class HtmlReportArguments
    {
        public HtmlReportArguments() { }

        [NamedValueArgument("css")]
        public string CssPath { get; set; }

        [RemainingArguments]
        public string[] ReportPaths { get; set; }
    }

    public static class Program
    {
        public static void Main(string[] Args)
        {
            var definition = new ArgumentDefinition(new HtmlReportArguments().GetType());
            var parser = new GNUArgumentParser();

            try
            {
                var arguments = parser.Parse<HtmlReportArguments>(definition, Args);

                string cssCode = ReadAllText(arguments.CssPath);
                var reports = new List<TestReport>();
                foreach (var reportPath in arguments.ReportPaths)
                {
                    reports.Add(ParseReport(reportPath));
                }
                var htmlDoc = CreateComparisonHtml(cssCode, reports);
                htmlDoc.Save(Console.Out);
                Console.WriteLine();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("error: {0}", ex.Message);
                Console.Error.Write("usage: {0}", parser.GenerateUsageString(definition));

                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Parses the report at the given path.
        /// </summary>
        /// <param name="Path">The path at which the report can be found.</param>
        /// <returns></returns>
        private static TestReport ParseReport(string Path)
        {
            var doc = new XmlDocument();
            doc.Load(Path);
            var report = ReportParser.ParseReport(doc);
            return new TestReport(GetFileNameWithoutExtension(Path), report.TestSuites);
        }

        /// <summary>
        /// Creates an HTML document that compares the given list of reports.
        /// </summary>
        /// <param name="Reports"></param>
        /// <returns></returns>
        private static HtmlDocument CreateComparisonHtml(string CssCode, IEnumerable<TestReport> Reports)
        {
            var matrix = new TestReportMatrix(Reports);
            var doc = new HtmlDocument();

            var htmlNode = doc.CreateElement("html");
            doc.DocumentNode.AppendChild(htmlNode);

            var header = doc.CreateElement("head");
            var style = doc.CreateElement("style");
            style.InnerHtml = CssCode;
            header.AppendChild(style);
            htmlNode.AppendChild(header);

            var body = doc.CreateElement("body");
            body.AppendChild(matrix.ToHtml(doc));
            htmlNode.AppendChild(body);

            return doc;
        }
    }
}