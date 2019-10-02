using System.Collections.Generic;

namespace TechTest.TestRunner
{
    public interface IUtilities
    {
        IDictionary<string, IList<string>> FindTests();
        string RunTest(string testName);
    }
}