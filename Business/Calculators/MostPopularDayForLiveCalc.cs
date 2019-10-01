using System;
using System.Collections.Generic;
using System.Linq;
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

            // Flatten structure and just get created 
            // date for live deployments
            return projects.SelectMany(
                p =>
                p.releases.SelectMany(
                    r =>
                    r.deployments
                )
                .Where(
                    d =>
                    d.environment == "Live"
                )
                .Select(s => s.created.DayOfWeek)
            )

            // Group by the day of the week
            .GroupBy(g => g)

            // Apply ordering to get most popular day or first day if a tie
            .OrderByDescending(o => o.Count())
            .ThenBy(t => t.Key)

            // Select the day of week making sure to allow for nulls if
            // no values, otherwise will automatically cast as Sunday
            .Select(f => (Nullable<DayOfWeek>) f.Key)
            .FirstOrDefault();
        }
    }
}
