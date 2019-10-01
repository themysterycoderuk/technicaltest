using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using TechTest.Interfaces.Business;
using TechTest.WebSiteTestHarness.Extensions;
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
        public IActionResult Index(string filename)
        {
            var results = TempData.Get<AnalysisInfo>("RESULTS");

            var model = new ProjectsInfoViewModel()
            {
                Filename = filename,
                Results = results
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetProjectCount(ProjectsInfoViewModel model)
        {
            var results = await Task.Run(() => _reporter.AnalyseDataset(model.Filename));
            TempData.Put<AnalysisInfo>("RESULTS", results);
            return RedirectToAction("Index", new { filename = model.Filename }) ;
        }
    }
}