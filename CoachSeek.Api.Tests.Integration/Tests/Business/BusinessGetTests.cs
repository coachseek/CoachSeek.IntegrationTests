using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
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
            var setup = RegisterBusiness();

            var response = WhenGetBusiness(setup);
            ThenReturnBusiness(response, setup);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenGetBusinessForOnlineBooking_ThenReturnNotAuthorised()
        {
            var domain = GivenNonExistentBusiness();
            var response = WhenGetBusinessForOnlineBooking(domain);
            ThenReturnUnauthorised(response);
        }

        [Test]
        public void GivenValidBusiness_WhenGetBusinessForOnlineBooking_ThenReturnBusiness()
        {
            var setup = RegisterBusiness();

            GivenValidBusiness();
            var response = WhenGetBusinessForOnlineBooking(setup.Business.Domain);
            ThenReturnBusiness(response, setup);
        }


        private string GivenNonExistentBusiness()
        {
            return "abc123";
        }

        private void GivenValidBusiness()
        {
            // Nothing to do.
        }


        private ApiResponse WhenGetBusiness(SetupData setup)
        {
            return new TestAuthenticatedApiClient().Get<BusinessData>(setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      RelativePath);
        }

        private ApiResponse WhenGetBusinessForOnlineBooking(string businessDomain)
        {
            return new TestBusinessAnonymousApiClient().Get<BusinessData>(businessDomain,
                                                                          "OnlineBooking/Business");
        }


        private void ThenReturnBusiness(ApiResponse response, SetupData setup)
        {
            var business = AssertSuccessResponse<BusinessData>(response);

            Assert.That(business.id, Is.EqualTo(setup.Business.Id));
            Assert.That(business.name, Is.EqualTo(setup.Business.Name));
            Assert.That(business.domain, Is.EqualTo(setup.Business.Domain));
            Assert.That(business.payment.currency, Is.EqualTo(setup.Business.Payment.currency));
            Assert.That(business.payment.isOnlinePaymentEnabled, Is.EqualTo(setup.Business.Payment.isOnlinePaymentEnabled));
            Assert.That(business.payment.forceOnlinePayment, Is.EqualTo(setup.Business.Payment.forceOnlinePayment));
            Assert.That(business.payment.paymentProvider, Is.EqualTo(setup.Business.Payment.paymentProvider));
            Assert.That(business.payment.merchantAccountIdentifier, Is.EqualTo(setup.Business.Payment.merchantAccountIdentifier));
        }

        private void ThenReturnUnauthorised(ApiResponse response)
        {
            AssertUnauthorised(response);
        }
    }
}
