using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BucketReport.Basic
{

    public class RawFilter
    {
        #region Declarations

        #endregion

        #region Constructor
        public RawFilter()
        {
            try
            {
                id = 0;
                isBase = 0;
                description = "";
                value = JsonConvert.SerializeObject(new Filter());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RawFilter(Filter filter)
        {
            try
            {
                id = filter.Id;
                isBase = filter.Base;
                description = filter.Description;
                value = JsonConvert.SerializeObject(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public Filter getFilter()
        {
            try
            {
                return JsonConvert.DeserializeObject<Filter>(value);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public int id { get; set; }
        public int isBase { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        #endregion
    }

    public class Filter
    {
        public Filter()
        {
            Id = 0;
            Base = 0;
            Description = "";
            Fields = new List<Field>();
        }
        #region Declarations

        #endregion

        #region Constructor

        #endregion

        #region Methods
        public string getQueryString()
        {
            try
            {
                return mountQuery(Fields);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string getWhereString()
        {
            try
            {
                return mountQuerySql(Fields);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string mountQuery(List<Field> fields)
        {
            string result;

            try
            {

                result = "";

                fields.ForEach(field =>
                {

                if (!field.Equals(fields.First()))
                {
                    result += " " + field.LogicOperator;
                }

                if (field.SubFields.Count == 0)
                {
                    if (field.FieldName.Equals("id") || field.FieldName.Equals("created_on") || field.FieldName.Equals("updated_on"))
                    {
                        result += " " + field.FieldName + " " + field.Operator + " " + field.Value;
                    }
                    else if (field.FieldName.Equals("assignee") || field.FieldName.Equals("reporter"))
                    {
                        result += " " + field.FieldName + ".display_name " + field.Operator + " \"" + field.Value + "\"";
                        }
                        else if (field.FieldName.Equals("milestone") || field.FieldName.Equals("version") || field.FieldName.Equals("component"))
                        {
                            result += " " + field.FieldName + ".name " + field.Operator + " \"" + field.Value + "\"";
                        }
                        else
                        {
                            result += " " + field.FieldName + " " + field.Operator + " \"" + field.Value + "\"";
                        }
                    }
                    else
                    {
                        result += " (" + mountQuery(field.SubFields) + ")";
                    }

                });

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string mountQuerySql(List<Field> fields)
        {
            string result;

            try
            {

                result = "";

                fields.ForEach(field =>
                {

                    if (!field.Equals(fields.First()))
                    {
                        result += " " + field.LogicOperator;
                    }

                    if (field.SubFields.Count == 0)
                    {
                        if (!field.Operator.Trim().ToUpper().Contains("~"))
                        {
                            if (!field.FieldName.Equals("id"))
                            {
                                result += " " + field.FieldName + " " + field.Operator + " \"" + field.Value + "\""; 
                            }
                            else
                            {
                                result += " " + field.FieldName + " " + field.Operator + " " + field.Value;
                            }
                        }
                        else
                        {
                            if (!field.Operator.Trim().ToUpper().Contains("!"))
                            {
                                result += " " + field.FieldName + " like (\"%" + field.Value + "%\")";
                            }
                            else
                            {
                                result += " " + field.FieldName + " not like (\"%" + field.Value + "%\")";
                            }
                        }                        
                    }
                    else
                    {
                        result += " (" + mountQuery(field.SubFields) + ")";
                    }

                });

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public int Id { get; set; }
        public int Base { get; set; }
        public string Description { get; set; }
        public List<Field> Fields { get; set; }
        #endregion
    }

    public class Field
    {

        #region Declarations

        #endregion

        #region Constructor
        public Field(string @operator, string logicOperator, string field, string value)
        {
            try
            {
                Operator = @operator;
                LogicOperator = logicOperator;
                FieldName = field;
                Value = value;
                SubFields = new List<Field>();
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public Field(string @operator, string logicOperator, List<Field> subFields)
        {
            try
            {
                Operator = @operator;
                LogicOperator = logicOperator;
                FieldName = "";
                Value = "";
                SubFields = subFields;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Field()
        {
            try
            {
                Operator = "=";
                LogicOperator = "AND";
                FieldName = "";
                Value = "";
                SubFields = new List<Field>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods

        #endregion

        #region Properties
        public string Operator { get; set; }
        public string LogicOperator { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public List<Field> SubFields { get; set; }
        #endregion
    }
}
