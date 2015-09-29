using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddTests : BaseBookingTests
    {
        // Session
        protected ApiResponse WhenTryBookSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookSession(json, setup);
        }

        protected ApiResponse WhenTryBookSessionAnonymously(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return BusinessAnonymousPost<SingleSessionBookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryBookSession(string json, SetupData setup)
        {
            return AuthenticatedPost<SingleSessionBookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryBookCourse(string json, SetupData setup)
        {
            return AuthenticatedPost<CourseBookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryOnlineBookSession(string json, SetupData setup)
        {
            return BusinessAnonymousPost<SingleSessionBookingData>(json, "OnlineBooking/Bookings", setup);
        }

        protected ApiResponse WhenTryOnlineBookCourse(string json, SetupData setup)
        {
            return BusinessAnonymousPost<CourseBookingData>(json, "OnlineBooking/Bookings", setup);
        }


        protected void AssertCreateSingleSessionBooking(SingleSessionBookingData booking,
                                                        ExpectedStandaloneSession session,
                                                        ExpectedCustomer customer)
        {
            AssertCreateSingleSessionBookingData(booking, session, customer, Constants.PAYMENT_STATUS_PENDING_INVOICE);
        }

        protected void AssertCreateSingleSessionOnlineBooking(SingleSessionBookingData booking,
                                                              ExpectedStandaloneSession session,
                                                              ExpectedCustomer customer)
        {
            AssertCreateSingleSessionBookingData(booking, session, customer, Constants.PAYMENT_STATUS_PENDING_PAYMENT);
        }

        private void AssertCreateSingleSessionBookingData(SingleSessionBookingData booking,
                                                          ExpectedStandaloneSession session,
                                                          ExpectedCustomer customer,
                                                          string paymentStatus)
        {
            Assert.That(booking.id, Is.InstanceOf<Guid>());
            Assert.That(booking.parentId, Is.Null);

            Assert.That(booking.session.id, Is.EqualTo(session.Id));
            Assert.That(booking.session.name, Is.EqualTo(session.Description));

            Assert.That(booking.customer.id, Is.EqualTo(customer.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", customer.FirstName, customer.LastName)));

            Assert.That(booking.paymentStatus, Is.EqualTo(paymentStatus));
            Assert.That(booking.hasAttended, Is.Null);
        }

    }
}
