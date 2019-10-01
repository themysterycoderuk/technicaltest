using DTOs;
using System.Collections.Generic;
using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface IPipelineBreakdownCalc
    {
        IList<PipelineBreakdown> Calculate(IList<Project> projects);
    }
}
