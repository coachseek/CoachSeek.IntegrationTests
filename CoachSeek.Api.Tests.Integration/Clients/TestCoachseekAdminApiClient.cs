using System.Net;
using Coachseek.API.Client.Services;
using CoachSeek.Common;

namespace CoachSeek.Api.Tests.Integration.Clients
{
    public class TestCoachseekAdminApiClient : CoachseekAdminApiClient
    {
        public TestCoachseekAdminApiClient(string scheme = "https", ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(scheme, dataFormat)
        { }

        protected override void ModifyRequest(HttpWebRequest request)
        {
            base.ModifyRequest(request);
            TestingHeaderSetter.SetTestingHeader(request);
        }
    }
}
