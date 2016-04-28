using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw.Models
{
    public class SuggestionViewModel
    {
        public String CurrentWord { get; set; }

        public List<String> Suggestions { get; set; }
    }
}