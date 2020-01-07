using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JJFWebCore.Models;
using WebService;
using Microsoft.AspNetCore.Authorization;

namespace JJFWebCore.Controllers
{
    public class HomeController : Controller
    {
        public TestSevice testSevice { get; set; }
        private readonly ILogger<HomeController> _logger;

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize]
        public string Test()
        {
            System.Threading.Thread.Sleep(1000 * 60);
            return testSevice.Test();
        }
    }
}
