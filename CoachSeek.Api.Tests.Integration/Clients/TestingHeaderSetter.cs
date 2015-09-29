using System.Net;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public static class TestingHeaderSetter
    {
        public static void SetTestingHeader(WebRequest request)
        {
            request.Headers["Testing"] = "true";
        }
    }
}
