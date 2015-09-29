using System.Net;
using Coachseek.API.Client.Services;
using CoachSeek.Common;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public class TestCoachseekAnonymousApiClient : CoachseekAnonymousApiClient
    {
        public TestCoachseekAnonymousApiClient(string scheme = "https", ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(scheme, dataFormat)
        { }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            base.ModifyRequest(request);
            TestingHeaderSetter.SetTestingHeader(request);
        }
    }
}
