using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDataBase;
using BucketReport.Basic;

namespace BucketReport.Layers.Repository
{
    class BucketReportRep
    {

        #region Declarations
        private IDataBase dataBase;
        private string command;
        private DataSet dataSet;
        private List<clsDataBaseParametes> parameters;
        #endregion

        #region Constructor
        public BucketReportRep(string dbFile)
        {
            clsConfiguration config;
            try
            {
                config = new clsConfiguration();
                config.DataBase = dbFile;
                config.Server = dbFile;
                config.Type = DataBase.enmDataBaseType.SqLite;
                dataBase = DataBase.fnOpenConnection(config);
                initializeDataBase();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        private void initializeDataBase()
        {
            try
            {

                command = "CREATE TABLE IF NOT EXISTS 'issues' (\n";
                command += "'id'	INTEGER NOT NULL,\n";
                command += "'number'	TEXT NOT NULL,\n";
                command += "'title'	TEXT NOT NULL,\n";
                command += "'assignee'	TEXT NOT NULL,\n";
                command += "'type'	TEXT NOT NULL,\n";
                command += "'priority'	TEXT NOT NULL,\n";
                command += "'state'	TEXT NOT NULL,\n";
                command += "'component'	TEXT NOT NULL,\n";
                command += "'milestone'	TEXT NOT NULL,\n";
                command += "'version'	TEXT NOT NULL,\n";
                command += "'reporter'	INTEGER NOT NULL,\n";
                command += "'created_on'	TEXT NOT NULL,\n";
                command += "'updated_on'	TEXT NOT NULL,\n";
                command += "'raw'	TEXT NOT NULL,\n";
                command += "PRIMARY KEY('id')\n";
                command += ");\n";
                dataBase.sbExecute(command);

                command = "CREATE TABLE IF NOT EXISTS 'issuesHistory' (\n";
                command += "'id'	INTEGER NOT NULL,\n";
                command += "'sequence'	INTEGER NOT NULL,\n";
                command += "'number'	TEXT NOT NULL,\n";
                command += "'title'	TEXT NOT NULL,\n";
                command += "'assignee'	TEXT NOT NULL,\n";
                command += "'type'	TEXT NOT NULL,\n";
                command += "'priority'	TEXT NOT NULL,\n";
                command += "'state'	TEXT NOT NULL,\n";
                command += "'component'	TEXT NOT NULL,\n";
                command += "'milestone'	TEXT NOT NULL,\n";
                command += "'version'	TEXT NOT NULL,\n";
                command += "'reporter'	INTEGER NOT NULL,\n";
                command += "'created_on'	TEXT NOT NULL,\n";
                command += "'updated_on'	TEXT NOT NULL,\n";
                command += "'eventDate'	TEXT NOT NULL,\n";
                command += "PRIMARY KEY('id', 'sequence')\n";
                command += ");\n";
                dataBase.sbExecute(command);

                command = "CREATE TABLE IF NOT EXISTS 'filters' (\n";
                command += "'id'	INTEGER NOT NULL,\n";
                command += "'isBase'	INTEGER NOT NULL,\n";
                command += "'description'	TEXT NOT NULL,\n";
                command += "'value'	TEXT NOT NULL,\n";
                command += "PRIMARY KEY('id')\n";
                command += ");\n";
                dataBase.sbExecute(command);

                dataBase.sbCommit();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Issue> getIssues()
        {

            List<RawIssue> rawResult = new List<RawIssue>();

            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "number,\n";
                command += "title,\n";
                command += "assignee,\n";
                command += "type,\n";
                command += "priority,\n";
                command += "state,\n";
                command += "component,\n";
                command += "milestone,\n";
                command += "version,\n";
                command += "reporter,\n";
                command += "created_on,\n";
                command += "updated_on,\n";
                command += "raw\n";
                command += "from issues\n";
                dataSet = dataBase.fnExecute(command);

                rawResult = (from DataRow row in dataSet.Tables[0].Rows
                             select new RawIssue
                             {
                                id = int.Parse(row["id"].ToString()),
                                number = row["number"].ToString(),
                                title = row["title"].ToString(),
                                kind = row["type"].ToString(),
                                assignee = row["assignee"].ToString(),
                                type = row["type"].ToString(),
                                priority = row["priority"].ToString(),
                                state = row["state"].ToString(),
                                component = row["component"].ToString(),
                                milestone = row["milestone"].ToString(),
                                version = row["version"].ToString(),
                                reporter = row["reporter"].ToString(),
                                created_on = DateTime.Parse(row["created_on"].ToString()),
                                updated_on = DateTime.Parse(row["updated_on"].ToString()),
                                raw = row["raw"].ToString()
                             }).ToList();

            }
            catch   (DataBaseException exdb)
            {
                if(exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rawResult.Select(item => item.getIssue()).ToList();

        }

        public List<Issue> getIssues(Filter filter)
        {

            List<RawIssue> rawResult = new List<RawIssue>();

            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "number,\n";
                command += "title,\n";
                command += "assignee,\n";
                command += "type,\n";
                command += "priority,\n";
                command += "state,\n";
                command += "component,\n";
                command += "milestone,\n";
                command += "version,\n";
                command += "reporter,\n";
                command += "created_on,\n";
                command += "updated_on,\n";
                command += "raw\n";
                command += "from issues\n";
                command += "where " + filter.getWhereString() + "\n";
                dataSet = dataBase.fnExecute(command);

                rawResult = (from DataRow row in dataSet.Tables[0].Rows
                             select new RawIssue
                             {
                                 id = int.Parse(row["id"].ToString()),
                                 number = row["number"].ToString(),
                                 title = row["title"].ToString(),
                                 kind = row["type"].ToString(),
                                 assignee = row["assignee"].ToString(),
                                 type = row["type"].ToString(),
                                 priority = row["priority"].ToString(),
                                 state = row["state"].ToString(),
                                 component = row["component"].ToString(),
                                 milestone = row["milestone"].ToString(),
                                 version = row["version"].ToString(),
                                 reporter = row["reporter"].ToString(),
                                 created_on = DateTime.Parse(row["created_on"].ToString()),
                                 updated_on = DateTime.Parse(row["updated_on"].ToString()),
                                 raw = row["raw"].ToString()
                             }).ToList();
            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rawResult.Select(item => item.getIssue()).ToList();

        }

        public void saveIssue(Issue issue)
        {

            List<Issue> result;
            RawIssue raw;
            Filter filter;
            
            try
            {
                filter = new Filter();
                filter.Fields.Add(new Field());
                filter.Fields.First().FieldName = "id";
                filter.Fields.First().Value = issue.id.ToString();

                result = getIssues(filter);

                if(result.Count > 0)
                {
                    saveIssueHistory(new IssueHistory(result.Last()));
                }

                raw = new RawIssue(issue);


                command = "INSERT OR REPLACE INTO issues(\n";
                command += "'id',\n";
                command += "'number',\n";
                command += "'title',\n";
                command += "'assignee',\n";
                command += "'type',\n";
                command += "'priority',\n";
                command += "'state',\n";
                command += "'component',\n";
                command += "'milestone',\n";
                command += "'version',\n";
                command += "'reporter',\n";
                command += "'created_on',\n";
                command += "'updated_on',\n";
                command += "'raw')\n";
                command += "values (\n";
                command += "@id,\n";
                command += "@number,\n";
                command += "@title,\n";
                command += "@assignee,\n";
                command += "@type,\n";
                command += "@priority,\n";
                command += "@state,\n";
                command += "@component,\n";
                command += "@milestone,\n";
                command += "@version,\n";
                command += "@reporter,\n";
                command += "@created_on,\n";
                command += "@updated_on,\n";
                command += "@raw)\n"; ;

                parameters = new List<clsDataBaseParametes>();
                parameters.Add(new clsDataBaseParametes("@id", raw.id.ToString()));
                parameters.Add(new clsDataBaseParametes("@number", raw.number));
                parameters.Add(new clsDataBaseParametes("@title", raw.title));
                parameters.Add(new clsDataBaseParametes("@assignee", raw.assignee));
                parameters.Add(new clsDataBaseParametes("@type", raw.type));
                parameters.Add(new clsDataBaseParametes("@priority", raw.priority));
                parameters.Add(new clsDataBaseParametes("@state", raw.state));
                parameters.Add(new clsDataBaseParametes("@component", raw.component));
                parameters.Add(new clsDataBaseParametes("@milestone", raw.milestone));
                parameters.Add(new clsDataBaseParametes("@version", raw.version));
                parameters.Add(new clsDataBaseParametes("@reporter", raw.reporter));
                parameters.Add(new clsDataBaseParametes("@created_on", raw.created_on.ToString("yyyy-MM-ddTHH:mm:ss")));
                parameters.Add(new clsDataBaseParametes("@updated_on", raw.updated_on.ToString("yyyy-MM-ddTHH:mm:ss")));
                parameters.Add(new clsDataBaseParametes("@raw", raw.raw));


                dataBase.sbExecute(command, parameters);
                dataBase.sbCommit();


            }
            catch (DataBaseException exdb)
            {
                throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IssueHistory> getIssuesHistory()
        {

            List<IssueHistory> result = new List<IssueHistory>();

            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "sequence,\n";
                command += "number,\n";
                command += "title,\n";
                command += "assignee,\n";
                command += "type,\n";
                command += "priority,\n";
                command += "state,\n";
                command += "component,\n";
                command += "milestone,\n";
                command += "version,\n";
                command += "reporter,\n";
                command += "created_on,\n";
                command += "updated_on,\n";
                command += "eventDate\n";
                command += "from issuesHistory\n";
                dataSet = dataBase.fnExecute(command);

                result = (from DataRow row in dataSet.Tables[0].Rows
                             select new IssueHistory
                             {
                                 id = int.Parse(row["id"].ToString()),
                                 sequence = int.Parse(row["sequence"].ToString()),
                                 number = row["number"].ToString(),
                                 title = row["title"].ToString(),
                                 kind = row["kind"].ToString(),
                                 assignee = row[""].ToString(),
                                 type = row["assignee"].ToString(),
                                 priority = row["priority"].ToString(),
                                 state = row["state"].ToString(),
                                 component = row["component"].ToString(),
                                 milestone = row["milestone"].ToString(),
                                 version = row["version"].ToString(),
                                 reporter = row["reporter"].ToString(),
                                 created_on = DateTime.Parse(row["created_on"].ToString()),
                                 eventDate = DateTime.Parse(row["eventDate"].ToString())
                             }).ToList();
            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        public List<IssueHistory> getIssuesHistory(Filter filter)
        {

            List<IssueHistory> result = new List<IssueHistory>();

            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "sequence,\n";
                command += "number,\n";
                command += "title,\n";
                command += "assignee,\n";
                command += "type,\n";
                command += "priority,\n";
                command += "state,\n";
                command += "component,\n";
                command += "milestone,\n";
                command += "version,\n";
                command += "reporter,\n";
                command += "created_on,\n";
                command += "updated_on,\n";
                command += "eventDate\n";
                command += "from issuesHistory\n";
                command += "where " + filter.getWhereString() + "\n";
                dataSet = dataBase.fnExecute(command);

                result = (from DataRow row in dataSet.Tables[0].Rows
                          select new IssueHistory
                          {
                              id = int.Parse(row["id"].ToString()),
                              sequence = int.Parse(row["sequence"].ToString()),
                              number = row["number"].ToString(),
                              title = row["title"].ToString(),
                              kind = row["kind"].ToString(),
                              assignee = row[""].ToString(),
                              type = row["assignee"].ToString(),
                              priority = row["priority"].ToString(),
                              state = row["state"].ToString(),
                              component = row["component"].ToString(),
                              milestone = row["milestone"].ToString(),
                              version = row["version"].ToString(),
                              reporter = row["reporter"].ToString(),
                              created_on = DateTime.Parse(row["created_on"].ToString()),
                              eventDate = DateTime.Parse(row["eventDate"].ToString())
                          }).ToList();

            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        public void saveIssueHistory(IssueHistory history)
        {
            try
            {

                history.sequence = getNextHistorySequence(history.id);

                command = "INSERT OR REPLACE INTO issuesHistory(\n";
                command += "id,\n";
                command += "sequence,\n";
                command += "number,\n";
                command += "title,\n";
                command += "assignee,\n";
                command += "type,\n";
                command += "priority,\n";
                command += "state,\n";
                command += "component,\n";
                command += "milestone,\n";
                command += "version,\n";
                command += "reporter,\n";
                command += "created_on,\n";
                command += "updated_on,\n";
                command += "eventDate)\n";
                command += "values (\n";
                command += "@id,\n";
                command += "@sequence,\n";
                command += "@number,\n";
                command += "@title,\n";
                command += "@assignee,\n";
                command += "@type,\n";
                command += "@priority,\n";
                command += "@state,\n";
                command += "@component,\n";
                command += "@milestone,\n";
                command += "@version,\n";
                command += "@reporter,\n";
                command += "@created_on,\n";
                command += "@updated_on,\n";
                command += "@eventDate)\n"; ;

                parameters = new List<clsDataBaseParametes>();
                parameters.Add(new clsDataBaseParametes("@id", history.id.ToString()));
                parameters.Add(new clsDataBaseParametes("@sequence", history.sequence.ToString()));
                parameters.Add(new clsDataBaseParametes("@number", history.number));
                parameters.Add(new clsDataBaseParametes("@title", history.title));
                parameters.Add(new clsDataBaseParametes("@assignee", history.assignee));
                parameters.Add(new clsDataBaseParametes("@type", history.type));
                parameters.Add(new clsDataBaseParametes("@priority", history.priority));
                parameters.Add(new clsDataBaseParametes("@state", history.state));
                parameters.Add(new clsDataBaseParametes("@component", history.component));
                parameters.Add(new clsDataBaseParametes("@milestone", history.milestone));
                parameters.Add(new clsDataBaseParametes("@version", history.version));
                parameters.Add(new clsDataBaseParametes("@reporter", history.reporter));
                parameters.Add(new clsDataBaseParametes("@created_on", history.created_on.ToString("yyyy-MM-ddTHH:mm:ss")));
                parameters.Add(new clsDataBaseParametes("@updated_on", history.updated_on.ToString("yyyy-MM-ddTHH:mm:ss")));
                parameters.Add(new clsDataBaseParametes("@eventDate", history.eventDate.ToString("yyyy-MM-ddTHH:mm:ss")));

                dataBase.sbExecute(command, parameters);
                dataBase.sbCommit();

            }
            catch (DataBaseException exdb)
            {
                throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int getNextHistorySequence(int id)
        {
            int result = 1;            

            try
            {
                command = "select coalesce(max(sequence), 0) from issuesHistory where id = " + id.ToString();
                dataSet = dataBase.fnExecute(command);
                result = int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) + 1;
            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Filter> getFilters()
        {

            List<RawFilter> rawResult = new List<RawFilter>();
            
            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "isBase,\n";
                command += "description,\n";
                command += "value\n";
                command += "from filters\n";
                command += "where isBase = 0\n";
                dataSet = dataBase.fnExecute(command);

                rawResult = (from DataRow row in dataSet.Tables[0].Rows
                             select new RawFilter
                             {
                                 id = int.Parse(row["id"].ToString()),
                                 description = row["description"].ToString(),
                                 isBase = int.Parse(row["isBase"].ToString()),
                                 value = row["value"].ToString()
                             }).ToList();
            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rawResult.Select(item => item.getFilter()).ToList();

        }

        public Filter getBaseFilter()
        {

            List<RawFilter> rawResult = new List<RawFilter>();
            Filter result = null;

            try
            {
                command = "Select\n";
                command += "id,\n";
                command += "isBase,\n";
                command += "description,\n";
                command += "value\n";
                command += "from filters\n";
                command += "where isBase = 1\n";
                dataSet = dataBase.fnExecute(command);

                rawResult = (from DataRow row in dataSet.Tables[0].Rows
                            select new RawFilter
                            {
                                id = int.Parse(row["id"].ToString()),
                                description = row["description"].ToString(),
                                isBase = int.Parse(row["isBase"].ToString()),
                                value = row["value"].ToString()
                            }).ToList();

                result = rawResult.First().getFilter();

            }
            catch (DataBaseException exdb)
            {
                if (exdb.Code != DataBaseException.enmDataBaseExeptionCode.NotExists)
                {
                    throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        public void saveFilter(Filter filter)
        {
            RawFilter raw;

            try
            {

                if(filter.Id == 0)
                {
                    filter.Id = getFiltersNextId();
                }
                raw = new RawFilter(filter);

                command = "INSERT OR REPLACE INTO filters(\n";
                command += "id,\n";
                command += "isBase,\n";
                command += "description,\n";
                command += "value)\n";
                command += "values (\n";
                command += "@id,\n";
                command += "@isBase,\n";
                command += "@description,\n";
                command += "@value)\n";

                parameters = new List<clsDataBaseParametes>();
                parameters.Add(new clsDataBaseParametes("@id", raw.id.ToString()));
                parameters.Add(new clsDataBaseParametes("@isBase", raw.isBase.ToString()));
                parameters.Add(new clsDataBaseParametes("@description", raw.description));
                parameters.Add(new clsDataBaseParametes("@value", raw.value));

                dataBase.sbExecute(command, parameters);
                dataBase.sbCommit();


            }
            catch (DataBaseException exdb)
            {
                throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getFiltersNextId()
        {
            int result = 1;

            try
            {

                command = "select\n";
                command += "coalesce(max(id), 0)\n";                
                command += "from 'filters'\n";
                dataSet = dataBase.fnExecute(command);
                result = int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) + 1;
            }
            catch (DataBaseException exdb)
            {
                throw new Exception("Database error: " + exdb.Code + " - " + exdb.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Properties

        #endregion

    }
}
