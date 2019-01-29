using BControls;
using BucketReport.Basic;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace BucketReport.Layers.FrontEnd
{
    /// <summary>
    /// Interaction logic for FrmReport.xaml
    /// </summary>
    public partial class FrmReport : Window
    {

        #region Declarations
        private Filter filter;
        private List<DataSet> views;
        private bool loaded = false;
        #endregion

        #region Events
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                copy();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnSaveCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveCSV();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                saveTxt();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                exit();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadReport();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Constuctors
        public FrmReport(Filter filter)
        {
            try
            {
                InitializeComponent();
                this.filter = filter;
                views = new List<DataSet>();
                cmbViews.Items.Clear();
                cmbViews.Items.Add("All");
                cmbViews.Items.Add("State");
                cmbViews.Items.Add("Component");
                cmbViews.Items.Add("Milestone");
                cmbViews.SelectedIndex = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Methods
        private void loadReport()
        {
            try
            {
                lblFilter.Content = filter.Description;




                loaded = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading report.", ex);
            }
        }

        private void selectView()
        {
            try
            {
                if (loaded)
                {




                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void copy()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error copying report.", ex);
            }
        }

        private void saveTxt()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error saving report.", ex);
            }
        }

        private void SaveCSV()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error saving report.", ex);
            }
        }

        private void exit()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error closing.", ex);
            }
        }

        #endregion

        #region Properties

        #endregion


    }
}
