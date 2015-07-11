using System;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Booking
{
    public class ExpectedCourseBooking
    {
        public Guid Id { get; set; }

        public IList<Guid> SessionIds { get; private set; }
        public ApiCustomerKey Customer { get; private set; }


        public ExpectedCourseBooking(IEnumerable<Guid> sessionIds, Guid customerId)
        {
            SessionIds = new List<Guid>(sessionIds);
            Customer = new ApiCustomerKey { id = customerId };
        }
    }
}
