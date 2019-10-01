using DTOs;
using System.Collections.Generic;
using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface ISuccessDeploymentBreakdownCalc
    {
        /// <summary>
        /// Calculate breakdown of no of successful deployments by project group,
        /// environment and year
        /// </summary>
        /// <param name="projects">The projects to calculate breakdown from</param>
        /// <returns>Collection of succesful deployment breakdown results</returns>
        IList<DeploymentBreakdown> Calculate(IList<Project> projects);
    }
}
