using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cts.csw.Models
{
    // Hmmmmm, smells vaguely of Intellisense...
    public class Intelliscents
    {
        public static List<String> ShowMethods(String typeString)
        {
            //var types = assembly.GetTypes();
            Type type = null;
            List<String> output = new List<String>();
            string assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            Assembly[] assemblies = new Assembly[]
            {
                Assembly.LoadFrom(Path.Combine(assemblyPath, "mscorlib.dll")),
                //Assembly.LoadFrom(typeof(object).Assembly.Location),
                //Assembly.LoadFrom(typeof(Enumerable).Assembly.Location),
                //Assembly.LoadFrom(typeof(System.Object).Assembly.Location),
                //Assembly.LoadFrom(typeof(System.String).Assembly.Location),
                //Assembly.LoadFrom(Path.Combine(assemblyPath, "mscorlib.dll")),
                Assembly.LoadFrom(Path.Combine(assemblyPath, "System.dll")),
                Assembly.LoadFrom(Path.Combine(assemblyPath, "System.Core.dll")),
                Assembly.LoadFrom(Path.Combine(assemblyPath, "System.IO.dll")),
                Assembly.LoadFrom(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };


            //myType = AppDomain.CurrentDomain.GetAssemblies().Get

            foreach (var assembly in assemblies)
            {
                var typeResults = assembly.GetTypes().Where(t => t.Name == typeString);
                if (typeResults.Count() > 0)
                {
                    type = typeResults.First();
                }
                if (type != null)
                {
                    output = type.GetMethods().OrderBy(method => method.Name).GroupBy(method => method.Name).Select(group => group.First().Name).ToList();
                    break;
                }
            }

            return output;
        }
    }
}