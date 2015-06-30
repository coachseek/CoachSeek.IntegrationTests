using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Tests.Booking;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Attendance
{
    [TestFixture]
    public class AttendanceTests : BaseBookingTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenWantToSetToHasAttended_WhenTrySetAttendance_ThenSetsHasAttendedToTrue()
        {
            var command = GivenWantToSetToHasAttended();
            WhenTrySetAttendance(command);
            ThenSetsHasAttendedTo(true);
        }

        [Test]
        public void GivenWantToSetToHasNotAttended_WhenTrySetAttendance_ThenSetsHasAttendedToFalse()
        {
            var command = GivenWantToSetToHasNotAttended();
            WhenTrySetAttendance(command);
            ThenSetsHasAttendedTo(false);
        }


        private ApiBookingSetAttendanceCommand GivenWantToSetToHasAttended()
        {
            return new ApiBookingSetAttendanceCommand { hasAttended = true };
        }

        private ApiBookingSetAttendanceCommand GivenWantToSetToHasNotAttended()
        {
            return new ApiBookingSetAttendanceCommand { hasAttended = false };
        }


        private void WhenTrySetAttendance(ApiBookingSetAttendanceCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, FredOnAaronOrakei14To15SessionId);

            WhenTrySetAttendance(json, relativePath);
        }

        protected void WhenTrySetAttendance(string json, string relativePath)
        {
            var response = new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json, 
                                                                                           Business.UserName, 
                                                                                           Business.Password,
                                                                                           relativePath);
        }


        private void ThenSetsHasAttendedTo(bool hasAttended)
        {
            var response = AuthenticatedGet<SingleSessionBookingData>("Bookings", FredOnAaronOrakei14To15SessionId);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.hasAttended, Is.EqualTo(hasAttended));
        }
    }
}
