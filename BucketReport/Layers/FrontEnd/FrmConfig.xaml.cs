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
using System.Windows.Shapes;
using BControls;
using BucketReport.Layers.BackEnd;
using System.ComponentModel;

namespace BucketReport.Layers.FrontEnd
{
    /// <summary>
    /// Interaction logic for FrmConfig.xaml
    /// </summary>
    public partial class FrmConfig : Window
    {

        #region Declarations
        private bool modified = false;
        #endregion

        #region Events
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                save();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnBaseFilter_Click(object sender, RoutedEventArgs e)
        {
               try
            {
                baseFilter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }       
        #endregion

        #region Constructors
        public FrmConfig()
        {
            try
            {
                InitializeComponent();
                txtApiUri.Text = BucketReportBE.Instance.Configuration.BaseApiUri;
                txtAuthUri.Text = BucketReportBE.Instance.Configuration.AuthenticationUri;
                txtIssuesUri.Text = BucketReportBE.Instance.Configuration.IssuesRepository;
                txtUserKey.Text = BucketReportBE.Instance.Configuration.UserKey;
                txtUserSecret.Password = BucketReportBE.Instance.Configuration.UserSecret;
                txtRefreshTime.Text = BucketReportBE.Instance.Configuration.RefreshTime.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        private void save()
        {
            int refreshTime = 0;
            try
            {
                BucketReportBE.Instance.Configuration.BaseApiUri = txtApiUri.Text;
                BucketReportBE.Instance.Configuration.AuthenticationUri = txtAuthUri.Text;
                BucketReportBE.Instance.Configuration.IssuesRepository = txtIssuesUri.Text;
                BucketReportBE.Instance.Configuration.UserKey = txtUserKey.Text;
                BucketReportBE.Instance.Configuration.UserSecret = txtUserSecret.Password;
                if (int.TryParse(txtRefreshTime.Text, out refreshTime)){
                    BucketReportBE.Instance.Configuration.RefreshTime = refreshTime;
                }
                else
                {
                    txtRefreshTime.Text = "10";
                    BucketReportBE.Instance.Configuration.RefreshTime = 10;
                }

                BucketReportBE.Instance.saveConfig();

                modified = true;
                Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error saving configuration", ex);
            }
        }

        private void baseFilter()
        {
            FrmFilter filter;
            try
            {
                filter = new FrmFilter(BucketReportBE.Instance.BaseFilter, true);
                if (filter.ShowDialog() == true)
                {
                    modified = true;
                }
                filter = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving configuration", ex);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                DialogResult = modified;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Properties

        #endregion

        
    }
}
