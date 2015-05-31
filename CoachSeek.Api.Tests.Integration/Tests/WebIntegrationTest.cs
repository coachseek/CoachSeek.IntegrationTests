using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class WebIntegrationTest
    {
        protected const string AdminUserName = "userZvFXUEmjht1hFJGn+H0YowMqO+5u5tEI";
        protected const string AdminPassword = "passYBoVaaWVp1W9ywZOHK6E6QXFh3z3+OUf";


        protected CoachSteve Steve { get; set; }
        protected CoachAaron Aaron { get; set; }
        protected CoachBobby Bobby { get; set; }


        private string _email;

        public static string BaseUrl
        {
#if DEBUG
            get { return "https://localhost:44300"; }
#else
            get { return "https://api.coachseek.com"; }
#endif
        }
        

        public ExpectedBusiness Business { get; set; }


        protected abstract string RelativePath { get; }

        protected Uri Url
        {
            get { return new Uri(string.Format("{0}/{1}", BaseUrl, RelativePath)); }
        }

        protected Uri OnlineBookingUrl
        {
            get { return new Uri(string.Format("{0}/OnlineBooking/{1}", BaseUrl, RelativePath)); }
        }

        protected Uri AdminUrl
        {
            get { return new Uri(string.Format("{0}/Admin/{1}", BaseUrl, RelativePath)); }
        }

        protected string Email
        {
            get { return _email; }
            set { _email = value ?? Random.RandomEmail; }
        }

        protected string Username
        {
            get { return Email; }
        }

        protected string Password { get; set; }


        public Response PostAnonymously<TResponse>(string json)
        {
            var http = CreateAnonymousWebRequest(Url);
            return Post<TResponse>(json, http);
        }

        protected Response PostAnonymously<TResponse>(string json, string relativePath)
        {
            var url = string.Format("{0}/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBusinessDomainHeader(http, Business.Domain);

            return Post<TResponse>(json, http);
        }

        protected Response Post<TResponse>(string json)
        {
            var http = CreateAuthenticatedWebRequest();

            return Post<TResponse>(json, http);
        }

        protected Response PostForOnlineBooking<TResponse>(string json)
        {
            var http = CreateOnlineBookingWebRequest();

            return Post<TResponse>(json, http);
        }

        private HttpWebRequest CreateAuthenticatedWebRequest()
        {
            var http = (HttpWebRequest)WebRequest.Create(Url);
            SetBasicAuthHeader(http, Business.UserName, Business.Password);
            return http;
        }

        private HttpWebRequest CreateAnonymousWebRequest(Uri url)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            SetBusinessDomainHeader(http, Business.Domain);
            return http;
        }

        private HttpWebRequest CreateOnlineBookingWebRequest()
        {
            return CreateAnonymousWebRequest(OnlineBookingUrl);
        }

        protected Response Post<TResponse>(string json, string relativePath)
        {
            var url = string.Format("{0}/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, Business.UserName, Business.Password);

            return Post<TResponse>(json, http);
        }

        protected Response Delete<TResponse>(string relativePath, Guid id)
        {
            var url = string.Format("{0}/{1}/{2}", BaseUrl, relativePath, id);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, Business.UserName, Business.Password);

            return Delete<TResponse>(http);
        }

        protected Response DeleteAnonymously<TResponse>(string relativePath, Guid id)
        {
            var url = string.Format("{0}/{1}/{2}", BaseUrl, relativePath, id);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));

            return Delete<TResponse>(http);
        }


        protected Response GetAnonymously<TResponse>(string url)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBusinessDomainHeader(http, Business.Domain);

            return Get<TResponse>(http);
        }

        protected Response AuthenticatedGet<TResponse>(string relativePath, Guid id)
        {
            var url = string.Format("{0}/{1}/{2}", BaseUrl, relativePath, id);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));

            return AuthenticatedGet<TResponse>(url);
        }

        protected Response AuthenticatedGet<TResponse>(string url)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, Business.UserName, Business.Password);

            return Get<TResponse>(http);
        }

        protected Response AdminAuthenticatedGet<TResponse>(string relativePath)
        {
            var url = string.Format("{0}/Admin/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, AdminUserName, AdminPassword);

            return Get<TResponse>(http);
        }

        protected Response AdminAuthenticatedGet<TResponse>(string relativePath, string searchParameter)
        {
            var url = string.Format("{0}/Admin/{1}/{2}", BaseUrl, relativePath, searchParameter);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, AdminUserName, AdminPassword);

            return Get<TResponse>(http);
        }

        private Response Get<TResponse>(HttpWebRequest request)
        {
            PrepareGetRequest(request);

            // NOTE: DO NOT REMOVE THIS CHECK!!
            // We do not want to send heaps of testing data to our production database!!
            Assert.That(request.Headers["Testing"], Is.EqualTo("true"));

            return HandleResponse<TResponse>(request);
        }

        protected Response Post<TResponse>(string json, HttpWebRequest request)
        {
            PreparePostRequest(request);

            SendData(json, request);

            return HandleResponse<TResponse>(request);
        }

        private Response Delete<TResponse>(HttpWebRequest request)
        {
            PrepareDeleteRequest(request);

            // NOTE: DO NOT REMOVE THIS CHECK!!
            // We do not want to delete data to our production database!!
            Assert.That(request.Headers["Testing"], Is.EqualTo("true"));

            return HandleResponse<TResponse>(request);
        }


        private static Response HandleResponse<TResponse>(HttpWebRequest request)
        {
            try
            {
                request.Timeout = 200000;
                var response = request.GetResponse();
                var status = ((HttpWebResponse) response).StatusCode;
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

                var status = ((HttpWebResponse) ex.Response).StatusCode;
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

        private static void SendData(string json, HttpWebRequest request)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(json);

            // NOTE: DO NOT REMOVE THIS CHECK!!
            // We do not want to send heaps of testing data to our production database!!
            Assert.That(request.Headers["Testing"], Is.EqualTo("true"));

            var newStream = request.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
        }

        private static void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        private static void SetBusinessDomainHeader(WebRequest request, string domain)
        {
            if (domain != null)
                request.Headers["Business-Domain"] = domain;
        }

        private static void SetTestingHeader(WebRequest request)
        {
            request.Headers["Testing"] = "true";
        }

        private static void PreparePostRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";

            SetTestingHeader(request);
        }

        private static void PrepareGetRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "GET";

            SetTestingHeader(request);
        }

        private static void PrepareDeleteRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "DELETE";

            SetTestingHeader(request);
        }


        protected void AssertStatusCode(HttpStatusCode actualStatusCode, HttpStatusCode expectedStatusCode)
        {
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        protected void AssertNotFound(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.NotFound);
        }

        protected void AssertUnauthorised(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        protected T AssertSuccessResponse<T>(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<T>());
            return (T)response.Payload;
        }

        protected ApplicationError[] AssertErrorResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            return (ApplicationError[])response.Payload;
        }

        protected ApplicationError AssertSingleError(Response response, string message, string field = null)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], field, message);

            return errors[0];
        }

        protected void AssertMultipleErrors(Response response, string[,] expectedErrors)
        {
            var errors = AssertErrorResponse(response);
            Assert.That(errors.GetLength(0), Is.EqualTo(expectedErrors.GetLength(0)));

            var i = 0;
            foreach (var error in errors)
            {
                AssertApplicationError(error, expectedErrors[i, 1], expectedErrors[i, 0]);
                i++;
            }
        }

        protected void AssertApplicationError(ApplicationError error, string field, string message)
        {
            Assert.That(error.field, Is.EqualTo(field));
            Assert.That(error.message, Is.EqualTo(message));
        }

        protected void RegisterTestBusiness()
        {
            Business = new RandomBusiness();
            BusinessRegistrar.RegisterBusiness(Business);
        }

        protected string CreateTestBusinessRegistrationCommand()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = Random.RandomString },
                admin = new ApiBusinessAdmin
                {
                    firstName = "Bob",
                    lastName = "Smith",
                    email = Email = Random.RandomEmail,
                    password = Password = "password1"
                }
            };

            return JsonConvert.SerializeObject(registration);
        }

        protected string BuildGetAllUrl()
        {
            return string.Format("{0}/{1}", BaseUrl, RelativePath);
        }

        protected string BuildGetByIdUrl(Guid id)
        {
            return string.Format("{0}/{1}/{2}", BaseUrl, RelativePath, id);
        }
    }
}
