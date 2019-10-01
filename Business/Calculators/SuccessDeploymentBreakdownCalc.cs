using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business.Calculators
{
    public class SuccessDeploymentBreakdownCalc : ISuccessDeploymentBreakdownCalc
    {
        /// <summary>
        /// Calculate breakdown of no of successful deployments by project 
        /// group, environment and year
        /// </summary>
        /// <param name="projects">The projects to calculate breakdown from</param>
        /// <returns>Collection of succesful deployment breakdown results</returns>
        public IList<DeploymentBreakdown> Calculate(IList<Project> projects)
        {
            if (projects == null)
            {
                throw new ApplicationException("Cannot supply a null projects collection");
            }

            // Get rid of projects without any succesful deployments
            return projects.Where(
                p => p.releases.Any(
                    r => r.deployments.Any(
                        d => d.state == "Success"
                    )
                )
            )
            
            // Only take those deployements whish were successful and
            // flatten structure ready for grouping
            .SelectMany(
                p =>
                p.releases.SelectMany(
                    r =>
                    r.deployments.Where(
                        d => d.state == "Success"
                    ).Select(
                        s => new
                        {
                            p.project_group,
                            s.environment,
                            s.created.Year
                        }
                    )
                )
            )

            // Perform the group by and get the counts
            .GroupBy(g => new { g.project_group, g.environment, g.Year })
            .Select(s => new DeploymentBreakdown()
            {
                ProjectGroup = s.Key.project_group,
                Environment = s.Key.environment,
                Year = s.Key.Year,
                NoOfSuccessDeployments = s.Count()
            })
            
            // Materialise to list
            .ToList();
        }
    }
}
