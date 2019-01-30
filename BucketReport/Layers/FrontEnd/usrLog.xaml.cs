using BucketReport.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BucketReport.Layers.FrontEnd
{
    /// <summary>
    /// Interaction logic for usrLog.xaml
    /// </summary>
    public partial class usrLog : UserControl
    {
        #region Declarations
        private Issue issue;
        private string log;
        private DateTime dateTime;
        #endregion

        #region Events
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadObject();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                openIssue();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Constructors
        public usrLog(Issue issue, string log)
        {
            try
            {
                InitializeComponent();
                dateTime = DateTime.Now;
                this.issue = issue;
                this.log = log;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        private void loadObject(){
            try
            {
                txtLog.Text = log;
                if (issue == null)
                {
                    btnOpen.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                throw  new Exception("Error loading", ex);
            }
        }

        private void openIssue()
        {
            try
            {
                if(issue != null)
                {
                    System.Diagnostics.Process.Start(issue.links.html.href);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening issue.", ex);
            }
        }
        #endregion

        #region Properties

        #endregion

  
    }
}
