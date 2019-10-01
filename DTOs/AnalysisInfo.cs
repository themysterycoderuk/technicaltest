using System;
using System.Collections.Generic;

namespace DTOs
{ 
    /// <summary>
    /// Interface to define the structure needed to return results of 
    /// analysis on the dataset, namely:
    /// 
    /// 1.  How many successful deployments have taken place?
    /// 
    /// 2.  How does this break down by project group, by environment, 
    /// by year?
    /// 
    /// 3.  Which is the most popular day of the week for live deployments?
    /// 
    /// 4.  What is the average length of time a release takes 
    /// from integration to live, by project group?
    /// 
    /// 5.  A break down by project group of success 
    /// and unsuccessful deployments (successful being releases that 
    /// aren't deployed to live), the number of deployments involved 
    /// in the release pipeline and whether some environments had 
    /// to be repeatedly deployed
    /// </summary>
    public class AnalysisInfo
    {
        /// <summary>
        /// Constructor used to set readonly fields.  Please refer
        /// to documentation for properties for more info of inputs
        /// </summary>
        /// <param name="noofsuccessdeployments"><see cref="TotalNoOfSuccessfulDeployments"/></param>
        /// <param name="mostpopularliveday"><see cref="MostPopularLiveDeploymentWeekday"/></param>
        public AnalysisInfo()
        {}

        /// <summary>
        /// Total number of successful deployments
        /// </summary>
        public int TotalNoOfSuccessfulDeployments
        {
            get; set;
        }

        /// <summary>
        /// Break down of successful deployments by project group, 
        /// by environment and year
        /// </summary>
        public IList<DeploymentBreakdown> SuccessfulDeploymentBreakdown
        {
            get; set;
        }

        /// <summary>
        /// The most popular day of the week for live deployments
        /// </summary>
        public DayOfWeek? MostPopularLiveDeploymentWeekday
        {
            get; set;
        }

        /// <summary>
        /// The average length of time a release takes
        /// from integration to live, by project group
        /// </summary>
        public IList<IntegrationToLiveBreakdown> IntegrationToLiveBreakdowns
        {
            get; set;
        }

        /// <summary>
        /// A break down by project group of success 
        /// and unsuccessful releases (unsuccessful being releases that 
        /// aren't sucessfully deployed to live), the number of deployments
        /// involved in the release pipeline and whether some environments
        /// had to be repeatedly deployed
        /// </summary>
        public IList<PipelineBreakdown> PipelineBreakdowns
        {
            get; set;
        }
    }
}
