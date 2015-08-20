using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingAddStandaloneSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenMoreThanOneSession_WhenTryBookStandaloneSession_ThenReturnStandaloneSessionsAreBookedOneAtATimeError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenMoreThanOneSession(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnStandaloneSessionsAreBookedOneAtATimeError(response);
        }

        [Test]
        public void GivenNonExistentSession_WhenTryBookStandaloneSession_ThenReturnNonExistentSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCustomerFred(setup);

            var command = GivenNonExistentSession(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnNonExistentSessionError(response, command.sessions[0].id.GetValueOrDefault());
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryBookStandaloneSession_ThenReturnNonExistentCustomerError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnNonExistentCustomerError(response, command.customer.id.Value);
        }

        [Test]
        public void GivenNonExistentSessionAndCustomer_WhenTryBookStandaloneSession_ThenReturnNonExistentSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenNonExistentSessionAndCustomer(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnNonExistentSessionError(response, command.sessions[0].id.GetValueOrDefault());
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryBookStandaloneSession_ThenReturnDuplicateStandaloneSessionBookingError()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenThisCustomerIsAlreadyBookedOntoThisStandaloneSession(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnDuplicateStandaloneSessionBookingError(response, setup.Fred.Id, setup.AaronOrakeiMiniRed14To15.Id);
        }

        [Test]
        public void GivenSessionIsNotOnlineBookable_WhenTryBookStandaloneSession_ThenCreateSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed16To17(setup);
            RegisterCustomerWilma(setup);

            var command = GivenSessionIsNotOnlineBookable(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenCreateSessionBooking(response, setup.AaronOrakeiMiniRed16To17, setup.Wilma, setup);
        }

        [Test]
        public void GivenSessionIsOnlineBookable_WhenTryBookStandaloneSession_ThenCreateSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenSessionIsOnlineBookable(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenCreateSessionBooking(response, setup.AaronOrakeiMiniRed14To15, setup.Fred, setup);
        }

        [Test]
        public void GivenSessionIsFull_WhenTryBookStandaloneSession_ThenReturnSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerBambam(setup);

            var command = GivenSessionIsFull(setup);
            var response = WhenTryBookStandaloneSession(command, setup);
            ThenReturnSessionFullError(response, setup);
        }
        

        private ApiBookingSaveCommand GivenMoreThanOneSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[] { setup.AaronOrakeiMiniRed14To15.Id, Guid.NewGuid() }, setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenNonExistentSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(Guid.NewGuid(), setup.Fred.Id);
        }
        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, Guid.NewGuid());
        }

        private ApiBookingSaveCommand GivenSessionIsFull(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.BamBam.Id);
        }


        private ApiResponse WhenTryBookStandaloneSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookSession(json, setup);
        }


        private void ThenReturnStandaloneSessionsAreBookedOneAtATimeError(ApiResponse response)
        {
            AssertSingleError(response, 
                              ErrorCodes.StandaloneSessionMustBeBookedOneAtATime, 
                              "Standalone sessions must be booked one at a time.", 
                              null);
        }

        private void ThenReturnSessionFullError(ApiResponse response, SetupData setup)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionFullyBooked, 
                              "Session is already fully booked.",
                              setup.AaronOrakeiMiniRed14To15.Id.ToString());
        }
    }
}
