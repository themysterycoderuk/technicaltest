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

        public Reporter(
            IJSONLoader loader,
            INoOfSuccessDeploymentsCalc noofsuccessdeploymentscalc,
            ISuccessDeploymentBreakdownCalc successDeploymentBreakdownCalc)
        {
            _loader = loader;
            _noofsuccessdeploymentscalc = noofsuccessdeploymentscalc;
            _successDeploymentBreakdownCalc = successDeploymentBreakdownCalc;
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

            // Compose results into Analysis DTO to be returned
            var results = new AnalysisInfo()
            {
                TotalNoOfSuccessfulDeployments = noofsuccessdeployments,
                SuccessfulDeploymentBreakdown = successbreakdown
            };

            return results;
        }
    }
}
