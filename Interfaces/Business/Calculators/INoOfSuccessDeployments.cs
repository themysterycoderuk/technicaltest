using TechTest.Entities;

namespace techTest.Interfaces.Business.Calculators
{
    public interface INoOfSuccessDeployments
    {
        int Calculate(Projects projects);
    }
}
