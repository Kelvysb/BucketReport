using BucketReport.Basic;
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
        private string workDirectory;
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

                workDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BucketReport\\";
                if (!Directory.Exists(workDirectory))
                {
                    Directory.CreateDirectory(workDirectory);
                }

                LoadedIssues = new List<Issue>();

                if (File.Exists(workDirectory + CONS_CONFIG_FILE))
                {
                    file = new StreamReader(workDirectory + CONS_CONFIG_FILE);
                    configFile = file.ReadToEnd();
                    file.Close();
                    file.Dispose();
                    file = null;
                    Configuration = JsonConvert.DeserializeObject<BRConfig>(configFile);
                }
                else
                {
                    Configuration = new BRConfig();
                    Configuration.BaseApiUri = "https://api.bitbucket.org/2.0/repositories/";
                    Configuration.AuthenticationUri = "https://bitbucket.org/site/oauth2/access_token";
                    Configuration.IssuesRepository = "";
                    Configuration.UserKey = "";
                    Configuration.UserSecret = "";
                    Configuration.RefreshTime = 60;
                    Configuration.LastUpdate = DateTime.Parse("1900-01-01T00:00:00");
                    saveConfig();
                }

                repository = new BucketReportRep(workDirectory + CONS_DB_FILE);

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
                file = new StreamWriter(workDirectory + CONS_CONFIG_FILE, false);
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

        private List<Issue> getIssues()
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

        private List<Issue> getIssues(Filter filter)
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

        public void filterIssues(Filter filter)
        {
            try
            {
                LoadedIssues = getIssues(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void filterIssues()
        {
            try
            {
                LoadedIssues = getIssues();
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
            IRestResponse response;
            RestClient client;
            RestRequest request;

            try
            {
                client = new RestClient(Configuration.AuthenticationUri);
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=" + Configuration.UserKey + "&client_secret=" + Configuration.UserSecret, ParameterType.RequestBody);
                response = client.Execute(request);
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

            IRestResponse response;
            RestClient client;
            RestRequest request;

            try
            {
                client = new RestClient(Configuration.AuthenticationUri);
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=refresh_token&client_id=" + Configuration.UserKey + "&refresh_token=" + token.refresh_token, ParameterType.RequestBody);
                response = client.Execute(request);
                token = JsonConvert.DeserializeObject<Token>(response.Content);
                tokenValid = DateTime.Now.AddSeconds(token.expires_in);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Tuple<Issue, string>> getUpdateIssues()
        {

            List<Tuple<Issue, string>> result = new List<Tuple<Issue, string>>();
            List<Issue> issues = new List<Issue>();
            Issue oldIssue;
            string query = "";
            BucketResponseIssue bucketResponse;
            IRestResponse response;
            RestClient client;
            RestRequest request;
            List<Issue> allIssues = new List<Issue>();

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

                if(BaseFilter != null)
                {
                    query = "(" + BaseFilter.getQueryString() + ") AND updated_on > " + Configuration.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    query = "updated_on > " + Configuration.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                if (Configuration.IssuesRepository.StartsWith("/"))
                {
                    Configuration.IssuesRepository = Configuration.IssuesRepository.Substring(1);
                }

                client = new RestClient(Configuration.BaseApiUri + Configuration.IssuesRepository + "?" + "access_token=" + token.access_token + "&q=" + query);
                request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                response = client.Execute(request);

                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    getToken();

                    client = new RestClient(Configuration.BaseApiUri + Configuration.IssuesRepository + "?" + "access_token=" + token.access_token + "&q=" + query);
                    request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    response = client.Execute(request);

                }

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    bucketResponse = JsonConvert.DeserializeObject<BucketResponseIssue>(response.Content);

                    if (bucketResponse != null)
                    {

                        if (bucketResponse.values != null)
                        {


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


                            allIssues = getIssues();

                            issues.ForEach(issue =>
                            {

                                oldIssue = allIssues.Find(iss => iss.id == issue.id);

                                if (oldIssue == null)
                                {
                                    result.Add(Tuple.Create(issue, "New issue: " + issue.id + " = " + issue.title));
                                }
                                else
                                {
                                    if (!oldIssue.Equals(issue))
                                    {
                                        result.Add(Tuple.Create(issue, "Issue update: " + issue.id + " = " + issue.title + "\r\n" + issue.getChanges(oldIssue)));
                                    }
                                }

                                repository.saveIssue(issue);
                            });


                            if (issues.Count > 0)
                            {
                                Configuration.LastUpdate = issues.Max(issue => issue.updated_on);
                                saveConfig();
                            }

                        }
                    }
                    else
                    {
                        result.Add(Tuple.Create<Issue, string>(null, "Error connecting to the server."));
                    }
                }
                else
                {
                    result.Add(Tuple.Create<Issue, string>(null, "Server error: " + response.StatusDescription));
                }

            }
            catch (Exception ex)
            {
                result.Add(Tuple.Create<Issue, string>(null, "Error retrieving issues from the server."));
            }

            return result;
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
        public string WorkDirectory { get => workDirectory; }
        #endregion
    }
}
