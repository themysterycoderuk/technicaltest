using DTOs;

namespace TechTest.Interfaces.Business
{
    public interface IReporter
    {
        AnalysisInfo AnalyseDataset(string filename);
    }
}