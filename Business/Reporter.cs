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

        public Reporter(
            IJSONLoader loader,
            INoOfSuccessDeploymentsCalc noofsuccessdeploymentscalc)
        {
            _loader = loader;
            _noofsuccessdeploymentscalc = noofsuccessdeploymentscalc;
        }

        public int GetNoOfProjects(string filename)
        {
            var projects = _loader.LoadFromFile(filename);
            return projects.projects.Count;
        }

        public AnalysisInfo AnalyseDataset(string filename)
        {
            // Serialise JSON file to objects
            var projects = _loader.LoadFromFile(filename);

            // Call calculator classes to get metrics
            var noofsuccessdeployments = 
                _noofsuccessdeploymentscalc.Calculate(projects.projects);

            // Compose results into Analysis DTO to be returned
            var results = new AnalysisInfo()
            {
                TotalNoOfSuccessfulDeployments = noofsuccessdeployments
            };

            return results;
        }
    }
}
