﻿using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingAddSessionTests : BaseBookingAddSessionTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }

        
        [Test]
        public void GivenNonExistentSession_WhenTryBookOnlineSession_ThenReturnNonExistentSessionError()
        {
            var command = GivenNonExistentSession();
            var response = WhenTryBookOnlineSession(command);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryBookOnlineSession_ThenReturnNonExistentCustomerError()
        {
            var command = GivenNonExistentCustomer();
            var response = WhenTryBookOnlineSession(command);
            ThenReturnNonExistentCustomerError(response);
        }

        [Test]
        public void GivenNonExistentSessionAndCustomer_WhenTryBookOnlineSession_ThenReturnNonExistentSessionErrorOnly()
        {
            var command = GivenNonExistentSessionAndCustomer();
            var response = WhenTryBookOnlineSession(command);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryBookOnlineSession_ThenReturnDuplicateBookingError()
        {
            var command = GivenThisCustomerIsAlreadyBookedOntoThisSession();
            var response = WhenTryBookOnlineSession(command);
            ThenReturnDuplicateBookingError(response);
        }

        [Test]
        public void GivenSessionIsNotOnlineBookable_WhenTryBookOnlineSession_ThenReturnNotOnlineBookableError()
        {
            var command = GivenSessionIsNotOnlineBookable();
            var response = WhenTryBookOnlineSession(command);
            ThenReturnSessionNotOnlineBookableError(response);
        }

        [Test]
        public void GivenSessionIsOnlineBookable_WhenTryBookOnlineSession_ThenCreateSessionBooking()
        {
            BamBam = new CustomerBamBam();
            CustomerRegistrar.RegisterCustomer(BamBam, Business);

            var command = GivenSessionIsOnlineBookable();
            var response = WhenTryBookOnlineSession(command);
            ThenCreateSessionBooking(response, AaronOrakei14To15, BamBam, 3);
        }


        private void ThenReturnSessionNotOnlineBookableError(Response response)
        {
            AssertSingleError(response, "This session is not online bookable.", "booking.session");
        }
    }
}
