using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw.Models
{
    public class TestCodeViewModel
    {
        public string TheCode { get; set; }
        public List<TestCase> TestCases { get; set; }
        public string ErrorMessage { get; set; }
        public List<CodeError> ErrorList { get; set; }

        public TestCodeViewModel()
        {
            ErrorList = new List<CodeError>();
        }

        public void AddTestCases()
        {
            TestCases = TestCase.GetTestCases();
        }

        public void AddError(CodeError error)
        {
            ErrorList.Add(error);
        }
    }
}