using System;

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
    }
}
