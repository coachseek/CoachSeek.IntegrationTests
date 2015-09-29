using System.Net;
using Coachseek.API.Client.Services;
using CoachSeek.Common;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public class TestCoachseekAuthenticatedApiClient : CoachseekAuthenticatedApiClient
    {
        public TestCoachseekAuthenticatedApiClient(string username,
                                                   string password, 
                                                   string scheme = "https", 
                                                   ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(username, password, scheme, dataFormat)
        { }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            base.ModifyRequest(request);
            TestingHeaderSetter.SetTestingHeader(request);
        }
    }
}
