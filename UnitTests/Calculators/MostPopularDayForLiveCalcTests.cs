using NUnit.Framework;
using System;
using System.Collections.Generic;
using techTest.Interfaces.Business.Calculators;
using TechTest.Business.Calculators;
using TechTest.Entities;
using TechTest.UnitTests.Helpers;

namespace TechTest.UnitTests.Calculators
{
    public class MostPopularDayForLiveCalcTests
    {
        private IMostPopularDayForLiveCalc _calculator;
        private IList<Project> _projects;
        private DateTime _dtNow;
        private DateTime _dtWeekAgo;
        private DateTime _dt11DaysAgo;
        private DateTime _dt18DaysAgo;

        [SetUp]
        public void Setup()
        {
            _calculator = new MostPopularDayForLiveCalc();
            _projects = new List<Project>();
            _dtNow = DateTime.Now;
            _dtWeekAgo = _dtNow.AddDays(-7);
            _dt11DaysAgo = _dtNow.AddDays(-11);
            _dt18DaysAgo = _dtNow.AddDays(-18);
        }

        #region Test Methods

        [Test]
        public void Calculate_Includes_All_Live_Deployments()
        {
            // Arrange
            var project = Datasetup.AddProject(_projects);
            var release = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release, environment: "Live", created: _dtNow);
            Datasetup.AddDeployment(release, environment: "Live", created: _dtWeekAgo);
            Datasetup.AddDeployment(release, environment: "Live", created: _dt11DaysAgo);
           
            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(_dtNow.DayOfWeek, result);
        }

        [Test]
        public void Calculate_Includes_All_Releases_With_LiveDeployment()
        {
            // Arrange
            var project = Datasetup.AddProject(_projects);
            var release1 = Datasetup.AddRelease(project);
            var release2 = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release1, environment: "Live", created: _dtNow);
            Datasetup.AddDeployment(release1, environment: "Live", created: _dt11DaysAgo);
            Datasetup.AddDeployment(release2, environment: "Live", created: _dt18DaysAgo);
            
            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(_dt11DaysAgo.DayOfWeek, result);
        }

        [Test]
        public void Calculate_Includes_All_Projects_With_LiveDeployment()
        {
            // Arrange
            var project1 = Datasetup.AddProject(_projects);
            var project2 = Datasetup.AddProject(_projects);
            var release1_1 = Datasetup.AddRelease(project1);
            var release1_2 = Datasetup.AddRelease(project1);
            var release2_1 = Datasetup.AddRelease(project2);
            Datasetup.AddDeployment(release1_1, environment: "Live", created: _dtNow);
            Datasetup.AddDeployment(release1_2, environment: "Live", created: _dt11DaysAgo);
            Datasetup.AddDeployment(release2_1, environment: "Live", created: _dtWeekAgo);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(_dtNow.DayOfWeek, result);
        }

        [Test]
        public void Calculate_When_Tied_Returns_First_Day()
        {
            // Arrange
            var project = Datasetup.AddProject(_projects);
            var release = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release, environment: "Live", created: _dtNow);
            Datasetup.AddDeployment(release, environment: "Live", created: _dt11DaysAgo);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            var expected = Math.Min((int)_dtNow.DayOfWeek, (int)_dt11DaysAgo.DayOfWeek);
            Assert.AreEqual(expected, (int) result);
        }

        [Test]
        public void Calculate_Doesnt_Include_Non_Live_Deployments()
        {
            // Arrange
            var project = Datasetup.AddProject(_projects);
            var release = Datasetup.AddRelease(project);
            Datasetup.AddDeployment(release, environment: "Live", created: _dt11DaysAgo);
            Datasetup.AddDeployment(release, environment: "Test", created: _dtNow);
            Datasetup.AddDeployment(release, environment: "Staging", created: _dtWeekAgo);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(_dt11DaysAgo.DayOfWeek, result);
        }

        [Test]
        public void Calculate_When_No_Live_Deployments_Returns_Null()
        {
            // Arrange
            Datasetup.AddProjectWithSingleDeployment(_projects, environment: "test");

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Calculate_When_Empty_Dataset_Returns_Null()
        {
            // Arrange
            // Handled in set up

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.IsNull(result);
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

