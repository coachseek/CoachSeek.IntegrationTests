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
