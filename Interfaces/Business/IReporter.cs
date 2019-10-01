using DTOs;

namespace TechTest.Interfaces.Business
{
    public interface IReporter
    {
        int GetNoOfProjects(string filename);

        AnalysisInfo AnalyseDataset(string filename);
    }
}