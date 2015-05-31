using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    [TestFixture]
    public class BusinessGetTests : BusinessTests
    {
        [Test]
        public void WhenGetBusiness_ThenReturnBusiness()
        {
            var response = WhenGetBusiness();
            ThenReturnBusiness(response);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenGetBusinessForOnlineBooking_ThenReturnNotAuthorised()
        {
            GivenNonExistentBusiness();
            var response = WhenGetBusinessForOnlineBooking();
            ThenReturnNotAuthorised(response);
        }

        [Test]
        public void GivenValidBusiness_WhenGetBusinessForOnlineBooking_ThenReturnBusiness()
        {
            GivenValidBusiness();
            var response = WhenGetBusinessForOnlineBooking();
            ThenReturnBusiness(response);
        }


        private void GivenNonExistentBusiness()
        {
            Business.Domain = "abc123";
        }

        private void GivenValidBusiness()
        {
            // Nothing to do.
        }


        private Response WhenGetBusiness()
        {
            return AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
        }

        private Response WhenGetBusinessForOnlineBooking()
        {
            return GetAnonymously<BusinessData>(OnlineBookingUrl.AbsoluteUri);
        }


        private void ThenReturnBusiness(Response response)
        {
            var business = AssertSuccessResponse<BusinessData>(response);

            Assert.That(business.id, Is.EqualTo(Business.Id));
            Assert.That(business.name, Is.EqualTo(Business.Name));
            Assert.That(business.domain, Is.EqualTo(Business.Domain));
        }

        private void ThenReturnNotAuthorised(Response response)
        {
            AssertUnauthorised(response);
        }


        protected string BuildGetBusinessUrl()
        {
            return Url.AbsoluteUri;
        }
    }
}
