using CoachSeek.Api.Tests.Integration.Models;
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
        private string _email;

        protected Guid BusinessId { get; set; }

        protected string BaseUrl
        {
            //get { return "http://coachseek-api.azurewebsites.net/api"; }
            get { return "http://localhost:5272/api"; }
        }

        protected abstract string RelativePath { get; }

        protected string Url
        {
            get { return string.Format("{0}/{1}", BaseUrl, RelativePath); }
        }

        protected string Email
        {
            get { return _email; }
            set { _email = value ?? RandomEmail; }
        }

        protected string Username
        {
            get { return Email; }
        }

        protected string Password { get; set; }


        protected Response PostAnonymously<TResponse>(string json)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(Url));

            return Post<TResponse>(json, http);
        }

        protected Response PostAnonymously<TResponse>(string json, string relativePath)
        {
            var url = string.Format("{0}/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));

            return Post<TResponse>(json, http);
        }

        protected Response Post<TResponse>(string json)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            SetBasicAuthHeader(http, Username, Password);

            return Post<TResponse>(json, http);
        }

        protected Response Post<TResponse>(string json, string relativePath)
        {
            var url = string.Format("{0}/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            SetBasicAuthHeader(http, Username, Password);

            return Post<TResponse>(json, http);
        }

        protected Response Post<TResponse>(string json, HttpWebRequest request)
        {
            PrepareRequest(request);

            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(json);

            var newStream = request.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            try
            {
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
                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        var obj = JsonConvert.DeserializeObject<ApplicationError[]>(errors);

                        return new Response(status, obj);
                    }
                }
            }
        }

        private void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        private static void PrepareRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
        }

        protected void AssertStatusCode(HttpStatusCode actualStatusCode, HttpStatusCode expectedStatusCode)
        {
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        protected void AssertSingleError(Response response, string message, string field = null)
        {
            var errors = AssertErrors(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], field, message);
        }

        protected void AssertMultipleErrors(Response response, string[,] expectedErrors)
        {
            var errors = AssertErrors(response);
            Assert.That(errors.GetLength(0), Is.EqualTo(expectedErrors.GetLength(0)));

            var i = 0;
            foreach (var error in errors)
            {
                AssertApplicationError(error, expectedErrors[i, 1], expectedErrors[i, 0]);
                i++;
            }
        }

        private ApplicationError[] AssertErrors(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            return errors;
        }

        protected void AssertApplicationError(ApplicationError error, string field, string message)
        {
            Assert.That(error.field, Is.EqualTo(field));
            Assert.That(error.message, Is.EqualTo(message));
        }

        protected void RegisterTestBusiness()
        {
            var json = CreateTestBusinessRegistrationCommand();
            var response = PostAnonymously<RegistrationData>(json, "BusinessRegistration");
            var registrationResponse = ((RegistrationData)response.Payload);
            BusinessId = registrationResponse.business.id;
        }


        protected string CreateTestBusinessRegistrationCommand()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = RandomString },
                admin = new ApiBusinessAdmin
                {
                    firstName = "Bob",
                    lastName = "Smith",
                    email = Email = RandomEmail,
                    password = Password = "password1"
                }
            };

            return JsonConvert.SerializeObject(registration);
        }

        protected string RandomEmail
        {
            get
            {
                return string.Format("{0}@{1}.com", RandomString, RandomString).ToLower();
            }
        }

        protected string RandomString
        {
            get { return Guid.NewGuid().ToString().ToLower().Replace("-", ""); }
        }
    }
}
