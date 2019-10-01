using TechTest.Interfaces.Data;
using TechTest.Interfaces.Business;
using System.Threading.Tasks;

namespace TechTest.Business
{
    public class Reporter : IReporter
    {
        private IJSONLoader _loader;

        public Reporter(IJSONLoader loader)
        {
            _loader = loader;
        }

        public int GetNoOfProjects(string filename)
        {
            var projects = _loader.LoadFromFile(filename);
            return projects.projects.Count;
        }
    }
}
