using Microsoft.Extensions.DependencyInjection;
using techTest.Interfaces.Business.Calculators;
using TechTest.Business;
using TechTest.Data;
using TechTest.Interfaces.Business;
using TechTest.Interfaces.Data;

namespace TechTest.Dependencies
{
    /// <summary>
    /// Class for wiring up dependencies, effectively acting as 
    /// a mediator to reduce coupling between the projects
    /// </summary>
    public static class DependencyResolver
    {
        public static void AddBindings(IServiceCollection services)
        {
            services.AddSingleton<IJSONLoader, JSONLoader>();
            services.AddScoped<IReporter, Reporter>();
            services.AddTransient<INoOfSuccessDeploymentsCalc, NoOfSuccessDeploymentsCalc>();
        }
    }
}
