using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TechTest.WebSiteTestHarness.Models;

namespace TechTest.WebSiteTestHarness.Controllers
{
    public class HomeController : controllerBase
    {
        private readonly IHostingEnvironment _env;

        public HomeController(
            IHostingEnvironment env
        )
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Approach")]
        public IActionResult Approach()
        {
            ViewBag.Title = "Approach";
            addHomeToBreadCrumb();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
