using System;
using System.Collections.Generic;

namespace TechTest.Entities
{
    /// <summary>
    /// Simple POCO representation of a project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// A unique GUID
        /// </summary>
        public Guid project_id { get; set; }

        /// <summary>
        /// A parent project group name of which the project is a member
        /// </summary>
        public string project_group { get; set; }

        /// <summary>
        /// A list of deployment environments that the project is deployed to,    for example:
        /// Integration
        /// Regression
        /// QA
        /// UAT
        /// Live
        /// </summary>
        IList<string> environments { get; set; }

        /// <summary>
        /// One or more releases.  Releases are ordered by Version
        /// from earliest to latest
        /// </summary>
        IList<Release> releases { get; set; }
    }
}
