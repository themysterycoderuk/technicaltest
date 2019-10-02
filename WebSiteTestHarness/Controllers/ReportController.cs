using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TechTest.Interfaces.Business;
using TechTest.WebSiteTestHarness.Extensions;
using TechTest.WebSiteTestHarness.Models;

namespace WebSiteTestHarness.Controllers
{
    public class ReportController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private IReporter _reporter;

        public ReportController(IReporter reporter, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _reporter = reporter;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jsonfile = "projects.json";
            var results = await Task.Run(() => _reporter.AnalyseDataset(jsonfile));

            // Format the output
            var reportfile = $"report-{DateTime.Now.ToString("yyyyMMddTHHmmssfffffff")}";
            string docPath = $"{_hostingEnvironment.WebRootPath}/temp/{reportfile}";
            Console.WriteLine($"Writing file to {docPath}");

            using (var outputFile = new StreamWriter(docPath))
            {
                outputFile.WriteLine($"Report run at {DateTime.Now}");
                outputFile.WriteLine();
                outputFile.WriteLine();

                outputFile.WriteLine("Output 1: Number of successful deployments");
                outputFile.WriteLine("=========");
                outputFile.WriteLine($"Total number of successful deployments = {results.TotalNoOfSuccessfulDeployments}");
                outputFile.WriteLine();

                outputFile.WriteLine("Output 2: Breakdown by project group, environment and year");
                outputFile.WriteLine("=========");

                foreach (var breakdown in results.SuccessfulDeploymentBreakdown
                    .OrderBy(o => o.ProjectGroup)
                    .ThenBy(o => o.Environment)
                    .ThenByDescending(o => o.Year))
                {
                    var line = $"{breakdown.ProjectGroup}   {breakdown.Environment} {breakdown.Year} {breakdown.NoOfSuccessDeployments}";
                    outputFile.WriteLine(line);
                }

                outputFile.WriteLine();

                outputFile.WriteLine("Output 3: most popular day for live deployments");
                outputFile.WriteLine("=========");
                outputFile.WriteLine($"Most popular day for live deployments is {results.MostPopularLiveDeploymentWeekday}");
                outputFile.WriteLine();

                outputFile.WriteLine("Output 4: Average time from integration to live deployment by project group");
                outputFile.WriteLine("=========");
                outputFile.WriteLine("Note: Average time is a timespan i.e. Days.Hours:Minutes:Seconds.Miiliseconds (optional)");

                foreach (var breakdown in results.IntegrationToLiveBreakdowns
                    .OrderBy(o => o.ProjectGroup))
                {
                    var line = $"{breakdown.ProjectGroup}   {breakdown.AverageTime}";
                    outputFile.WriteLine(line);
                }
                outputFile.WriteLine();

                outputFile.WriteLine("Output 5: Breakdown by project group of successful and unsuccesful releases");
                outputFile.WriteLine("=========");
                outputFile.WriteLine("NOTE: There was an ambiguity in question 5 between Deployments and Releases, namely");
                outputFile.WriteLine("<quote>of success and unsuccessful deployments (unsuccessful being releases that ");
                outputFile.WriteLine("aren't deployed to live), the number of deployments ");
                outputFile.WriteLine("involved in the release pipeline</quote>");
                outputFile.WriteLine();
                outputFile.WriteLine("I have assumed the 1st mention of deployments in the above");
                outputFile.WriteLine("quote should actually be releases as otherwise the wording");
                outputFile.WriteLine("of the question doesn't seem to make sense with regard");
                outputFile.WriteLine("to the JSON structure where deployments are a collection");
                outputFile.WriteLine("within a release");
                outputFile.WriteLine();

                foreach (var info in results.PipelineBreakdowns.OrderBy(o => o.ProjectGroup))
                {
                    outputFile.WriteLine($"Project Group: {info.ProjectGroup}");
                    outputFile.WriteLine("=====================================");
                    outputFile.WriteLine();
                    outputFile.WriteLine("Successful Releases: (version, no of deployments, required repeated deployments");
                    outputFile.WriteLine();

                    foreach (var release in info.Releases.
                        Where(r => r.WasSuccessful)
                        .OrderBy(o => o.Version))
                    {
                        var line = $"{release.Version}  {release.NoOfDeployments}   {release.RepeatedDeployments}";
                        outputFile.WriteLine(line);
                    }

                    outputFile.WriteLine();
                    outputFile.WriteLine("Unsuccessful Releases: (version, no of deployments, required repeated deployments");
                    outputFile.WriteLine();

                    foreach (var release in info.Releases.
                        Where(r => !r.WasSuccessful)
                        .OrderBy(o => o.Version))
                    {
                        var line = $"{release.Version}  {release.NoOfDeployments}   {release.RepeatedDeployments}";
                        outputFile.WriteLine(line);
                    }
                }
            }

            Console.WriteLine("Written");
            Console.WriteLine($"Reading", docPath);
            var stream = new FileStream(docPath, FileMode.Open);
            return new FileStreamResult(stream, "text/plain");
        }
    }
}