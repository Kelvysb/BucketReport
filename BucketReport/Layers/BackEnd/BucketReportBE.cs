﻿using BucketReport.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BucketReport.Layers.Repository;
using RestSharp;
using System.Web;

namespace BucketReport.Layers.BackEnd
{
    public class BucketReportBE
    {

        #region Declarations
        private const string CONS_LOGS_FOLDER = "logs\\";
        private const string CONS_CONFIG_FILE = "config.json";
        private const string CONS_DB_FILE = "BR.db";
        private static BucketReportBE instance;
        private Token token;
        private DateTime tokenValid;
        private BucketReportRep repository;
        #endregion

        #region Events
            
        #endregion

        #region Constructors
        private BucketReportBE()
        {

            StreamReader file;   
            string configFile;

            try
            {

                LoadedIssues = new List<Issue>();

                if (File.Exists(CONS_CONFIG_FILE))
                {
                    file = new StreamReader(CONS_CONFIG_FILE);
                    configFile = file.ReadToEnd();
                    file.Close();
                    file.Dispose();
                    file = null;
                    Configuration = JsonConvert.DeserializeObject<BRConfig>(configFile);
                }
                else
                {
                    Configuration = new BRConfig();
                    Configuration.BaseApiUri = "https://api.bitbucket.org/2.0";
                    Configuration.AuthenticationUri = "https://bitbucket.org/site/oauth2/access_token";
                    Configuration.IssuesRepository = "";
                    Configuration.UserKey = "";
                    Configuration.UserSecret = "";
                    Configuration.RefreshTime = 10;
                    Configuration.LastUpdate = DateTime.Parse("1900-01-01T00:00:00");
                    saveConfig();
                }

                repository = new BucketReportRep(CONS_DB_FILE);

                loadFilters();

            }
            catch (Exception)
            {
                throw;
            }
        }  
        
        public static void Initialize()
        {
            try
            {
                instance = new BucketReportBE();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public void saveConfig()
        {
            StreamWriter file;
            string configFile;
            try
            {
                file = new StreamWriter(CONS_CONFIG_FILE, false);
                configFile = JsonConvert.SerializeObject(Configuration);
                file.Write(configFile);
                file.Close();
                file.Dispose();
                file = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Issue> getIssues()
        {
            try
            {
                return repository.getIssues();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Issue> getIssues(Filter filter)
        {
            try
            {
                return repository.getIssues(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void saveIssue(Issue issue)
        {
            try
            {
                repository.saveIssue(issue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadFilters()
        {
            try
            {
                Filters = repository.getFilters();
                BaseFilter = repository.getBaseFilter();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void saveFilters()
        {
            try
            {
                Filters.ForEach(filter => repository.saveFilter(filter));
                if(BaseFilter != null)
                {
                    repository.saveFilter(BaseFilter);                
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void getToken()
        {
            try
            {
                var client = new RestClient(Configuration.AuthenticationUri);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=" + Configuration.UserKey + "&client_secret=" + Configuration.UserSecret, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                token = JsonConvert.DeserializeObject<Token>(response.Content);
                tokenValid = DateTime.Now.AddSeconds(token.expires_in);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void getRefreshToken()
        {
            try
            {
                var client = new RestClient(Configuration.AuthenticationUri);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=refresh_token&client_id=" + Configuration.UserKey + "&refresh_token=" + token.refresh_token, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                token = JsonConvert.DeserializeObject<Token>(response.Content);
                tokenValid = DateTime.Now.AddSeconds(token.expires_in);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> getUpdateIssues()
        {

            List<string> result = new List<string>();
            List<Issue> issues = new List<Issue>();
            Issue oldIssue;
            string query = "";

            try
            {

                if(Configuration.IssuesRepository.Equals("") 
                    || Configuration.BaseApiUri.Equals("") 
                    || Configuration.UserKey.Equals("") 
                    || Configuration.UserSecret.Equals(""))
                {
                    throw new Exception("Check configuration");
                }

                if(token == null)
                {
                    getToken();
                }
                
                if(tokenValid.CompareTo(DateTime.Now) <= 0)
                {
                    getRefreshToken();
                }

                if(BaseFilter != null)
                {
                    query = "(" + BaseFilter.getQueryString() + ") AND updated_on > " + Configuration.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    query = "updated_on > " + Configuration.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                var client = new RestClient(Configuration.BaseApiUri + Configuration.IssuesRepository + "?" + "access_token=" + token.access_token + "&q=" + HttpUtility.ParseQueryString(query));
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                IRestResponse response = client.Execute(request);
                BucketResponseIssue bucketResponse = JsonConvert.DeserializeObject<BucketResponseIssue>(response.Content);

                issues.AddRange(bucketResponse.values);

                if (bucketResponse.next != null)
                {
                    do
                    {
                        client = new RestClient(bucketResponse.next);
                        response = client.Execute(request);
                        bucketResponse = JsonConvert.DeserializeObject<BucketResponseIssue>(response.Content);
                        issues.AddRange(bucketResponse.values);
                    } while (bucketResponse.next != null);
                }

                issues.ForEach(issue =>
                {

                    oldIssue = LoadedIssues.Find(iss => iss.id == issue.id);
                    if(oldIssue == null)
                    {
                        result.Add("New issue: " + issue.id + " = " + issue.title);
                    }
                    else
                    {
                        result.Add("Issue update: " + issue.id + " = " + issue.title + "\n" + issue.getChanges(oldIssue));
                    }
                    repository.saveIssue(issue);
                });


                Configuration.LastUpdate = issues.Max(issue => issue.updated_on);
                saveConfig();

                LoadedIssues = issues;

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Properties
        public static BucketReportBE Instance
        {
            get
            {
                if (instance == null)
                {
                    Initialize();
                }
                return instance;
            }
        }
        public BRConfig Configuration { get; set; }
        public static string LogsFolder => CONS_LOGS_FOLDER;
        public List<Issue> LoadedIssues { get; set; }
        public List<Filter> Filters { get; set; }
        public Filter BaseFilter { get; set; }
        #endregion
    }
}