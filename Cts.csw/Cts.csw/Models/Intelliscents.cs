using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cts.csw.Models
{
    // Hmmmmm, smells vaguely of Intellisense...
    public class Intelliscents
    {
        public static List<String> ShowMethods(Type type)
        {
            //List<String> output = new List<String>();

            var output = type.GetMethods().OrderBy(method => method.Name).GroupBy(method => method.Name).Select(group => group.First().Name).ToList();

            //foreach (var method in type.GetMethods().GroupBy(method => method.Name).Select(group => group.First()))
            //{
            //    var parameters = method.GetParameters();
            //    var parameterDescriptions = string.Join
            //        (", ", method.GetParameters()
            //                     .Select(x => x.ParameterType + " " + x.Name)
            //                     .ToArray());

            //    output.Add(String.Format("{0}", method.Name));
            //}

            return output;
        }
    }
}