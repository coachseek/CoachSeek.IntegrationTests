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
    }
}
