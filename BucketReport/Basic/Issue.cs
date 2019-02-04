using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BucketReport.Basic
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Attachments
    {
        public string href { get; set; }
    }

    public class Watch
    {
        public string href { get; set; }
    }

    public class Comments
    {
        public string href { get; set; }
    }

    public class Vote
    {
        public string href { get; set; }
    }

    public class Links
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public Attachments attachments { get; set; }
        public Self self { get; set; }
        public Watch watch { get; set; }
        public Comments comments { get; set; }
        public Html html { get; set; }
        public Vote vote { get; set; }
        public Avatar avatar { get; set; }
        #endregion
    }

    public class Repository
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public Links links { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string uuid { get; set; }
        #endregion
    }

    public class Reporter
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string username { get; set; }
        public string display_name { get; set; }
        public string account_id { get; set; }
        public Links links { get; set; }
        public string nickname { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        #endregion
    }

    public class Component
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string name { get; set; }
        public Links links { get; set; }
        #endregion
    }

    public class Content
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string raw { get; set; }
        public string markup { get; set; }
        public string html { get; set; }
        public string type { get; set; }
        #endregion
    }

    public class Assignee
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string username { get; set; }
        public string display_name { get; set; }
        public string account_id { get; set; }
        public Links links { get; set; }
        public string nickname { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        #endregion
    }

    public class Version
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string name { get; set; }
        public Links links { get; set; }
        #endregion
    }

    public class Milestone
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods

        #endregion

        #region Properties
        public string name { get; set; }
        public Links links { get; set; }
        #endregion
    }

    public class Issue
    {
        #region Declarations

        #endregion

        #region Constructors 

        #endregion

        #region Methods
        internal string getChanges(Issue oldIssue)
        {
            string response = "";
            RawIssue rawOldIssue;
            RawIssue rawIssue;

            try
            {

                rawIssue = new RawIssue(this);
                rawOldIssue = new RawIssue(oldIssue);

                if (!rawIssue.priority.Equals(rawOldIssue.priority))
                {
                    response += "Priority: " + rawOldIssue.priority + " -> " + rawIssue.priority + "\n"; 
                }

                if (!rawIssue.kind.Equals(rawOldIssue.kind))
                {
                    response += "kind: " + rawOldIssue.kind + " -> " + rawIssue.kind + "\n";
                }

                if (!rawIssue.title.Equals(rawOldIssue.title))
                {
                    response += "title: " + rawOldIssue.title + " -> " + rawIssue.title + "\n";
                }

                if (!rawIssue.reporter.Equals(rawOldIssue.reporter))
                {
                    response += "reporter: " + rawOldIssue.reporter + " -> " + rawIssue.reporter + "\n";
                }

                if (!rawIssue.component.Equals(rawOldIssue.component))
                {
                    response += "component: " + rawOldIssue.component + " -> " + rawIssue.component + "\n";
                }

                if (!rawIssue.assignee.Equals(rawOldIssue.assignee))
                {
                    response += "assignee: " + rawOldIssue.assignee + " -> " + rawIssue.assignee + "\n";
                }

                if (!rawIssue.state.Equals(rawOldIssue.state))
                {
                    response += "state: " + rawOldIssue.state + " -> " + rawIssue.state + "\n";
                }

                if (!rawIssue.version.Equals(rawOldIssue.version))
                {
                    response += "version: " + rawOldIssue.version + " -> " + rawIssue.version + "\n";
                }

                if (!rawIssue.milestone.Equals(rawOldIssue.milestone))
                {
                    response += "milestone: " + rawOldIssue.milestone + " -> " + rawIssue.milestone + "\n";
                }

                if (!rawIssue.type.Equals(rawOldIssue.type))
                {
                    response += "type: " + rawOldIssue.type + " -> " + rawIssue.type + "\n";
                }

                if (response.Equals(""))
                {
                    response = "Body changes";
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override string ToString()
        {
            string result = "";

            try
            {
                result += id.ToString() + ";";
                result += title + ";";
                result += kind + ";";
                result += type + ";";
                result += priority + ";";
                result += state + ";";

                if (reporter != null)
                {
                    result += reporter.display_name + ";";
                }
                else
                {
                    result += "None" + ";";
                }
                if (assignee != null)
                {
                    result += assignee.display_name + ";";
                }
                else
                {
                    result += "None" + ";";
                }
                if (component != null)
                {
                    result += component.name + ";";
                }
                else
                {
                    result += "None" + ";";
                }
                if (milestone != null)
                {
                    result += milestone.name + ";";
                }
                else
                {
                    result += "None" + ";";
                }

                if (version != null)
                {
                    result += version.name + ";";
                }
                else
                {
                    result += "None" + ";";
                }

                result += created_on.ToString("yyyy-MM-dd HH:mm:ss") + ";";

                if (updated_on != null)
                {
                    result += updated_on.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    result += "None";
                }

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj.GetType().Equals(this.GetType()))
                {
                    return this.ToString().Trim().ToUpper().Equals(obj.ToString().Trim().ToUpper());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Properties
        public string priority { get; set; }
        public string kind { get; set; }
        public Repository repository { get; set; }
        public Links links { get; set; }
        public Reporter reporter { get; set; }
        public string title { get; set; }
        public Component component { get; set; }
        public int votes { get; set; }
        public int watches { get; set; }
        public Content content { get; set; }
        public Assignee assignee { get; set; }
        public string state { get; set; }
        public Version version { get; set; }
        public object edited_on { get; set; }
        public DateTime created_on { get; set; }
        public Milestone milestone { get; set; }
        public DateTime updated_on { get; set; }
        public string type { get; set; }
        public int id { get; set; } 
        #endregion
    }

    public class RawIssue
    {

        #region Declarations

        #endregion

        #region Constructors 
        public RawIssue()
        {
            try
            {
                id = 0;
                number = "";
                title = "";
                kind = "";
                assignee = "";
                type = "";
                priority = "";
                state = "";
                component = "";
                milestone = "";
                version = "";
                reporter = "";
                created_on = DateTime.Now;
                updated_on = DateTime.Now;
                raw = JsonConvert.SerializeObject(new Issue());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public RawIssue(Issue issue)
        {
            try
            {
                id = issue.id;
                number = "#" + issue.id.ToString();
                title = issue.title;
                kind = issue.kind;
                type = issue.type;
                priority = issue.priority;
                state = issue.state;

                if (issue.reporter != null)
                {
                    reporter = issue.reporter.display_name;
                }
                else
                {
                    reporter = "";
                }
                if (issue.assignee != null)
                {
                    assignee = issue.assignee.display_name;
                }
                else
                {
                    assignee = "";
                }
                if (issue.component != null)
                {
                    component = issue.component.name;
                }
                else
                {
                    component = "";
                }
                if (issue.milestone != null)
                {
                    milestone = issue.milestone.name;
                }
                else
                {
                    milestone = "";
                }

                if (issue.version != null)
                {
                    version = issue.version.name;
                }
                else
                {
                    version = "";
                }               

                created_on = issue.created_on;
                updated_on = issue.updated_on;
                raw = JsonConvert.SerializeObject(issue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public Issue getIssue()
        {
            try
            {
                return JsonConvert.DeserializeObject<Issue>(raw);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override string ToString()
        {
            return number + ";" 
                + title + ";"
                + kind + ";"
                + (assignee.Equals("")?"None":assignee) + ";"
                + type + ";"
                + priority + ";"
                + state + ";"
                + (component.Equals("") ? "None" : component) + ";"
                + (milestone.Equals("") ? "None" : milestone) + ";"
                + (version.Equals("") ? "None" : version ) + ";"
                + (reporter.Equals("") ? "None" : reporter) + ";"
                + created_on.ToString("yyyy-MM-dd HH:mm:ss") + ";"
                + (updated_on != null?updated_on.ToString("yyyy-MM-dd HH:mm:ss"):"None");
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj.GetType().Equals(this.GetType()))
                {
                    return this.ToString().Trim().ToUpper().Equals(obj.ToString().Trim().ToUpper());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Properties
        public int id { get; set; }
        public string number { get; set; }
        public string title { get; set; }
        public string kind { get; set; }
        public string assignee { get; set; }
        public string type { get; set; }
        public string priority { get; set; }
        public string state { get; set; }
        public string component { get; set; }
        public string milestone { get; set; }
        public string version { get; set; }
        public string reporter { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public string raw { get; set; }
        #endregion
    }

    public class IssueHistory
    {

        #region Declarations

        #endregion

        #region Constructors 
        public IssueHistory()
        {
            try
            {
                id = 0;
                sequence = 0;
                number = "";
                title = "";
                kind = "";
                assignee = "";
                type = "";
                priority = "";
                state = "";
                component = "";
                milestone = "";
                version = "";
                reporter = "";
                created_on = DateTime.Now;
                updated_on = DateTime.Now;
                eventDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IssueHistory(Issue issue)
        {
            try
            {
                id = issue.id;
                sequence = 0;
                number = "#" + issue.id.ToString();
                title = issue.title;
                kind = issue.kind;
                type = issue.type;
                priority = issue.priority;
                state = issue.state;
                
                created_on = issue.created_on;
                updated_on = issue.updated_on;
                eventDate = DateTime.Now;

                if (issue.reporter != null)
                {
                    reporter = issue.reporter.display_name;                  
                }
                else
                {
                    reporter = "";
                }

                if (issue.assignee != null)
                {
                    assignee = issue.assignee.display_name;
                }
                else
                {
                    assignee = "";
                }
                if (issue.component != null)
                {
                    component = issue.component.name;
                }
                else
                {
                    component = "";
                }
                if (issue.milestone != null)
                {
                    milestone = issue.milestone.name;
                }
                else
                {
                    milestone = "";
                }

                if (issue.version != null)
                {
                    version = issue.version.name;
                }
                else
                {
                    version = "";
                }
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
        public int id { get; set; }
        public int sequence { get; set; }
        public string number { get; set; }
        public string title { get; set; }
        public string kind { get; set; }
        public string assignee { get; set; }
        public string type { get; set; }
        public string priority { get; set; }
        public string state { get; set; }
        public string component { get; set; }
        public string milestone { get; set; }
        public string version { get; set; }
        public string reporter { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public DateTime eventDate { get; set; }
        #endregion
    }
}
