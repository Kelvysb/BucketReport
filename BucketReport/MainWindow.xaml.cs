﻿using System;
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

namespace BucketReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Declarations

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
                Synthetize();
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
                BucketReportBE.Instance.getUpdateIssues();
                loadIssues();
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadIssues()
        {
            UserIssue issueControl;
            try
            {
                stkIssues.Children.Clear();
                BucketReportBE.Instance.LoadedIssues.ForEach(issue =>
                {
                    issueControl = new UserIssue(issue);
                    issueControl.Margin = new Thickness(2);
                    stkIssues.Children.Add(issueControl);
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Synthetize()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening Synthetize", ex);
            }
        }

        private void addLog()
        {
            try
            {

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
                    loadIssues();
                }
                else
                {
                    BucketReportBE.Instance.filterIssues();
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
            try
            {
                BucketReportBE.Instance.getUpdateIssues();
                loadIssues();
            }
            catch (Exception ex)
            {
                throw new Exception("Error refreshing", ex);
            }
        }

        private void clearLog()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error clearing the log", ex);
            }
        }



        #endregion

        #region Properties

        #endregion
        
    }
}
