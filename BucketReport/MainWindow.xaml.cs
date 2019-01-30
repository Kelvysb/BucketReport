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
using RestSharp;
using Newtonsoft.Json;
using BucketReport.Basic;
using BControls;
using BucketReport.Layers.BackEnd;
using BucketReport.Layers.FrontEnd;
using System.Threading;
using System.ComponentModel;

namespace BucketReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Declarations
        private Filter selectedFilter = null;
        private Thread task;
        private bool active = true;
        List<UserIssue> issuesControls;
        #endregion

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadwindow();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnAddFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addFilter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                removeFilter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                openConfig();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearLog();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnEditFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                editFilter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearFilter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnSynthetize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                report();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                quickSearch();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                about();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Constructors
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                BucketReportBE.Initialize();

                BMessage.sbInitialize((Brush)FindResource("BaseColor"),
                                        (Brush)FindResource("BackColor"),
                                        (Brush)FindResource("FontColor"),
                                        (Brush)FindResource("FontColor"),
                                        (FontFamily)FindResource("Font"),
                                        BucketReportBE.LogsFolder);

                if (BucketReportBE.Instance.Configuration.IssuesRepository.Equals("")
                    || BucketReportBE.Instance.Configuration.BaseApiUri.Equals("")
                    || BucketReportBE.Instance.Configuration.UserKey.Equals("")
                    || BucketReportBE.Instance.Configuration.UserSecret.Equals(""))
                {
                    if (!openConfig())
                    {
                        Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }                    
        }
        #endregion

        #region Methods

        private void loadwindow()
        {
            try
            {
                reloadFilters();
                filter();
                refresh();
                task = new Thread(() => taskExecution());
                task.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("Loading", ex);
            }
        }

        private void reloadFilters()
        {
            try
            {
                cmbFilters.Items.Clear();
                BucketReportBE.Instance.Filters.ForEach(filter =>
                {
                    cmbFilters.Items.Add(filter.Description);
                });
                selectedFilter = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void quickSearch()
        {

            List<UserIssue> auxIssuesControls;

            try
            {

                issuesControls.ForEach(issue => issue.Visibility = Visibility.Visible);

                auxIssuesControls = issuesControls.FindAll(issues =>
                {
                    return (issues.Issue.id.ToString().Contains(txtSearch.Text)
                            || issues.Issue.title.ToString().Trim().ToUpper().Contains(txtSearch.Text.Trim().ToUpper()));
                    
                });

                if(auxIssuesControls.Count > 0)
                {
                    issuesControls.ForEach(issue => issue.Visibility = Visibility.Collapsed);
                    auxIssuesControls.ForEach(issue => issue.Visibility = Visibility.Visible);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching", ex);
            }
        }

        private void loadIssues()
        {
            try
            {
                grdLoading.Visibility = Visibility.Visible;
                stkIssues.Children.Clear();
                issuesControls = new List<UserIssue>();
                BucketReportBE.Instance.LoadedIssues.ForEach(issue =>
                {
                    issuesControls.Add(new UserIssue(issue));
                    issuesControls.Last().Margin = new Thickness(2);
                    stkIssues.Children.Add(issuesControls.Last());
                });

                lblTotal.Content = BucketReportBE.Instance.LoadedIssues.Count().ToString();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                grdLoading.Visibility = Visibility.Hidden;
            }
        }

        private void report()
        {
            FrmReport report;
            try
            {

                report = new FrmReport(selectedFilter);
                report.ShowDialog();
                report = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening report", ex);
            }
        }

        private void about()
        {
            FrmAbout about;
            try
            {

                about = new FrmAbout();
                about.ShowDialog();
                about = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening Synthetize", ex);
            }
        }

        private void addLog(List<Tuple<Issue, string>> logs)
        {
            usrLog logControl;

            try
            {
                logs.ForEach(log =>
                {
                    logControl = new usrLog(log.Item1, log.Item2);
                    logControl.Margin = new Thickness(2);
                    stkLog.Children.Add(logControl);
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool openConfig()
        {
            FrmConfig config;
            try
            {
                config = new FrmConfig();
                return config.ShowDialog() == true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening configuration", ex);
            }
        }

        private void filter()
        {
            try
            {
                if (cmbFilters.SelectedIndex != -1)
                {
                    BucketReportBE.Instance.filterIssues(BucketReportBE.Instance.Filters[cmbFilters.SelectedIndex]);
                    selectedFilter = BucketReportBE.Instance.Filters[cmbFilters.SelectedIndex];
                    loadIssues();
                }
                else
                {
                    BucketReportBE.Instance.filterIssues();
                    selectedFilter = null;
                    loadIssues();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error filtering", ex);
            }
        }

        private void clearFilter()
        {
            try
            {
                cmbFilters.SelectedIndex = -1;
                selectedFilter = null;
                filter();
            }
            catch (Exception ex)
            {
                throw new Exception("Error on clear", ex);
            }
        }

        private void editFilter()
        {
            FrmFilter filter;
            try
            {
                if(cmbFilters.SelectedIndex != -1)
                {
                    filter = new FrmFilter(BucketReportBE.Instance.Filters[cmbFilters.SelectedIndex], false);
                    filter.ShowDialog();
                    filter = null;
                }              
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding filter", ex);
            }
        
        }

        private void addFilter()
        {
            FrmFilter filter;
            try
            {
                filter = new FrmFilter(new Filter(), false);
                if (filter.ShowDialog() == true)
                {
                    cmbFilters.Items.Add(BucketReportBE.Instance.Filters.Last().Description);
                    cmbFilters.SelectedIndex = BucketReportBE.Instance.Filters.Count - 1;
                }
                filter = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding filter", ex);
            }
                     
        }

        private void removeFilter()
        {
            try
            {
                if (cmbFilters.SelectedIndex != -1)
                {
                    BucketReportBE.Instance.Filters.RemoveAt(cmbFilters.SelectedIndex);
                    BucketReportBE.Instance.saveFilters();
                    reloadFilters();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing filter", ex);
            }
        }

        private void refresh()
        {
            Thread refreshTask;

            try
            {
                grdLoading.Visibility = Visibility.Visible;
                refreshTask = new Thread(() => refreshTaskExecute());
                refreshTask.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("Error refreshing", ex);
            }
        }

        private void refreshTaskExecute()
        {
            List<Tuple<Issue, string>> logs;

            try
            {

                logs = BucketReportBE.Instance.getUpdateIssues();

                Dispatcher.Invoke(() => {
                    addLog(logs);
                    loadIssues();
                });

            }
            catch (Exception ex)
            {
                throw new Exception("Error refreshing", ex);
            }
            finally
            {
                Dispatcher.Invoke(() => {
                    grdLoading.Visibility = Visibility.Hidden;
                });
            }
        }

        private void clearLog()
        {
            try
            {
                stkLog.Children.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error clearing the log", ex);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);
                active = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void taskExecution()
        {
            int count = 0;
            List<Tuple<Issue, string>> logs;

            try
            {
                do
                {
                    count++;

                    if (!(BucketReportBE.Instance.Configuration.IssuesRepository.Equals("")
                        || BucketReportBE.Instance.Configuration.BaseApiUri.Equals("")
                        || BucketReportBE.Instance.Configuration.UserKey.Equals("")
                        || BucketReportBE.Instance.Configuration.UserSecret.Equals("")))
                    {
                        if (count >= BucketReportBE.Instance.Configuration.RefreshTime)
                        {
                            count = 0;

                            logs = BucketReportBE.Instance.getUpdateIssues();

                            Dispatcher.Invoke(() => {
                                addLog(logs);
                                loadIssues();
                            });
                        }
                    }
                    Thread.Sleep(1000);
                } while (active);

            }
            catch (Exception)
            {

            }
        }


        #endregion

        #region Properties

        #endregion

        
    }
}
