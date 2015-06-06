using System.Net;
using Coachseek.API.Client.Services;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public class TestAuthenticatedApiClient : AuthenticatedApiClient
    {
        protected override void MakeAdditionalChangesToRequest(HttpWebRequest request)
        {
            SetTestingHeader(request);
        }

        private static void SetTestingHeader(WebRequest request)
        {
            request.Headers["Testing"] = "true";
        }
    }
}
