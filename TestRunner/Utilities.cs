using NUnit.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TechTest.TestRunner
{
    public class Utilities : IUtilities, ITestEventListener
    {
        
        private string _assemblyPath = "./bin/Debug/netcoreapp2.2/UnitTests.dll";

        public Utilities(bool isProduction)
        {
            if (isProduction)
            {
                _assemblyPath = "./UnitTests.dll";
            }
        }

        public IDictionary<string, IList<string>> FindTests()
        {
            var testStructure = new Dictionary<string, IList<string>>();
            var runner = getRunner();
            var builder = new TestFilterBuilder();
            var filter = builder.GetFilter();
            var explore = runner.Explore(filter);
            var testFileNodes = explore.SelectNodes(@"//test-suite/test-suite/test-suite");

            foreach (var testFileNode in testFileNodes.Cast<XmlNode>())
            {
                var fileName = testFileNode.Attributes["name"].Value;
                var tests = testFileNode
                    .SelectNodes(@"./test-case")
                    .Cast<XmlNode>()
                    .Select(s => s.Attributes["name"].Value)
                    .ToList();

                testStructure.Add(fileName, tests);
            }

            return testStructure;
        }

        public string RunTest(string testName)
        {
            var runner = getRunner();
            var builder = new TestFilterBuilder();
            builder.SelectWhere($"method==\"{testName}\"");
            var filter = builder.GetFilter();
            var results = runner.Run(this, filter);
            var testNodes = results.SelectNodes(@"//test-case");
            return testNodes.Cast<XmlNode>().Select(s => s.Attributes["result"].Value).First();
        }

        public void OnTestEvent(string report)
        {
            var x = report;
        }

        private ITestRunner getRunner()
        {
            var engine = TestEngineActivator.CreateInstance();
            var package = new TestPackage(_assemblyPath);
            return engine.GetRunner(package);
        }
    }
}
