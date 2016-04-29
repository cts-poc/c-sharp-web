using Cts.csw.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Cts.csw
{
    public class CSharpRunner
    {

        public TestCodeViewModel RunCSharp(TestCodeViewModel vm)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(vm.TheCode);

            // find out where all those sweet .dlls live
            string assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            // references to add to the assembly
            MetadataReference[] references = new MetadataReference[]
            {
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
                        string sPattern = @"\((\d+),";
                        var lineNumber = int.Parse((Regex.Match(diagnostic.ToString(), sPattern)).Groups[1].Value);
                        CodeError error = new CodeError(lineNumber, diagnostic.Id, diagnostic.GetMessage());
                        vm.AddError(error);

                        vm.ErrorMessage += "Line: " + error.LineNumber + "\nID: " + error.ID + "\nMessage: " + error.Message + "\n\n";
                    }
                    return vm;
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
                    for (int testCaseNumber = 0; testCaseNumber < vm.TestCases.Count; testCaseNumber++)
                    {
                        MemoryStream mem = new MemoryStream(1000);
                        StreamWriter writer = new StreamWriter(mem);
                        Console.SetOut(writer);

                        TextReader r = new StringReader(vm.TestCases[testCaseNumber].Input);
                        Console.SetIn(r);

                        //TODO: verify that the method exists before invoking it, and throw appropriate error if it's missing
                        // sample code here: http://stackoverflow.com/questions/14479074/c-sharp-reflection-load-assembly-and-invoke-a-method-if-it-exists
                        type.InvokeMember("Run"
                            , BindingFlags.Default | BindingFlags.InvokeMethod
                            , null
                            , obj
                            , null);

                        r.Close();
                        writer.Close();
                        string s = Encoding.Default.GetString(mem.ToArray());
                        mem.Close();
                        vm.TestCases[testCaseNumber].ActualOutput = s;
                    }
                    Console.SetOut(StandardOut);
                    Console.SetIn(StandardIn);
                    return vm;
                }
            }
        }
    }
}