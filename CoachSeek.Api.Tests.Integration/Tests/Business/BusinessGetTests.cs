﻿using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    [TestFixture]
    public class BusinessGetTests : BusinessTests
    {
        [Test]
        public void WhenGetBusiness_ThenReturnBusinessWithStatistics()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterTestCourses(setup);

            var response = WhenGetBusiness(setup);
            ThenReturnBusinessWithStatistics(response, setup);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenGetBusinessForOnlineBooking_ThenReturnNotAuthorised()
        {
            var domain = GivenNonExistentBusiness();
            var response = WhenGetBusinessForOnlineBooking(domain);
            ThenReturnUnauthorised(response);
        }

        [Test]
        public void GivenValidBusiness_WhenGetBusinessForOnlineBooking_ThenReturnBasicBusiness()
        {
            var setup = RegisterBusiness();

            GivenValidBusiness();
            var response = WhenGetBusinessForOnlineBooking(setup.Business.Domain);
            ThenReturnBasicBusiness(response, setup);
        }


        private string GivenNonExistentBusiness()
        {
            return Random.RandomString;
        }

        private void GivenValidBusiness()
        {
            // Nothing to do.
        }


        private ApiResponse WhenGetBusiness(SetupData setup)
        {
            return AuthenticatedGet<BusinessData>(RelativePath, setup);
        }

        private ApiResponse WhenGetBusinessForOnlineBooking(string businessDomain)
        {
            return BusinessAnonymousGet<BusinessData>("OnlineBooking/Business", businessDomain);
        }


        private void ThenReturnBusinessWithStatistics(ApiResponse response, SetupData setup)
        {
            var business = AssertSuccessResponse<BusinessData>(response);

            Assert.That(business.id, Is.EqualTo(setup.Business.Id));
            Assert.That(business.name, Is.EqualTo(setup.Business.Name));
            Assert.That(business.domain, Is.EqualTo(setup.Business.Domain));
            Assert.That(business.sport, Is.EqualTo(setup.Business.Sport));
            AssertDateTime(business.authorisedUntil, setup.Business.AuthorisedUntil);
            Assert.That(business.subscriptionPlan, Is.EqualTo(setup.Business.SubscriptionPlan));
            Assert.That(business.payment.currency, Is.EqualTo(setup.Business.Payment.currency));
            Assert.That(business.payment.isOnlinePaymentEnabled, Is.EqualTo(setup.Business.Payment.isOnlinePaymentEnabled));
            Assert.That(business.payment.forceOnlinePayment, Is.EqualTo(setup.Business.Payment.forceOnlinePayment));
            Assert.That(business.payment.paymentProvider, Is.EqualTo(setup.Business.Payment.paymentProvider));
            Assert.That(business.payment.merchantAccountIdentifier, Is.EqualTo(setup.Business.Payment.merchantAccountIdentifier));
            Assert.That(business.statistics.totalNumberOfSessions, Is.EqualTo(8));
        }

        private void ThenReturnBasicBusiness(ApiResponse response, SetupData setup)
        {
            var business = AssertSuccessResponse<BasicBusinessData>(response);

            Assert.That(business.id, Is.EqualTo(setup.Business.Id));
            Assert.That(business.name, Is.EqualTo(setup.Business.Name));
            Assert.That(business.domain, Is.EqualTo(setup.Business.Domain));
            Assert.That(business.sport, Is.EqualTo(setup.Business.Sport));
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
