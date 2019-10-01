using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class PipelineBreakdown
    {
        public string ProjectGroup { get; set; }
        public IList<ReleaseInfo> Releases { get; set; }
    }
}
