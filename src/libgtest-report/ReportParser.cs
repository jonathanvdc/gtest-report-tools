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
            return ParseReport((XmlElement)Document.FirstChild);
        }

        /// <summary>
        /// Parses the given gtest XML report node.
        /// </summary>
        /// <param name="ReportNode">The XML node to parse.</param>
        /// <returns></returns>
        public static TestReport ParseReport(XmlElement ReportNode)
        {
            XmlNode nameAttribute = ReportNode.GetAttributeNode("name");
            var suites = new List<TestSuite>();
            foreach (XmlElement child in ReportNode)
            {
                suites.Add(ParseSuite(child));
            }
            return new TestReport(nameAttribute.InnerText, suites);
        }

        private static TestSuite ParseSuite(XmlElement SuiteNode)
        {
            XmlNode nameAttribute = SuiteNode.GetAttributeNode("name");
            var cases = new List<TestCase>();
            foreach (XmlElement child in SuiteNode)
            {
                cases.Add(ParseCase(child));
            }
            return new TestSuite(nameAttribute.InnerText, cases);
        }

        private static TestCase ParseCase(XmlElement CaseNode)
        {
            XmlNode nameAttribute = CaseNode.GetAttributeNode("name");
            var failures = new List<string>();
            foreach (XmlElement child in CaseNode)
            {
                XmlNode messageAttribute = CaseNode.GetAttributeNode("message");
                failures.Add(messageAttribute.InnerText);
            }
            return new TestCase(nameAttribute.InnerText, failures);
        }
    }
}