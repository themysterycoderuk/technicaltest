using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechTest.Interfaces.Business;
using TechTest.WebSiteTestHarness.Models;

namespace WebSiteTestHarness.Controllers
{
    public class ProjectsController : Controller
    {
        private IReporter _reporter;

        public ProjectsController(IReporter reporter)
        {
            _reporter = reporter;
        }

        [HttpGet]
        public IActionResult Index(ProjectsInfoViewModel model)
        {

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetProjectCount(ProjectsInfoViewModel model)
        {
            var noofprojects = await Task.Run(() => _reporter.GetNoOfProjects(model.Filename));
            var getModel = new ProjectsInfoViewModel()
            {
                Filename = model.Filename,
                NoOfProjects = noofprojects
            };

            return RedirectToAction("Index", getModel);
        }
    }
}