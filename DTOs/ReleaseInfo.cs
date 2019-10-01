using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ReleaseInfo
    {
        public string Version { get; set; }
        public bool WasSuccessful { get; set; }
        public int NoOfDeployments { get; set; }
        public bool RepeatedDeployments { get; set; }
    }
}
