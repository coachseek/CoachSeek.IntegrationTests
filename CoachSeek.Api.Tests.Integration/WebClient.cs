using System;
using System.IO;
using System.Net;
using System.Text;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration
{
    public static class WebClient
    {
        public static Response AnonymousPost<TResponse>(string json, string relativePath, string scheme = "https")
        {
            var http = CreateWebRequest(relativePath, scheme);

            return Post<TResponse>(json, http);
        }

        public static Response AnonymousPostForBusiness<TResponse>(string businessDomain, string relativePath, string json)
        {
            var http = CreateWebRequest(relativePath);
            SetBusinessDomainHeader(http, businessDomain);

            return Post<TResponse>(json, http);
        }

        public static Response AuthenticatedPost<TResponse>(string username, string password, string relativePath, string json)
        {
            var http = CreateWebRequest(relativePath);
            SetBasicAuthHeader(http, username, password);

            return Post<TResponse>(json, http);
        }

        private static HttpWebRequest CreateWebRequest(string relativePath, string scheme = "https")
        {
            var url = CreateUrl(scheme, relativePath);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        private static void SetBusinessDomainHeader(WebRequest request, string domain)
        {
            if (domain != null)
                request.Headers["Business-Domain"] = domain;
        }

        private static void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        private static Uri CreateUrl(string scheme, string relativePath)
        {
#if DEBUG
            if (scheme == "https")
                return new Uri(string.Format("https://localhost:44300/{0}", relativePath));
            if (scheme == "http")
                return new Uri(string.Format("http://localhost:5272/{0}", relativePath));

            throw new InvalidOperationException("Invalid scheme");
#else
            return new Uri(string.Format("{0}://api.coachseek.com/{1}", scheme, relativePath));
#endif
        }

        //protected Response Post<TResponse>(string json)
        //{
        //    var http = CreateAuthenticatedWebRequest();

        //    return Post<TResponse>(json, http);
        //}

        //protected Response PostForOnlineBooking<TResponse>(string json)
        //{
        //    var http = CreateOnlineBookingWebRequest();

        //    return Post<TResponse>(json, http);
        //}

        //private HttpWebRequest CreateAuthenticatedWebRequest()
        //{
        //    var http = (HttpWebRequest)WebRequest.Create(Url);
        //    SetBasicAuthHeader(http, Username, Password);
        //    return http;
        //}

        //private HttpWebRequest CreateBusinessAnonymousWebRequest(Uri url)
        //{
        //    var http = (HttpWebRequest)WebRequest.Create(url);
        //    SetBusinessDomainHeader(http, BusinessDomain);
        //    return http;
        //}

        //private HttpWebRequest CreateOnlineBookingWebRequest()
        //{
        //    return CreateAnonymousWebRequest(OnlineBookingUrl);
        //}

        //protected Response Post<TResponse>(string json, string relativePath)
        //{
        //    var url = string.Format("{0}/{1}", BaseUrl, relativePath);
        //    var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //    SetBasicAuthHeader(http, Username, Password);

        //    return Post<TResponse>(json, http);
        //}



        private static Response Post<TResponse>(string json, HttpWebRequest request)
        {
            PreparePostRequest(request);
            SendData(json, request);
            return HandleResponse<TResponse>(request);
        }

        private static void PreparePostRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";

            SetTestingHeader(request);
        }

        private static void SetTestingHeader(WebRequest request)
        {
            request.Headers["Testing"] = "true";
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

        private static Response HandleResponse<TResponse>(HttpWebRequest request)
        {
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
