using DTOs;
using System.Collections.Generic;
using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface IIntegrationToLiveAverageCalc
    {
        IList<IntegrationToLiveBreakdown> Calculate(IList<Project> projects);
    }
}
