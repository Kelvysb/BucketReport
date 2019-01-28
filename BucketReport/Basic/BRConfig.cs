using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketReport.Basic
{    
    public class BRConfig
    {
        #region Declarations

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string UserKey { get; set; }
        public string UserSecret { get; set; }
        public string BaseApiUri { get; set; }
        public string AuthenticationUri { get; set; }
        public string IssuesRepository { get; set; }
        public string PullRequestsRepository { get; set; }
        public int RefreshTime { get; set; }
        public DateTime LastUpdate { get; set; }
        #endregion
    }
}
