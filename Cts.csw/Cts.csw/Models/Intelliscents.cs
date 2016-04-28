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
                Assembly.LoadFrom(Path.Combine(assemblyPath, "mscorlib.dll"))
                //MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(System.String).Assembly.Location),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.IO.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };


            //myType = AppDomain.CurrentDomain.GetAssemblies().Get

            foreach (var assembly in assemblies)
            {
                type = assembly.GetTypes().Where(t => t.Name == typeString).First();
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