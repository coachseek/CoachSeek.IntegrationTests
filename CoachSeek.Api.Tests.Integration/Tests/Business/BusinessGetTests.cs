using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    [TestFixture]
    public class BusinessGetTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Business"; }
        }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
        }


        [Test]
        public void WhenGetBusiness_ThenReturnBusiness()
        {
            var response = WhenGetBusiness();
            ThenReturnBusiness(response);
        }


        private Response WhenGetBusiness()
        {
            var url = BuildGetBusinessUrl();
            return AuthenticatedGet<BusinessData>(url);
        }

        private void ThenReturnBusiness(Response response)
        {
            var business = AssertSuccessResponse<BusinessData>(response);

            Assert.That(business.id, Is.EqualTo(Business.Id));
            Assert.That(business.name, Is.EqualTo(Business.Name));
            Assert.That(business.domain, Is.EqualTo(Business.Domain));
        }


        protected string BuildGetBusinessUrl()
        {
            return string.Format("{0}/{1}", BaseUrl, RelativePath);
        }
    }
}
