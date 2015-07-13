using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddSessionTests : BaseBookingAddTests
    {
        protected ApiBookingSaveCommand GivenNonExistentSessionAndCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(Guid.NewGuid(), Guid.NewGuid());
        }

        protected ApiBookingSaveCommand GivenThisCustomerIsAlreadyBookedOntoThisSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSessionIsOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSessionIsNotOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed16To17.Id, setup.Wilma.Id);
        }


        protected void ThenReturnNonExistentSessionError(ApiResponse response)
        {
            AssertSingleError(response, "This session does not exist.");
        }

        protected void ThenReturnNonExistentCustomerError(ApiResponse response)
        {
            AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
        }

        protected void ThenReturnDuplicateBookingError(ApiResponse response)
        {
            AssertSingleError(response, "This customer is already booked for this session.");
        }

        protected void ThenCreateSessionBooking(ApiResponse response, 
                                                ExpectedStandaloneSession session, 
                                                ExpectedCustomer customer, 
                                                SetupData setup,
                                                int expectedBookingCount = 1)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertSingleSessionBooking(booking, session, customer);
            var bookingId = booking.id;

            var sessionResponse = AuthenticatedGet<SessionData>("Sessions", booking.session.id, setup);
            var sessionData = AssertSuccessResponse<SessionData>(sessionResponse);

            Assert.That(sessionData.booking.bookings.Count, Is.EqualTo(expectedBookingCount));
            var bookingOne = sessionData.booking.bookings[expectedBookingCount - 1];

            AssertCustomerBooking(bookingOne, bookingId, customer);
        }

        private void AssertSingleSessionBooking(SingleSessionBookingData booking, ExpectedStandaloneSession session, ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.InstanceOf<Guid>());
            Assert.That(booking.parentId, Is.Null);

            Assert.That(booking.session.id, Is.EqualTo(session.Id));
            Assert.That(booking.session.name, Is.EqualTo(session.Description));

            Assert.That(booking.customer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", expectedCustomer.FirstName, expectedCustomer.LastName)));
        }

        private void AssertCustomerBooking(CustomerBookingData booking, Guid expectedBookingId, ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.EqualTo(expectedBookingId));

            var bookingCustomer = booking.customer;
            Assert.That(bookingCustomer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(bookingCustomer.firstName, Is.EqualTo(expectedCustomer.FirstName));
            Assert.That(bookingCustomer.lastName, Is.EqualTo(expectedCustomer.LastName));
            Assert.That(bookingCustomer.email, Is.EqualTo(expectedCustomer.Email));
            Assert.That(bookingCustomer.phone, Is.EqualTo(expectedCustomer.Phone));
        }
    }
}
