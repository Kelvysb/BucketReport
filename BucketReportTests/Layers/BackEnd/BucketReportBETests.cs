using Microsoft.VisualStudio.TestTools.UnitTesting;
using BucketReport.Layers.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketReport.Layers.BackEnd.Tests
{
    [TestClass()]
    public class BucketReportBETests
    {
        [TestMethod()]
        public void getIssuesTest()
        {
            try
            {
                BucketReportBE.Initialize();
                BucketReportBE.Instance.getIssues();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            
        }
    }
}