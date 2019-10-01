using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class DeploymentBreakdown
    {
        public string ProjectGroup { get; set; }
        public string Environment { get; set; }
        public int Year { get; set; }
        public int NoOfSuccessDeployments { get; set; }
    }
}
