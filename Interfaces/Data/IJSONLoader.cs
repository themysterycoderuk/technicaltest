using TechTest.Entities;

namespace TechTest.Interfaces.Data
{
    public interface IJSONLoader
    {
        Projects LoadFromFile(string filename);
    }
}