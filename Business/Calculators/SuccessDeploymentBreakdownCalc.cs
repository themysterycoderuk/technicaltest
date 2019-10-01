using DTOs;
using System.Collections.Generic;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business.Calculators
{
    public class SuccessDeploymentBreakdownCalc : ISuccessDeploymentBreakdownCalc
    {
        /// <summary>
        /// Calculate breakdown of no of successful deployments by project group,
        /// environment and year
        /// </summary>
        /// <param name="projects">The projects to calculate breakdown from</param>
        /// <returns>Collection of succesful deployment breakdown results</returns>
        public IList<DeploymentBreakdown> Calculate(IList<Project> projects)
        {
            return null;
        }
    }
}
