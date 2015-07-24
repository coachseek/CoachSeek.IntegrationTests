using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingAddStandaloneSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenMoreThanOneSession_WhenTryOnlineBookStandaloneSession_ThenReturnStandaloneSessionsAreBookedOneAtATimeError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenMoreThanOneSession(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnStandaloneSessionsAreBookedOneAtATimeError(response);
        }

        [Test]
        public void GivenNonExistentSession_WhenTryOnlineBookStandaloneSession_ThenReturnNonExistentSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCustomerFred(setup);

            var command = GivenNonExistentSession(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryOnlineBookStandaloneSession_ThenReturnNonExistentCustomerError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnNonExistentCustomerError(response);
        }

        [Test]
        public void GivenNonExistentSessionAndCustomer_WhenTryOnlineBookStandaloneSession_ThenReturnNonExistentSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenNonExistentSessionAndCustomer(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryOnlineBookStandaloneSession_ThenReturnDuplicateStandaloneSessionBookingError()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenThisCustomerIsAlreadyBookedOntoThisStandaloneSession(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnDuplicateStandaloneSessionBookingError(response);
        }

        [Test]
        public void GivenSessionIsNotOnlineBookable_WhenTryOnlineBookStandaloneSession_ThenReturnSessionNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed16To17(setup);
            RegisterCustomerWilma(setup);

            var command = GivenSessionIsNotOnlineBookable(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnSessionNotOnlineBookableError(response);
        }

        [Test]
        public void GivenSessionIsOnlineBookable_WhenTryOnlineBookStandaloneSession_ThenCreateSessionOnlineBooking()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenSessionIsOnlineBookable(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenCreateSessionOnlineBooking(response, setup.AaronOrakeiMiniRed14To15, setup.Fred, setup);
        }

        [Test]
        public void GivenSessionIsFull_WhenTryOnlineBookStandaloneSession_ThenReturnSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerBambam(setup);

            var command = GivenSessionIsFull(setup);
            var response = WhenTryOnlineBookStandaloneSession(command, setup);
            ThenReturnSessionFullError(response);
        }
        

        private ApiBookingSaveCommand GivenMoreThanOneSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[] { setup.AaronOrakeiMiniRed14To15.Id, Guid.NewGuid() }, setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenNonExistentSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(Guid.NewGuid(), setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, Guid.NewGuid());
        }

        private ApiBookingSaveCommand GivenSessionIsFull(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.BamBam.Id);
        }


        private ApiResponse WhenTryOnlineBookStandaloneSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryOnlineBookSession(json, setup);
        }


        private void ThenReturnStandaloneSessionsAreBookedOneAtATimeError(ApiResponse response)
        {
            AssertSingleError(response, "Standalone sessions must be booked one at a time.", "booking.sessions");
        }

        private void ThenReturnSessionNotOnlineBookableError(ApiResponse response)
        {
            AssertSingleError(response, "A session is not online bookable.", "booking.sessions");
        }

        private void ThenReturnSessionFullError(ApiResponse response)
        {
            AssertSingleError(response, "This session is already fully booked.");
        }
    }
}
