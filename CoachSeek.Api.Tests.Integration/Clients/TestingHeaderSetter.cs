using System.Net;
using System.Net.Http;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public static class TestingHeaderSetter
    {
        public static void SetTestingHeader(WebRequest request)
        {
            request.Headers["Testing"] = "true";
        }

        public static void SetTestingHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Testing", "true");
        }
    }
}
