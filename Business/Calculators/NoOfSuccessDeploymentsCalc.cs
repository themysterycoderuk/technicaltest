using System;
using System.Collections.Generic;
using System.Linq;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business
{
    public class NoOfSuccessDeploymentsCalc : INoOfSuccessDeploymentsCalc
    {
        public int Calculate(IList<Project> projects)
        {
            if (projects == null)
            {
                throw new ApplicationException("Cannot supply a null projects collection");
            }

            // Try and do it all in one LINQ statement to avoid unecessary
            // materialisation of extra objects
            return projects.Sum(
                s =>
                s.releases.Sum(
                    r =>
                    r.deployments.Where(
                        d =>
                        d.state == "Success")
                    .Count()
                )
            );
        }
    }
}
