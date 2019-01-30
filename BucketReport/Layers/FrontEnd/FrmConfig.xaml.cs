using System;
using System.Windows;
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

        private void btnResetDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearDate();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                help();
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

                BMessage.Instance.fnMessage("Configuraton saved.\r\nBucket Report must be restarted to some configurations take effect.",
                    "Bucket Report", MessageBoxButton.OK);

                Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error saving configuration", ex);
            }
        }

        private void clearDate()
        {
            try
            {
                BucketReportBE.Instance.Configuration.LastUpdate = DateTime.Parse("1900-01-01T00:00:00");
                BucketReportBE.Instance.saveConfig();
                BMessage.Instance.fnMessage("Update date reseted.", "Bucket Report", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reset date", ex);
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

        private void help()
        {
            FrmHelp help;
            try
            {
                help = new FrmHelp();
                help.ShowDialog();
                help = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening help", ex);
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
