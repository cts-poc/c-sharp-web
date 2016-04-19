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
            vm.TheCode = @"
using System;

    namespace Cts
    {
        public class Writer
        {
            public void Write(string message)
            {
                Console.Write(message);
                Console.WriteLine("" has length "" + message.Length);
            }
        }
    }";
            return View(vm);
        }

        [HttpPost]
        public ActionResult TestCode(TestCodeViewModel vm)
        {
            CSharpRunner runner = new CSharpRunner();
            string[] input = new string[] { "Test CTS", "Hello World", "Output 3" };

            vm.Results = new List<string>(runner.RunCSharp(vm.TheCode, input));

            return View(vm);
        }
    }
}