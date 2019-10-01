using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business.Calculators
{
    public class PipelineBreakdownCalc : IPipelineBreakdownCalc
    {
        public IList<PipelineBreakdown> Calculate(IList<Project> projects)
        {
            if (projects == null)
            {
                throw new ApplicationException("Cannot supply a null projects collection");
            }

            var x = projects.Select(p => new
            {
                projectgroup = p.project_group,
                successreleases = p.releases.Where(r => r.deployments.Any(d => d.state == "Success" && d.environment == "Live")),
                failedreleases = p.releases.Where(r => !r.deployments.Any(d => d.state == "Success" && d.environment == "Live"))
            });

            var y = x.Select(p => new
            {
                projectgroup = p.projectgroup,
                releaseinfo = p.successreleases.Select(s => new ReleaseInfo()
                {
                    Version = s.version,
                    WasSuccessful = true,
                    NoOfDeployments = s.deployments.Count,
                    RepeatedDeployments = s.deployments.GroupBy(g => g.environment).Any(g => g.Count() > 1)
                })
                .Union(p.failedreleases.Select(s => new ReleaseInfo()
                {
                    Version = s.version,
                    WasSuccessful = false,
                    NoOfDeployments = s.deployments.Count,
                    RepeatedDeployments = s.deployments.GroupBy(g => g.environment).Any(g => g.Count() > 1)
                }))
            });

            var z = y.GroupBy(g => g.projectgroup)
            .Select(s => new PipelineBreakdown()
            {
                ProjectGroup = s.Key,
                Releases = s.SelectMany(i => i.releaseinfo).ToList()
            })
            .ToList();

            return z;
        }
    }
}
