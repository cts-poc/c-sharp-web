using Cts.csw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cts.csw.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestCode()
        {
            TestCodeViewModel vm = new TestCodeViewModel();
            vm.TheCode = @"using System; 
using System.IO;
//Add any additional libraries you may need

namespace Cts
{
    public class Program
    {
        public void Run()
        {
            Console.WriteLine(""Hello World!"");
            string input = Console.ReadLine();
            Console.WriteLine(""Got this input: "" + input);
            string input2 = Console.ReadLine();
            Console.WriteLine(""Got this input second: "" + input2);
        }
    }
}";
            vm.CurrentWord = "Console";
            return View(vm);
        }

        [HttpPost]
        public ActionResult TestCode(TestCodeViewModel vm)
        {

            //List<String> list = Intelliscents.ShowMethods(vm.CurrentWord);

            //vm.Suggestions = list;

            CSharpRunner runner = new CSharpRunner();

            //TEMP Commented out
            vm.AddTestCases();

            //string[] input = new string[] { "Test CTS", "Hello World", "Output 3" };

            //TEMP Commented Out
            vm = runner.RunCSharp(vm);

            return View(vm);
        }

        public ActionResult UpdateSuggestions(String currentWord)
        {
            //List<String> results = new List<String>();
            //results.Add("cat");
            //results.Add("dog");
            //results.Add("fish");

            List<String> results = Intelliscents.ShowMethods(currentWord);

            SuggestionViewModel vm = new SuggestionViewModel();
            vm.Suggestions = results;
            vm.CurrentWord = currentWord;

            return PartialView("_Suggestions", vm);
        }
    }
}