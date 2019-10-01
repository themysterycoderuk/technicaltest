using System;
using System.Collections.Generic;
using techTest.Interfaces.Business.Calculators;
using TechTest.Entities;

namespace TechTest.Business.Calculators
{
    public class MostPopularDayForLiveCalc : IMostPopularDayForLiveCalc
    {
        public DayOfWeek? Calculate(IList<Project> projects)
        {
            if (projects == null)
            {
                throw new ApplicationException("Cannot supply a null projects collection");
            }

            return null;
        }
    }
}
