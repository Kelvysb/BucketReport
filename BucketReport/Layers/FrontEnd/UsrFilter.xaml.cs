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
    /// Interaction logic for UsrFilter.xaml
    /// </summary>
    public partial class UsrFilter : UserControl
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

        public delegate void evRemoveHandler(UsrFilter sender);
        public event evRemoveHandler evRemove;

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

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                evRemove(this);
            }
            catch (Exception)
            {
                throw;
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

        private void cmbLogicOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                updateField();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void cmbField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                updateField();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void cmbOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                updateField();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                updateField();
            }
            catch (Exception ex)
            {
                BControls.BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Constructor
        public UsrFilter(Field filed)
        {
            try
            {
                InitializeComponent();
                Field = filed;

                cmbLogicOperator.Items.Add("AND");
                cmbLogicOperator.Items.Add("OR");
                cmbLogicOperator.SelectedIndex = 0;

                cmbOperator.Items.Add("=");
                cmbOperator.Items.Add("!=");
                cmbOperator.Items.Add("~");
                cmbOperator.Items.Add("!~");
                cmbOperator.Items.Add(">");
                cmbOperator.Items.Add(">=");
                cmbOperator.Items.Add("<");
                cmbOperator.Items.Add("<=");
                cmbOperator.SelectedIndex = 0;

                cmbField.Items.Add("");
                cmbField.Items.Add("id");
                cmbField.Items.Add("title");
                cmbField.Items.Add("reporter");
                cmbField.Items.Add("assignee");
                cmbField.Items.Add("created_on");
                cmbField.Items.Add("updated_on");
                cmbField.Items.Add("state");
                cmbField.Items.Add("kind");            
                cmbField.Items.Add("priority");
                cmbField.Items.Add("version");
                cmbField.Items.Add("component");
                cmbField.Items.Add("milestone");
                cmbField.SelectedIndex = 0;
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

                cmbLogicOperator.SelectedItem = Field.LogicOperator;
                cmbOperator.SelectedItem = Field.Operator;
                cmbField.SelectedItem = Field.FieldName;
                txtValue.Text = Field.Value;

                if(Field.SubFields.Count > 0)
                {
                    this.Height = 300;

                    Field.SubFields.ForEach(subField =>
                    {
                        subItem = new UsrFilter(subField);
                        subItem.Margin = new Thickness(2);
                        subItem.evRemove += removeSubItem;
                        stkSubItens.Children.Add(subItem);
                    });

                }

                loaded = true;

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
                Field.SubFields.Add(new Field());
                subItem = new UsrFilter(Field.SubFields.Last());
                subItem.Margin = new Thickness(2);
                subItem.evRemove += removeSubItem;
                stkSubItens.Children.Add(subItem);

                this.Height = 300;
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
                Field.SubFields.Remove(filter.Field);
                stkSubItens.Children.Remove(filter);
                filter.evRemove -= removeSubItem;

                if(Field.SubFields.Count == 0)
                {
                    this.Height = 50;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing field", ex);
            }
        }

        private void updateField()
        {
            try
            {
                if(Field != null && loaded)
                {
                    Field.LogicOperator = cmbLogicOperator.SelectedItem.ToString();
                    Field.Operator = cmbOperator.SelectedItem.ToString();
                    Field.FieldName = cmbField.SelectedItem.ToString();
                    Field.Value = txtValue.Text;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating field", ex);
            }
        }
        #endregion

        #region Properties
        public Field Field { get; set; }

        #endregion

       
    }
}
