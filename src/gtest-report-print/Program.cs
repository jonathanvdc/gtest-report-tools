using System;
using System.Collections.Generic;
using System.Xml;
using static System.Console;

namespace GTest.Report
{
    public static class Program
    {
        public static void Main(string[] Args)
        {
            string reportFileName = null;
            bool useAnsiColors = false;

            foreach (var arg in Args)
            {
                if (arg == "--ansi")
                {
                    useAnsiColors = true;
                }
                else if (reportFileName != null)
                {
                    WriteLine("usage: gtest-report-print [--ansi] gtest-report.xml");
                    return;
                }
                else
                {
                    reportFileName = arg;
                }
            }

            // Parse the report.
            var doc = new XmlDocument();
            doc.Load(reportFileName);
            var report = ReportParser.ParseReport(doc);

            // And print it.
            var printer = new ReportPrinter(useAnsiColors);
            printer.PrintReport(report);
        }
    }

    public class ReportPrinter
    {
        public ReportPrinter(bool UseAnsiColorCodes)
        {
            this.UseAnsiColorCodes = UseAnsiColorCodes;
        }

        /// <summary>
        /// Gets a Boolean flags that tells if this report printer should use ANSI escape
        /// codes to print colors.
        /// </summary>
        public bool UseAnsiColorCodes { get; private set; }

        /// <summary>
        /// Prints the given test report to standard output.
        /// </summary>
        /// <param name="Report">The test report to print.</param>
        public void PrintReport(TestReport Report)
        {
            int testCount = 0;
            int failureCount = 0;
            foreach (var suite in Report.TestSuites)
            {
                WriteLine(" • " + suite.Name);
                foreach (var testCase in suite.TestCases)
                {
                    testCount++;
                    Write("   ");
                    if (testCase.HasFailed)
                    {
                        failureCount++;
                        WriteColored("✗", ConsoleColor.Red);
                    }
                    else
                    {
                        WriteColored("✓", ConsoleColor.Green);
                    }
                    Write(" ");
                    Write(testCase.Name);
                    Write(" (");
                    Write(testCase.Duration.TotalMilliseconds);
                    Write("ms)");
                    WriteLine();
                }
                WriteLine();
            }

            if (testCount == 0)
            {
                Write("Nothing to print: test report was empty.");
            }
            else if (failureCount == 0)
            {
                Write("All ");
                Write(testCount);
                Write(" test cases were ");
                WriteColored("successful", ConsoleColor.Green);
                Write(".");
            }
            else
            {
                Write("Ran ");
                Write(testCount);
                Write(" test cases, found ");
                WriteColored(failureCount + " failure" + (failureCount == 1 ? "" : "s"), ConsoleColor.Red);
                Write(".");
            }
            WriteLine();
        }

        private void WriteColored(string Text, ConsoleColor Color)
        {
            if (UseAnsiColorCodes)
            {
                var fgColCode = ansiColorCodes[Color] + 30;
                if (ansiBoldColors.Contains(Color))
                {
                    PrintAnsiCode(1, fgColCode);
                }
                else
                {
                    PrintAnsiCode(fgColCode);
                }
                Write(Text);
                // Output the reset code.
                PrintAnsiCode(0);
            }
            else
            {
                var oldColor = ForegroundColor;
                ForegroundColor = Color;
                Write(Text);
                ForegroundColor = oldColor;
            }
        }

        private static void PrintAnsiCode(params int[] Codes)
        {
            Write("\x1b[");
            for (int i = 0; i < Codes.Length; i++)
            {
                if (i > 0)
                {
                    Write(";");
                }
                Write(Codes[i]);
            }
            Write("m");
        }

        private static readonly Dictionary<ConsoleColor, int> ansiColorCodes = new Dictionary<ConsoleColor, int>()
        {
            { ConsoleColor.Black, 0 },
            { ConsoleColor.DarkRed, 1 },
            { ConsoleColor.DarkGreen, 2 },
            { ConsoleColor.DarkYellow, 3 },
            { ConsoleColor.DarkBlue, 4 },
            { ConsoleColor.DarkMagenta, 5 },
            { ConsoleColor.DarkCyan, 6 },
            { ConsoleColor.Gray, 7 },
            { ConsoleColor.DarkGray, 0 },
            { ConsoleColor.Red, 1 },
            { ConsoleColor.Green, 2 },
            { ConsoleColor.Yellow, 3 },
            { ConsoleColor.Blue, 4 },
            { ConsoleColor.Magenta, 5 },
            { ConsoleColor.Cyan, 6 },
            { ConsoleColor.White, 7 }
        };

        private static readonly HashSet<ConsoleColor> ansiBoldColors = new HashSet<ConsoleColor>()
        {
            ConsoleColor.DarkGray, ConsoleColor.Red,
            ConsoleColor.Green, ConsoleColor.Yellow,
            ConsoleColor.Blue, ConsoleColor.Magenta,
            ConsoleColor.Cyan, ConsoleColor.White
        };
    }
}