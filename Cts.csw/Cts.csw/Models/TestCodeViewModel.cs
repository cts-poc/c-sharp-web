using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw.Models
{
    public class TestCodeViewModel
    {
        public string TheCode { get; set; }
        public List<string> Inputs { get; set; }
        public List<string> Results { get; set; }

        public TestCodeViewModel()
        {
            Inputs = new List<string>();
            Results = new List<string>();
        }
    }
}