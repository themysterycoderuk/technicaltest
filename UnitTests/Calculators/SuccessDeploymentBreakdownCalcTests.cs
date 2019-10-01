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
    public class SuccessDeploymentBreakdownCalcTests
    {
        private ISuccessDeploymentBreakdownCalc _calculator;
        private IList<Project> _projects;

        [SetUp]
        public void Setup()
        {
            _calculator = new SuccessDeploymentBreakdownCalc();
            _projects = new List<Project>();
        }

        #region Test Methods

        // Breaks down successful deployments by project group,
        // environment and year
        [Test]
        public void Calculate_Returns_Correct_Project_Group()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, "testgroup", null, null,
                true, null, null
            );

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(r => r.ProjectGroup == "testgroup"));
        }

        [Test]
        public void Calculate_Returns_Correct_Environment()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, null, null, null,
                true, "testenv", null
            );

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(r => r.Environment == "testenv"));
        }

        [Test]
        public void Calculate_Returns_Correct_Year()
        {
            // Arrange
            var dt = DateTime.Now.AddDays(-100);
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, null, null, null,
                true, null, dt
            );

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(r => r.Year == dt.Year));
        }

        [Test]
        public void Calculate_Returns_Correct_Count_By_Year()
        {
            // Arrange
            var dtNow = DateTime.Now;
            var dtYearAgo = DateTime.Now.AddYears(-1);
            var project = Datasetup.AddProject(_projects);
            var release1 = Datasetup.AddRelease(project);
            var release2 = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release1, true, null, dtNow);
            Datasetup.AddDeployment(release1, true, null, dtYearAgo);
            Datasetup.AddDeployment(release2, true, null, dtNow);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(2, result.Single(r => r.Year == dtNow.Year).NoOfSuccessDeployments);
            Assert.AreEqual(1, result.Single(r => r.Year == dtYearAgo.Year).NoOfSuccessDeployments);
        }

        [Test]
        public void Calculate_Returns_Correct_Count_By_Environment()
        {
            // Arrange
            var project = Datasetup.AddProject(_projects);
            var release1 = Datasetup.AddRelease(project);
            var release2 = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release1, true, "env1");
            Datasetup.AddDeployment(release1, true, "env2");
            Datasetup.AddDeployment(release2, true, "env1");

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(2, result.Single(r => r.Environment == "env1").NoOfSuccessDeployments);
            Assert.AreEqual(1, result.Single(r => r.Environment == "env2").NoOfSuccessDeployments);
        }

        [Test]
        public void Calculate_Returns_Correct_Count_By_Project_Group()
        {
            // Arrange
            var project1_1 = Datasetup.AddProject(_projects, null, "group1");
            var project1_2 = Datasetup.AddProject(_projects, null, "group1");
            var project2_1 = Datasetup.AddProject(_projects, null, "group2");
            var release1_1_1 = Datasetup.AddRelease(project1_1);    
            var release1_1_2 = Datasetup.AddRelease(project1_1);    
            var release1_2_1 = Datasetup.AddRelease(project1_2);
            var release2_1_1 = Datasetup.AddRelease(project2_1);
            Datasetup.AddDeployment(release1_1_1, true);            //group1
            Datasetup.AddDeployment(release1_1_1, true);            //group1
            Datasetup.AddDeployment(release1_1_2, true);            //group1
            Datasetup.AddDeployment(release1_2_1, true);            //group1
            Datasetup.AddDeployment(release2_1_1, true);            //group2

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(4, result.Single(r => r.ProjectGroup == "group1").NoOfSuccessDeployments);
            Assert.AreEqual(1, result.Single(r => r.ProjectGroup == "group2").NoOfSuccessDeployments);
        }

        [Test]
        public void Calculate_Doesnt_Return_Unsuccesful_Deployments()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, "testgroup", null, null,
                false, null, null
            );

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.IsFalse(result.Any());

        }

        [Test]
        public void Calculate_Doesnt_Count_Unsuccesful_Deployments()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, "testgroup", null, null,
                false, null, null
            );

            // Arrange
            Datasetup.AddProjectWithSingleDeployment(
                _projects,
                null, "testgroup", null, null,
                true, null, null
            );

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result.Sum(s => s.NoOfSuccessDeployments));
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
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Calculate_When_Dataset_Null_Throws_Exception()
        {
            // Assert
            Assert.Throws<ApplicationException>(delegate {
                _calculator.Calculate(null);
            });
        }

        #endregion
    }
}
