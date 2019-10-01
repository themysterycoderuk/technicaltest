using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business.Calculators
{
    public class IntegrationToLiveAverageCalc : IIntegrationToLiveAverageCalc
    {
        public IList<IntegrationToLiveBreakdown> Calculate(IList<Project> projects)
        {
            if (projects == null)
            {
                throw new ApplicationException("Cannot supply a null projects collection");
            }

            // Only include projects with a successful integration and live release
            return projects.Where(
                p =>
                p.releases.Any(
                    r =>
                    r.deployments.Any(
                        d =>
                        d.environment == "Integration"
                    ) &&
                    r.deployments.Any(
                        d =>
                        d.environment == "Live"
                    )
                )
            )

            // From those projects only use releases with an 
            // integration and live deployment and from them 
            // select the created date/time first intergration and 
            // first live deployments

            .Select(p => new
            {
                projectgroup = p.project_group,
                ticks = p.releases.Where(
                    r =>
                    r.deployments.Any(
                        d =>
                        d.environment == "Integration"
                    ) &&
                    r.deployments.Any(
                        d =>
                        d.environment == "Live"
                    )
                )
                .Select(s => new
                {
                    integration_time = s.deployments.Where(w => w.environment == "Integration").OrderBy(o => o.created).First().created,
                    live_time = s.deployments.Where(w => w.environment == "Live").OrderBy(o => o.created).First().created
                })
                .Select(s => s.live_time.Subtract(s.integration_time).Ticks) 
            })

            // Group by project group and get average times

            .GroupBy(g => g.projectgroup)
            .Select(s => new IntegrationToLiveBreakdown()
            {
                ProjectGroup = s.Key,
                AverageTime = new TimeSpan(Convert.ToInt64(s.SelectMany(a => a.ticks).Average()))
            })
            .ToList();
        }
    }
}
