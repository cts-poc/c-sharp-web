using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cts.csw
{
    public class CSharpRunner
    {

        public string[] RunCSharp(string code, string[] input)
        {
            List<string> results = new List<string>();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            // find out where all those sweet .dlls live
            string assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            // references to add to the assembly
            MetadataReference[] references = new MetadataReference[]
            {
                //TODO de-dupe this list
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.String).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.IO.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };

            // set up compiler
            CSharpCompilation csc = CSharpCompilation.Create("Cts"
                , new[] { tree }
                , references
                , new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                // attempt compilation
                EmitResult result = csc.Emit(ms);

                if (!result.Success)
                {
                    //compilation failed
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        results.Add(diagnostic.Id + ": " + diagnostic.GetMessage());
                    }
                    return results.ToArray();
                }
                else
                {
                    //compilation succeeded!
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    TextWriter StandardOut = Console.Out;
                    TextReader StandardIn = Console.In;
                    Type type = assembly.GetType("Cts.Program");
                    object obj = Activator.CreateInstance(type);

                    //iterate over inputs
                    for (int inputNumber = 0; inputNumber < input.Length; inputNumber++)
                    {
                        MemoryStream mem = new MemoryStream(1000);
                        StreamWriter writer = new StreamWriter(mem);
                        Console.SetOut(writer);

                        TextReader r = new StringReader(input[inputNumber] + "\nLine number 2" );
                        Console.SetIn(r);


                        type.InvokeMember("Run"
                            , BindingFlags.Default | BindingFlags.InvokeMethod 
                            , null
                            , obj
                            , null);

                        r.Close();
                        writer.Close();
                        string s = Encoding.Default.GetString(mem.ToArray());
                        mem.Close();
                        results.Add(s);
                    }
                    Console.SetOut(StandardOut);
                    Console.SetIn(StandardIn);
                    return results.ToArray();
                }
            }
        }
    }
}
