using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddSessionTests : BaseBookingAddTests
    {
        protected ApiBookingSaveCommand GivenNonExistentSessionAndCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(Guid.NewGuid(), Guid.NewGuid());
        }

        protected ApiBookingSaveCommand GivenThisCustomerIsAlreadyBookedOntoThisStandaloneSession(SetupData setup)
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

        protected void ThenReturnNonExistentCustomerError(ApiResponse response, Guid customerId)
        {
            AssertSingleError(response, 
                              ErrorCodes.CustomerInvalid, 
                              "This customer does not exist.",
                              customerId.ToString());
        }

        protected void ThenReturnDuplicateStandaloneSessionBookingError(ApiResponse response)
        {
            AssertSingleError(response, "This customer is already booked for this session.");
        }

        protected void ThenReturnDuplicateCourseSessionBookingError(ApiResponse response, Guid customerId, Guid sessionId)
        {
            AssertSingleError(response,
                              ErrorCodes.CustomerAlreadyBookedOntoSession,
                              "This customer is already booked for this session.",
                              string.Format("Customer: '{0}', Session: '{1}'", customerId, sessionId));
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

        protected void ThenCreateSessionOnlineBooking(ApiResponse response,
                                                      ExpectedStandaloneSession session,
                                                      ExpectedCustomer customer,
                                                      SetupData setup,
                                                      int expectedBookingCount = 1)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertSingleSessionOnlineBooking(booking, session, customer);
            var bookingId = booking.id;

            var sessionResponse = AuthenticatedGet<SessionData>("Sessions", booking.session.id, setup);
            var sessionData = AssertSuccessResponse<SessionData>(sessionResponse);

            Assert.That(sessionData.booking.bookings.Count, Is.EqualTo(expectedBookingCount));
            var bookingOne = sessionData.booking.bookings[expectedBookingCount - 1];

            AssertCustomerBooking(bookingOne, bookingId, customer);
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
