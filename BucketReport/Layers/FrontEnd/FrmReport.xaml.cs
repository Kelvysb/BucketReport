using BControls;
using BucketReport.Basic;
using BucketReport.Layers.BackEnd;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace BucketReport.Layers.FrontEnd
{
    /// <summary>
    /// Interaction logic for FrmReport.xaml
    /// </summary>
    public partial class FrmReport : Window
    {

        #region Types
        internal class view
        {
            public view(string key, int value)
            {
                this.key = key;
                this.value = value;
            }

            public string key { get; set; }
            public int value { get; set; }
        }
        #endregion

        #region Declarations
        private Filter filter;
        private List<view> byState;
        private List<view> byKind;
        private List<view> byComponent;
        private List<view> byPriority;
        private List<RawIssue> issues;
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
                issues = BucketReportBE.Instance.LoadedIssues.Select(issue => new RawIssue(issue)).ToList();
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

                Topmost = true;
                Thread.Sleep(10);
                Topmost = false;

                if(filter != null)
                {
                    lblFilter.Content = filter.Description;
                }
                else
                {
                    lblFilter.Content = "All";
                }

                

                byState = new List<view>();
                byState = (from RawIssue issue in issues
                           group issue by issue.state into result
                           select new view(result.Key.Equals("") ? "None" : result.Key, result.Count())).ToList();
                dtgState.ItemsSource = null;
                dtgState.ItemsSource = byState;

                byComponent = new List<view>();
                byComponent = (from RawIssue issue in issues
                               group issue by issue.component into result
                               select new view(result.Key.Equals("")?"None": result.Key, result.Count())).ToList();
                dtgComponent.ItemsSource = null;
                dtgComponent.ItemsSource = byComponent;

                byKind = new List<view>();
                byKind = (from RawIssue issue in issues
                          group issue by issue.kind into result
                            select new view(result.Key.Equals("") ? "None" : result.Key, result.Count())).ToList();
                dtgKind.ItemsSource = null;
                dtgKind.ItemsSource = byKind;

                byPriority = new List<view>();
                byPriority = (from RawIssue issue in issues
                              group issue by issue.priority into result
                              select new view(result.Key.Equals("") ? "None" : result.Key, result.Count())).ToList();
                dtgPriority.ItemsSource = null;
                dtgPriority.ItemsSource = byPriority;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading report.", ex);
            }
        }

        private string getResumetext()
        {
            string line = "";
            try
            {

                line += "Filter: " + lblFilter.Content + "\r\n\r\n";

                line += "By State\r\n";
                line += "-----------------------\r\n";
                line += "State".PadRight(20, ' ') + "Count\r\n";
                byState.ForEach(item => line += item.key.PadRight(20, ' ') + item.value + "\r\n");
                line += "-----------------------\r\n\r\n";

                line += "By Component\r\n";
                line += "-----------------------\r\n";
                line += "Component".PadRight(20, ' ') + "Count\r\n";
                byComponent.ForEach(item => line += item.key.PadRight(20, ' ') + item.value + "\r\n");
                line += "-----------------------\r\n\r\n";

                line += "By Kind\r\n";
                line += "-----------------------\r\n";
                line += "Kind".PadRight(20, ' ') + "Count\r\n";
                byKind.ForEach(item => line += item.key.PadRight(20, ' ') + item.value + "\r\n");
                line += "-----------------------\r\n\r\n";

                line += "By Priority\r\n";
                line += "-----------------------\r\n";
                line += "Priority".PadRight(20, ' ') + "Count\r\n";
                byPriority.ForEach(item => line += item.key.PadRight(20, ' ') + item.value + "\r\n");
                line += "-----------------------\r\n\r\n";

                return line;
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
              Clipboard.SetText(getResumetext(), TextDataFormat.Text);
              BMessage.Instance.fnMessage("Copied", "Bucket Report", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                throw new Exception("Error copying report.", ex);
            }
        }

        private void saveTxt()
        {
            StreamWriter file;
            CommonSaveFileDialog saveDialog;
            
            try
            {

                saveDialog = new CommonSaveFileDialog();
                saveDialog.Title = "Save resume";
                saveDialog.Filters.Add(new CommonFileDialogFilter("Text file", "txt"));
                saveDialog.DefaultExtension = "txt";
                saveDialog.DefaultFileName = lblFilter.Content.ToString().Trim() + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
                if (saveDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    file = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8);
                    file.Write(getResumetext());
                    file.Close();
                    file.Dispose();
                    file = null;
                    BMessage.Instance.fnMessage("Saved", "Bucket Report", MessageBoxButton.OK);
                }

                Topmost = true;
                Thread.Sleep(10);
                Topmost = false;


            }
            catch (Exception ex)
            {
                throw new Exception("Error saving report.", ex);
            }
            finally
            {
                Topmost = true;
                Thread.Sleep(10);
                Topmost = false;
            }
        }

        private void SaveCSV()
        {

            StreamWriter file;
            CommonSaveFileDialog saveDialog;

            try
            {

                saveDialog = new CommonSaveFileDialog();
                saveDialog.Title = "Save CSV file";
                saveDialog.Filters.Add(new CommonFileDialogFilter("CSV file", "csv"));
                saveDialog.DefaultExtension = "csv";
                saveDialog.DefaultFileName = lblFilter.Content.ToString().Trim() + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv";


                if (saveDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    file = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8);
                    file.WriteLine("number;title;kind;assignee;type;priority;state;component;milestone;version;reporter;created_on;updated_on");
                    issues.ForEach(issue => file.WriteLine(issue.ToString()));
                    file.Close();
                    file.Dispose();
                    file = null;
                    BMessage.Instance.fnMessage("Saved", "Bucket Report", MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error saving report.", ex);
            }
            finally
            {
                Topmost = true;
                Thread.Sleep(10);
                Topmost = false;
            }
        }

        private void exit()
        {
            try
            {
                Close();
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
