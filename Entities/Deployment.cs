using System;

namespace TechTest.Entities
{
    public class Deployment
    {
        /// <summary>
        /// The environment the release was deployed to
        /// </summary>
        public string environment { get; set; }

        /// <summary>
        /// A timestamp when the deployment took place
        /// </summary>
        public DateTime created { get; set; }

        /// <summary>
        /// Whether the deployment was successful
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// The name of the deployment
        /// </summary>
        public string name { get; set; }
    }
}
