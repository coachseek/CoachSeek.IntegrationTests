using System;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Booking
{
    public class ExpectedBooking
    {
        public Guid Id { get; set; }

        public ApiSessionKey Session { get; private set; }
        public ApiCustomerKey Customer { get; private set; }
        public bool? HasAttended { get; private set; }
        public string PaymentStatus { get; private set; }


        public ExpectedBooking(Guid sessionId, Guid customerId)
        {
            Session = new ApiSessionKey { id = sessionId };
            Customer = new ApiCustomerKey { id = customerId };
            HasAttended = null;
            PaymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE;
        }


        public void Assert(CustomerBookingData actualBooking, Guid sessionId)
        {
            NUnit.Framework.Assert.That(sessionId, Is.EqualTo(Session.id));

            NUnit.Framework.Assert.That(actualBooking.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualBooking.parentId, Is.Null);
            NUnit.Framework.Assert.That(actualBooking.customer.id, Is.EqualTo(Customer.id));
            NUnit.Framework.Assert.That(actualBooking.hasAttended, Is.EqualTo(HasAttended));
            NUnit.Framework.Assert.That(actualBooking.paymentStatus, Is.EqualTo(PaymentStatus));
        }
    }
}
