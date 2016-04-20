﻿using Microsoft.CodeAnalysis;
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

            string assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.String).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Linq.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Collections.Generic.dll")),
                //MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Text.dll"))
            };

            CSharpCompilation csc = CSharpCompilation.Create("MyAssembly"
                , new[] { tree }
                , references
                , new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = csc.Emit(ms);

                if (!result.Success)
                {
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
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    TextWriter StandardOut = Console.Out;
                    Type type = assembly.GetType("Cts.Writer");
                    object obj = Activator.CreateInstance(type);

                    for (int inputNumber = 0; inputNumber < input.Length; inputNumber++)
                    {
                        MemoryStream mem = new MemoryStream(1000);
                        StreamWriter writer = new StreamWriter(mem);
                        Console.SetOut(writer);

                        type.InvokeMember("Write"
                            , BindingFlags.Default | BindingFlags.InvokeMethod
                            , null
                            , obj
                            , new object[] { input[inputNumber] });

                        writer.Close();
                        string s = Encoding.Default.GetString(mem.ToArray());
                        mem.Close();
                        results.Add(s);
                    }
                    Console.SetOut(StandardOut);
                    return results.ToArray();
                }
            }
        }
    }
}