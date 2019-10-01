using System.Collections.Generic;
using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface INoOfSuccessDeploymentsCalc
    {
        int Calculate(IList<Project> projects);
    }
}
