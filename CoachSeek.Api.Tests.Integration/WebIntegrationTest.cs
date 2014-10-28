using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace CoachSeek.Api.Tests.Integration
{
    public abstract class WebIntegrationTest
    {
        //protected string BUSINESS_ID = "01234567-89AB-CDEF-0123-456789ABCDEF";
        protected Guid BusinessId { get; set; }

        protected string BaseUrl
        {
            get { return "http://localhost:5272/api"; }
        }

        protected abstract string RelativePath { get; }

        protected string Url
        {
            get { return string.Format("{0}/{1}", BaseUrl, RelativePath); }
        }

        protected Response Post<TData>(string json)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(Url));

            return Post<TData>(json, http);
        }

        protected Response Post<TData>(string json, string relativePath)
        {
            var url = string.Format("{0}/{1}", BaseUrl, relativePath);
            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));

            return Post<TData>(json, http);
        }

        protected Response Post<TData>(string json, HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";

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
                var obj = JsonConvert.DeserializeObject<TData>(content);

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

        protected void AssertStatusCode(HttpStatusCode actualStatusCode, HttpStatusCode expectedStatusCode)
        {
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        protected void AssertApplicationError(ApplicationError error, string field, string message)
        {
            Assert.That(error.field, Is.EqualTo(field));
            Assert.That(error.message, Is.EqualTo(message));
        }

        protected void RegisterTestBusiness()
        {
            var json = CreateTestBusinessRegistrationCommand();
            var response = Post<BusinessData>(json, "BusinessRegistration");
            BusinessId = ((BusinessData)response.Payload).id;
        }


        protected string CreateTestBusinessRegistrationCommand()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                businessName = RandomString,
                registrant = new ApiBusinessRegistrant
                {
                    firstName = "Bob",
                    lastName = "Smith",
                    email = RandomEmail,
                    password = "password"
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
