using System;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Booking
{
    public class ExpectedBooking
    {
        public Guid Id { get; set; }

        public ApiSessionKey Session { get; private set; }
        public ApiCustomerKey Customer { get; private set; }


        public ExpectedBooking(Guid sessionId, Guid customerId)
        {
            Session = new ApiSessionKey { id = sessionId };
            Customer = new ApiCustomerKey { id = customerId };
        }


        public void Assert(BookingData actualBooking)
        {
            NUnit.Framework.Assert.That(actualBooking.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualBooking.session.id, Is.EqualTo(Session.id));
            NUnit.Framework.Assert.That(actualBooking.customer.id, Is.EqualTo(Customer.id));
        }

        public void Assert(CustomerBookingData actualBooking, Guid sessionId)
        {
            NUnit.Framework.Assert.That(actualBooking.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(sessionId, Is.EqualTo(Session.id));
            NUnit.Framework.Assert.That(actualBooking.customer.id, Is.EqualTo(Customer.id));
        }
    }
}
