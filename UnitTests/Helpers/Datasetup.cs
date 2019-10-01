using System;
using System.Collections.Generic;
using TechTest.Entities;

namespace TechTest.UnitTests.Helpers
{
    public static class Datasetup
    {
        public const string cDEFAULT_PROJECTGROUP = "testgroup";
        public const string cDEFAULT_ENVIRONMENT = "testenv";
        public const string cDEFAULT_NOTSUCCESS = "Failed";
        public const int cDEFAULT_CREATED_DAYS_AGO = -100;
        public const string cDEFAULT_VERSION = "99.9";

       
        public static Project AddProject(
            IList<Project> projects,
            Guid? id = null,
            string group = null,
            IList<Entities.Environment> environments = null)
        {
            var project = new Project()
            {
                project_id = id ?? new Guid(),
                project_group = group ?? cDEFAULT_PROJECTGROUP,
                environments = environments ?? new List<Entities.Environment>()
                {
                    new Entities.Environment()
                    {
                        environment = cDEFAULT_ENVIRONMENT
                    }
                }
            };

            projects.Add(project);
            return project;
        }

        public static Project AddProjectWithSingleDeployment(
            IList<Project> projects,
            Guid? id = null,
            string group = null,
            IList<Entities.Environment> projectenvironments = null,
            string version = null,
            bool? isSuccess = false,
            string environment = null,
            DateTime? created = null)
        {
            var project = AddProject(projects, id, group, projectenvironments);
            AddReleaseWithSingleDeployment(projects[0], version, isSuccess, environment, created);
            return project;
        }

        public static Release AddRelease(Project project, string version = null)
        {
            if (project.releases == null)
            {
                project.releases = new List<Release>();
            }

            var release = new Release()
            {
                 version = version ?? cDEFAULT_VERSION
            };

            project.releases.Add(release);
            return release;
        }

        public static Release AddReleaseWithSingleDeployment(
            Project project, 
            string version = null, 
            bool? isSuccess = false,
            string environment = null,
            DateTime? created = null)
        {
            var release = AddRelease(project, version);
            AddDeployment(project.releases[0], isSuccess, environment, created);
            return release;
        }

        public static Deployment AddDeployment(
            Release release,
            bool? isSuccess = false,
            string environment = null,
            DateTime? created = null)
        {
            if (release.deployments == null)
            {
                release.deployments = new List<Deployment>();
            }

            var deployment = new Deployment()
            {
                state = isSuccess.Value ? "Success" : cDEFAULT_NOTSUCCESS,
                environment = environment ?? cDEFAULT_ENVIRONMENT,
                created = created ?? DateTime.Now.AddDays(cDEFAULT_CREATED_DAYS_AGO)
            };

            release.deployments.Add(deployment);

            return deployment;
        }
    }
}
