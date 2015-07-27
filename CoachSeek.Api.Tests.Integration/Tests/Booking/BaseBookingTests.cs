using System;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingTests : ScheduleTests
    {
        protected override string RelativePath
        {
            get { return "Bookings"; }
        }


        protected void AssertSingleSessionBooking(SingleSessionBookingData booking, ExpectedStandaloneSession session, ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.InstanceOf<Guid>());
            Assert.That(booking.parentId, Is.Null);

            Assert.That(booking.session.id, Is.EqualTo(session.Id));
            Assert.That(booking.session.name, Is.EqualTo(session.Description));

            Assert.That(booking.customer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", expectedCustomer.FirstName, expectedCustomer.LastName)));

            Assert.That(booking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
        }

        protected void AssertSingleSessionOnlineBooking(SingleSessionBookingData booking, ExpectedStandaloneSession session, ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.InstanceOf<Guid>());
            Assert.That(booking.parentId, Is.Null);

            Assert.That(booking.session.id, Is.EqualTo(session.Id));
            Assert.That(booking.session.name, Is.EqualTo(session.Description));

            Assert.That(booking.customer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", expectedCustomer.FirstName, expectedCustomer.LastName)));

            Assert.That(booking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
        }
    }
}
