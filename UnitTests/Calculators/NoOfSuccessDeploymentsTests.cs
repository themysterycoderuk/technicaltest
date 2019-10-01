using NUnit.Framework;
using System;
using System.Collections.Generic;
using techTest.Interfaces.Business.Calculators;
using TechTest.Business;
using TechTest.Entities;

namespace TechTest.UnitTests.Calculators
{
    public class NoOfSuccessDeploymentsTests
    {
        private INoOfSuccessDeploymentsCalc _calculator;
        private IList<Project> _projects;

        [SetUp]
        public void Setup()
        {
            _calculator = new NoOfSuccessDeploymentsCalc();
            _projects = new List<Project>();
        }

        #region Test Methods

        [Test]
        public void Calculate_When_Project_Has_Multiple_Releases_With_Success_Deployments_Returns_Correct_Count()
        {
            // Arrange
            var project = new Project()
            {
                 releases = new List<Release>()
                 {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                            new Deployment() { state = "Success" }  
                         }
                     },
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                            new Deployment() { state = "Success" }
                         }
                     }
                 }
            };

            _projects.Add(project);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void Calculate_When_Project_Has_Release_With_Multiple_Success_Deployments_Returns_Correct_Count()
        {
            // Arrange
            var project = new Project()
            {
                releases = new List<Release>()
                 {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                            new Deployment() { state = "Success" },
                            new Deployment() { state = "Success" },
                            new Deployment() { state = "Success" }
                         }
                     }
                 }
            };

           _projects.Add(project);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Calculate_When_Multiple_Success_Deployments_Across_Projects_Returns_Correct_Count()
        {
            // Arrange
            var project1 = new Project()
            {
                releases = new List<Release>()
                 {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                            new Deployment() { state = "Success" }
                         }
                     }
                 }
            };

            var project2 = new Project()
            {
                releases = new List<Release>()
                 {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                            new Deployment() { state = "Success" },
                            new Deployment() { state = "Success" },
                            new Deployment() { state = "Success" }
                         }
                     }
                 }
            };

            _projects.Add(project1);
            _projects.Add(project2);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(4, result);
        }

        [Test]
        public void Calculate_When_Deployment_Unsuccessful_Not_Included_In_Count()
        {
            // Arrange
            var project = new Project()
            {
                releases = new List<Release>()
                 {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                         {
                             new Deployment() { state = "Success" },
                            new Deployment() { state = "Unsuccesful" }
                         }
                     }
                 }
            };

            _projects.Add(project);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Calculate_When_No_Projects_Returns_Zero()
        {
            // Arrange
            // Handled in set up method

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Calculate_When_No_Releases_Returns_Zero()
        {
            // Arrange
            var project = new Project()
            {
                 releases = new List<Release>()
            };

            _projects.Add(project);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Calculate_When_No_Deployments_Returns_Zero()
        {
            // Arrange
            var project = new Project()
            {
                releases = new List<Release>()
                {
                     new Release()
                     {
                         deployments = new List<Deployment>()
                     }
                 }
            };

            _projects.Add(project);

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Calculate_When_Empty_Dataset_Returns_Zero()
        {
            // Arrange
            // Handled in set up

            // Act
            var result = _calculator.Calculate(_projects);

            // Assert
            Assert.AreEqual(0, result);
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
