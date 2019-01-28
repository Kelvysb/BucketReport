using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketReport.Basic
{
    public abstract class BucketResponse
    {
        public int pagelen { get; set; }
        public int size { get; set; }
        public int page { get; set; }
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class BucketResponseIssue : BucketResponse
    {
        public List<Issue> values { get; set; }
    }


}
