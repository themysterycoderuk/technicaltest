using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using techTest.Interfaces.Business.Calculators;
using TechTest.Business.Calculators;
using TechTest.Entities;
using TechTest.UnitTests.Helpers;

namespace TechTest.UnitTests.Calculators
{
    public class PipelineBreakdownCalcTests
    {
        private IPipelineBreakdownCalc _calculator;
        private IList<Project> _projects;

        [SetUp]
        public void Setup()
        {
            _calculator = new PipelineBreakdownCalc();
            _projects = new List<Project>();
        }

        #region Test Methods

        [Test]
        public void Calculate_Includes_Correct_Project_Groups()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(_projects, group: "Group1");
            Datasetup.AddProjectWithSingleDeployment(_projects, group: "Group2");

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.IsTrue(result.Any(p => p.ProjectGroup == "Group1"));
            Assert.IsTrue(result.Any(p => p.ProjectGroup == "Group2"));
        }

        [Test]
        public void Calculate_Includes_Correct_Successful_Releases_Across_Project_Groups()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects, group: "Group1");
            var project2 = Datasetup.AddProject(_projects, group: "Group2");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Live", isSuccess: true);
            Datasetup.AddReleaseWithSingleDeployment(project2, environment: "Live", isSuccess: true);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group1").Releases.Count(r => r.WasSuccessful));
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group2").Releases.Count(r => r.WasSuccessful));
        }

        [Test]
        public void Calculate_Includes_Correct_Unsuccessful_Releases_Due_To_Failure()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects, group: "Group1");
            var project2 = Datasetup.AddProject(_projects, group: "Group2");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Live", isSuccess: false);
            Datasetup.AddReleaseWithSingleDeployment(project2, environment: "Live", isSuccess: false);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group1").Releases.Count(r => !r.WasSuccessful));
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group2").Releases.Count(r => !r.WasSuccessful));
        }

        [Test]
        public void Calculate_Includes_Correct_Unsuccessful_Releases_Due_No_Live_Deployment()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects, group: "Group1");
            var project2 = Datasetup.AddProject(_projects, group: "Group2");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Test", isSuccess: true);
            Datasetup.AddReleaseWithSingleDeployment(project2, environment: "UAT", isSuccess: true);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group1").Releases.Count(r => !r.WasSuccessful));
            Assert.AreEqual(1, result.Single(p => p.ProjectGroup == "Group2").Releases.Count(r => !r.WasSuccessful));
        }

        [Test]
        public void Calculate_Includes_Correct_Successful_Releases_WithinGroup()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects, group: "Group1");
            var project2 = Datasetup.AddProject(_projects, group: "Group1");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Test", isSuccess: true);
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Live", version: "success1", isSuccess: true);
            var release2_1 = Datasetup.AddRelease(project2, version: "success2");
            Datasetup.AddDeployment(release2_1, isSuccess: false, environment: "Live");
            Datasetup.AddDeployment(release2_1, isSuccess: true, environment: "Live");

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            var project1Result = result.Single(p => p.ProjectGroup == "Group1");
            Assert.AreEqual(2, project1Result.Releases.Count(r => r.WasSuccessful));
            Assert.IsTrue(project1Result.Releases.Any(r => r.Version == "success1"));
            Assert.IsTrue(project1Result.Releases.Any(r => r.Version == "success1"));
        }

        [Test]
        public void Calculate_Includes_Correct_Unsuccessful_Releases_WithinGroup()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects, group: "Group1");
            var project2 = Datasetup.AddProject(_projects, group: "Group1");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Test", isSuccess: true, version: "nolive");
            Datasetup.AddReleaseWithSingleDeployment(project1, environment: "Live", version: "success1", isSuccess: true);
            var release2_1 = Datasetup.AddRelease(project2, version: "fail");
            Datasetup.AddDeployment(release2_1, isSuccess: false, environment: "Live");
            Datasetup.AddDeployment(release2_1, isSuccess: false, environment: "Live");

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            var project1Result = result.Single(p => p.ProjectGroup == "Group1");
            Assert.AreEqual(2, project1Result.Releases.Count(r => !r.WasSuccessful));
            Assert.IsTrue(project1Result.Releases.Any(r => r.Version == "nolive"));
            Assert.IsTrue(project1Result.Releases.Any(r => r.Version == "fail"));
        }

        [Test]
        public void Calculate_When_Empty_Dataset_Returns_Empty()
        {
            // Arrange
            // Handled in set up

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [Test]
        public void Calculate_When_Dataset_Null_Throws_Exception()
        {
            // Assert
            Assert.Throws<ApplicationException>(delegate
            {
                _calculator.Calculate(null);
            });
        }

        #endregion
    }
}