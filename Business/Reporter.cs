using TechTest.Interfaces.Data;
using TechTest.Interfaces.Business;
using System.Threading.Tasks;
using DTOs;
using techTest.Interfaces.Business.Calculators;

namespace TechTest.Business
{
    public class Reporter : IReporter
    {
        private IJSONLoader _loader;
        private INoOfSuccessDeploymentsCalc _noofsuccessdeploymentscalc;
        private ISuccessDeploymentBreakdownCalc _successDeploymentBreakdownCalc;
        private IMostPopularDayForLiveCalc _mostpopulardayforlivecalc;

        public Reporter(
            IJSONLoader loader,
            INoOfSuccessDeploymentsCalc noofsuccessdeploymentscalc,
            ISuccessDeploymentBreakdownCalc successdeploymentbreakdowncalc,
            IMostPopularDayForLiveCalc mostpopulardayforlivecalc)
        {
            _loader = loader;
            _noofsuccessdeploymentscalc = noofsuccessdeploymentscalc;
            _successDeploymentBreakdownCalc = successdeploymentbreakdowncalc;
            _mostpopulardayforlivecalc = mostpopulardayforlivecalc;
        }

        public AnalysisInfo AnalyseDataset(string filename)
        {
            // Serialise JSON file to objects
            var projects = _loader.LoadFromFile(filename);

            // Call calculator classes to get metrics
            var noofsuccessdeployments = 
                _noofsuccessdeploymentscalc.Calculate(projects.projects);

            var successbreakdown =
                _successDeploymentBreakdownCalc.Calculate(projects.projects);

            var mostpopularliveday =
                _mostpopulardayforlivecalc.Calculate(projects.projects);

            // Compose results into Analysis DTO to be returned
            var results = new AnalysisInfo()
            {
                TotalNoOfSuccessfulDeployments = noofsuccessdeployments,
                SuccessfulDeploymentBreakdown = successbreakdown,
                MostPopularLiveDeploymentWeekday = mostpopularliveday
            };

            return results;
        }
    }
}
