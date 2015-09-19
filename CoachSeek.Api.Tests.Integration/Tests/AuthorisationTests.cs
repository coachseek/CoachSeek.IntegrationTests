using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class AuthorisationTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Business"; }
        }

        [Test]
        public void GivenTrialHasExired_WhenTryAndAccessSystemAuthenticated_ThenReturnForbidden()
        {
            RegisterExpiredBusinessIfNotExists();
            var setup = CreateExpiredBusinessSetupData();

            var response = WhenTryAndAccessSystemAuthenticated(setup);
            ThenReturnForbidden(response);
        }


        private ApiResponse WhenTryAndAccessSystemAuthenticated(SetupData setup)
        {
            return AuthenticatedGet<BusinessData>(RelativePath, setup);
        }

        private ApiResponse WhenTryAndAccessSystemAnonymously()
        {
            return BusinessAnonymousGet<BusinessData>("OnlineBooking/Business", "expiredbusiness");
        }

        private void ThenReturnForbidden(ApiResponse response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.Forbidden);
        }

        private void ThenReturnBusiness(ApiResponse response, SetupData setup)
        {
            var business = AssertSuccessResponse<BusinessData>(response);

            Assert.That(business.id, Is.EqualTo(setup.Business.Id));
            Assert.That(business.name, Is.EqualTo(setup.Business.Name));
            Assert.That(business.domain, Is.EqualTo(setup.Business.Domain));
            Assert.That(business.sport, Is.EqualTo(setup.Business.Sport));
            AssertDateTime(business.authorisedUntil, setup.Business.AuthorisedUntil);
            Assert.That(business.payment.currency, Is.EqualTo(setup.Business.Payment.currency));
            Assert.That(business.payment.isOnlinePaymentEnabled, Is.EqualTo(setup.Business.Payment.isOnlinePaymentEnabled));
            Assert.That(business.payment.forceOnlinePayment, Is.EqualTo(setup.Business.Payment.forceOnlinePayment));
            Assert.That(business.payment.paymentProvider, Is.EqualTo(setup.Business.Payment.paymentProvider));
            Assert.That(business.payment.merchantAccountIdentifier, Is.EqualTo(setup.Business.Payment.merchantAccountIdentifier));
        }


        private SetupData CreateExpiredBusinessSetupData()
        {
            return new SetupData(new ExpectedBusiness("Expired Business", "", "", "", "", "expired.business@coachseek.com", "", "password"));
        }
    }
}
