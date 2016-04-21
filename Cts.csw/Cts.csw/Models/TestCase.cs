using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw
{
    public class TestCase
    {
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public string ActualOutput { get; set; }


        public static List<TestCase> GetTestCases()
        {
            List<TestCase> TestCases = new List<TestCase>();

            //TODO read these in from a database table
            TestCases.Add(new TestCase() { Input = "3\n6", ExpectedOutput = "6\nTRUE" });
            TestCases.Add(new TestCase() { Input = "SomeText\nIrrelevant", ExpectedOutput = "ERROR" });

            return TestCases;
        }
    }
}