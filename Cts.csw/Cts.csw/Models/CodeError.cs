using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw.Models
{
    public class CodeError
    {
        public int LineNumber { get; set; }
        public String ID { get; set; }
        public String Message { get; set; }

        public CodeError(int lineNumber, String id, String message)
        {
            LineNumber  = lineNumber;
            ID          = id;
            Message     = message;
        }
    }
}