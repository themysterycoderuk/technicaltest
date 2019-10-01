using System;
using System.Collections.Generic;
using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface IMostPopularDayForLiveCalc
    {
        /// <summary>
        /// Returns most popular weekday for Live deployments as
        /// standard DayOfWeek integer
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        DayOfWeek? Calculate(IList<Project> projects);
    }
}
