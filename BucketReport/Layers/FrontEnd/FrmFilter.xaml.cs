using BControls;
using BucketReport.Basic;
using BucketReport.Layers.BackEnd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FrmFilter.xaml
    /// </summary>
    public partial class FrmFilter : Window
    {
        #region Declarations
        bool saved = false;
        bool isBase = false;
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addSubFilters();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void removeSubItem(UsrFilter filter)
        {
            try
            {
                removeSubFilters(filter);
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                save();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadObject();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Constructor
        public FrmFilter(Filter filter, bool isBase)
        {
            try
            {
                InitializeComponent();
                Filter = filter;               
                this.isBase = isBase;

                if (Filter == null)
                {
                    Filter = new Filter();
                    if (this.isBase)
                    {
                        Filter.Description = "Base Filter";
                    }
                    else
                    {
                        Filter.Description = "Filter";
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
        private void loadObject()
        {
            UsrFilter subItem;

            try
            {
                txtDescription.Text = Filter.Description;

                Filter.Fields.ForEach(subField =>
                {
                    subItem = new UsrFilter(subField);
                    subItem.Margin = new Thickness(2);
                    subItem.evRemove += removeSubItem;
                    stkItens.Children.Add(subItem);
                });               
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void addSubFilters()
        {
            UsrFilter subItem;

            try
            {
                Filter.Fields.Add(new Field());
                subItem = new UsrFilter(Filter.Fields.Last());
                subItem.Margin = new Thickness(2);
                subItem.evRemove += removeSubItem;
                stkItens.Children.Add(subItem);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding field", ex);
            }
        }

        private void removeSubFilters(UsrFilter filter)
        {
            try
            {
                Filter.Fields.Remove(filter.Field);
                stkItens.Children.Remove(filter);
                filter.evRemove -= removeSubItem;          
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing field", ex);
            }
        }

        private void save()
        {
            Filter auxFilter;
            try
            {

                if (!txtDescription.Text.Equals(""))
                {
                    if (Filter.Fields.Count() >= 0)
                    {

                        Filter.Description = txtDescription.Text;

                        if (isBase)
                        {
                            Filter.Base = 1;
                            BucketReportBE.Instance.BaseFilter = Filter;
                        }
                        else
                        {
                            Filter.Base = 0;
                            if (Filter.Id == 0)
                            {
                                BucketReportBE.Instance.Filters.Add(Filter);
                            }
                            else
                            {
                                auxFilter = BucketReportBE.Instance.Filters.Find(filter => filter.Id == Filter.Id);
                                if (auxFilter != null)
                                {
                                    auxFilter.Base = 0;
                                    auxFilter.Description = Filter.Description;
                                    auxFilter.Fields = Filter.Fields;
                                }
                            }

                        }

                        BucketReportBE.Instance.saveFilters();

                        saved = true;

                        Close();

                    }
                    else
                    {
                        BMessage.Instance.fnMessage("Must have at least one field.", "Bucket Report", MessageBoxButton.OK);
                    }
                }
                else
                {
                    BMessage.Instance.fnMessage("Must have an description.", "Bucket Report", MessageBoxButton.OK);
                }
               
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);
                DialogResult = saved;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public Filter Filter { get; set; }
        #endregion

    }

}
