using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GTest.Report
{
    /// <summary>
    /// A collection of methods that parse gtest reports.
    /// </summary>
    public static class ReportParser
    {
        /// <summary>
        /// Parses the given gtest XML report document.
        /// </summary>
        /// <param name="Document">The XML document to parse.</param>
        /// <returns></returns>
        public static TestReport ParseReport(XmlDocument Document)
        {
            foreach (XmlNode child in Document)
            {
                if (child is XmlElement)
                {
                    return ParseReport(child);
                }
            }
            throw new Exception("XML document didn't contain any children.");
        }

        /// <summary>
        /// Parses the given gtest XML report node.
        /// </summary>
        /// <param name="ReportNode">The XML node to parse.</param>
        /// <returns></returns>
        public static TestReport ParseReport(XmlNode ReportNode)
        {
            XmlNode nameAttribute = ReportNode.Attributes["name"];
            var suites = new List<TestSuite>();
            foreach (XmlNode child in ReportNode)
            {
                suites.Add(ParseSuite(child));
            }
            return new TestReport(nameAttribute.InnerText, suites);
        }

        private static TestSuite ParseSuite(XmlNode SuiteNode)
        {
            XmlNode nameAttribute = SuiteNode.Attributes["name"];
            var cases = new List<TestCase>();
            foreach (XmlNode child in SuiteNode)
            {
                cases.Add(ParseCase(child));
            }
            return new TestSuite(nameAttribute.InnerText, cases);
        }

        private static TestCase ParseCase(XmlNode CaseNode)
        {
            XmlNode nameAttribute = CaseNode.Attributes["name"];
            XmlNode timeAttribute = CaseNode.Attributes["time"];
            var failures = new List<string>();
            foreach (XmlNode child in CaseNode)
            {
                XmlNode messageAttribute = child.Attributes["message"];
                failures.Add(messageAttribute.InnerText);
            }
            return new TestCase(
                nameAttribute.InnerText,
                TimeSpan.FromSeconds(double.Parse(timeAttribute.InnerXml)),
                failures);
        }
    }
}