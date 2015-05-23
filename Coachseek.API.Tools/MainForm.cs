using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;

namespace Coachseek.API.Tools
{
    public partial class MainForm : Form
    {
        const string AdminUserName = "userZvFXUEmjht1hFJGn+H0YowMqO+5u5tEI";
        const string AdminPassword = "passYBoVaaWVp1W9ywZOHK6E6QXFh3z3+OUf";

        
        public MainForm()
        {
            InitializeComponent();
        }

        private void UnsubscribeButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UnsubscribeEmail(EmailTextBox.Text.Trim().ToLower());
            EmailTextBox.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }



        public static string BaseUrl
        {
#if DEBUG
            get { return "https://localhost:44300"; }
#else
            get { return "https://api.coachseek.com"; }
#endif
        }

        private void UnsubscribeEmail(string emailAddress)
        {
            var url = string.Format("Email/Unsubscribe?email={0}", HttpUtility.UrlEncode(emailAddress));
            AdminAuthenticatedGet<string>(url);
        }

        protected Response AdminAuthenticatedGet<TResponse>(string relativePath)
        {
            var url = string.Format("{0}/Admin/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, AdminUserName, AdminPassword);

            return Get<TResponse>(http);
        }

        private static void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        private Response Get<TResponse>(HttpWebRequest request)
        {
            PrepareGetRequest(request);

            return HandleResponse<TResponse>(request);
        }

        private void PrepareGetRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "GET";

            SetTestingHeader(request);
        }

        private void SetTestingHeader(WebRequest request)
        {
            if (IsTestingCheckBox.Checked)
                request.Headers["Testing"] = "true";
        }

        private static Response HandleResponse<TResponse>(HttpWebRequest request)
        {
            try
            {
                request.Timeout = 200000;
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<TResponse>(content);

                return new Response(status, obj);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    return new Response(HttpStatusCode.RequestTimeout);

                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        return new Response(status, DeserialiseErrors(errors));
                    }
                }
            }
        }

        private static object DeserialiseErrors(string errors)
        {
            try
            {
                return JsonConvert.DeserializeObject<ApplicationError[]>(errors);
            }
            catch (JsonSerializationException)
            {
                return JsonConvert.DeserializeObject<ApplicationError>(errors);
            }
        }
    }
}
