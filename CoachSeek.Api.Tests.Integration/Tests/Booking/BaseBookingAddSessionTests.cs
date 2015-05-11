using System;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddSessionTests : BaseBookingAddTests
    {
        protected ApiBookingSaveCommand GivenNonExistentSession()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = Guid.NewGuid() },
                customer = new ApiCustomerKey { id = Fred.Id }
            };
        }

        protected ApiBookingSaveCommand GivenNonExistentCustomer()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronOrakei14To15.Id },
                customer = new ApiCustomerKey { id = Guid.NewGuid() }
            };
        }

        protected ApiBookingSaveCommand GivenNonExistentSessionAndCustomer()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = Guid.NewGuid() },
                customer = new ApiCustomerKey { id = Guid.NewGuid() }
            };
        }

        protected ApiBookingSaveCommand GivenThisCustomerIsAlreadyBookedOntoThisSession()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronOrakei14To15.Id },
                customer = new ApiCustomerKey { id = Fred.Id }
            };
        }

        protected ApiBookingSaveCommand GivenSessionIsOnlineBookable()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronOrakei14To15.Id },
                customer = new ApiCustomerKey { id = BamBam.Id }
            };
        }

        protected ApiBookingSaveCommand GivenSessionIsNotOnlineBookable()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronOrakei16To17.Id },
                customer = new ApiCustomerKey { id = Wilma.Id }
            };
        }


        protected void ThenReturnNonExistentSessionError(Response response)
        {
            AssertSingleError(response, "This session does not exist.", "booking.session.id");
        }

        protected void ThenReturnNonExistentCustomerError(Response response)
        {
            AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
        }

        protected void ThenReturnDuplicateBookingError(Response response)
        {
            AssertSingleError(response, "This customer is already booked for this session.");
        }

        protected void ThenCreateSessionBooking(Response response, ExpectedStandaloneSession session, ExpectedCustomer customer, int expectedBookingCount = 1)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertSingleSessionBooking(booking, session, customer);
            var bookingId = booking.id;

            var sessionResponse = AuthenticatedGet<SessionData>("Sessions", booking.session.id);
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
