using Coachseek.API.Client.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Payment
{
    [TestFixture]
    public class PaymentTests : WebIntegrationTest
    {
        [Test]
        public void TestApiIpnEndpoint()
        {
            var command = "hello world!";
            var response = WhenTryPostAnonymously(command);
        }


        private ApiResponse WhenTryPostAnonymously(string json)
        {
            return AnonymousPost<string>(json, RelativePath);
        }

        protected override string RelativePath
        {
            get { return "Paypal"; }
        }
    }
}
