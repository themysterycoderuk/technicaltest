using System.Collections.Generic;

namespace TechTest.Entities
{
    public class Release
    {
        /// <summary>
        /// A unique version identifier
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// One or more deployments associated with a particular release.
        /// Deployments are ordered from earliest to latest
        /// </summary>
        public IList<Deployment> deployments { get; set; }
    }
}
