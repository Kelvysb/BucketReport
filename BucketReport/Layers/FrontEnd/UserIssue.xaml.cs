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
    /// Interaction logic for UserIssue.xaml
    /// </summary>
    public partial class UserIssue : UserControl
    {

        #region Declarations
        bool loaded = false;
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

        #region Constructor
        public UserIssue(Issue issue)
        {
            try
            {
                InitializeComponent();
                Issue = issue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        private void loadObject()
        {
            try
            {
                lblId.Content = Issue.id;
                lblTitle.Content = Issue.title;

                if (Issue.assignee != null)
                {
                    lblAssignee.Content = Issue.assignee.display_name;
                }
                else
                {
                    lblAssignee.Content = "";
                }

                if (Issue.component != null)
                {
                    lblComponent.Content = Issue.component.name;
                }
                else
                {
                    lblComponent.Content = "";
                }
                if (Issue.milestone != null)
                {
                    lblMilestone.Content = Issue.milestone.name;
                }
                else
                {
                    lblMilestone.Content = "";
                }

                if (Issue.version != null)
                {
                    lblVersion.Content = Issue.version.name;
                }
                else
                {
                    lblVersion.Content = "";
                }

                lblStatus.Content = Issue.state;
                lblType.Content = Issue.type;
                lblUpdate.Content = Issue.updated_on.ToString("yyyy-MM-dd HH:mm:ss") ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void openIssue()
        {
            try
            {
                System.Diagnostics.Process.Start(Issue.links.html.href);
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening issue.", ex);
            }
        }
        #endregion

        #region Properties
        public Issue Issue { get; set; }

        #endregion


    }
}
