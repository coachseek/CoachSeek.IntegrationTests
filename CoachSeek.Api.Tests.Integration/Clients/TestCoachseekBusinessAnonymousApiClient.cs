using System.Net;
using Coachseek.API.Client.Services;
using CoachSeek.Common;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public class TestCoachseekBusinessAnonymousApiClient : CoachseekBusinessAnonymousApiClient
    {
        public TestCoachseekBusinessAnonymousApiClient(string businessDomain, 
                                                       string scheme = "https", 
                                                       ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(businessDomain, scheme, dataFormat)
        { }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            base.ModifyRequest(request);
            TestingHeaderSetter.SetTestingHeader(request);
        }
    }
}
