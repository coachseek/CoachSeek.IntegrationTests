using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Tests.Booking;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Attendance
{
    [TestFixture]
    public class AttendanceTests : BaseBookingTests
    {
        [Test]
        public void GivenWantToSetToHasAttended_WhenTrySetAttendance_ThenSetsHasAttendedToTrue()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetToHasAttended();
            WhenTrySetAttendance(command, setup);
            ThenSetsHasAttendedTo(true, setup);
        }

        [Test]
        public void GivenWantToSetToHasNotAttended_WhenTrySetAttendance_ThenSetsHasAttendedToFalse()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetToHasNotAttended();
            WhenTrySetAttendance(command, setup);
            ThenSetsHasAttendedTo(false, setup);
        }


        private ApiBookingSetAttendanceCommand GivenWantToSetToHasAttended()
        {
            return new ApiBookingSetAttendanceCommand { hasAttended = true };
        }

        private ApiBookingSetAttendanceCommand GivenWantToSetToHasNotAttended()
        {
            return new ApiBookingSetAttendanceCommand { hasAttended = false };
        }


        private void WhenTrySetAttendance(ApiBookingSetAttendanceCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);

            WhenTrySetAttendance(json, relativePath, setup);
        }

        protected void WhenTrySetAttendance(string json, string relativePath, SetupData setup)
        {
            var response = AuthenticatedPost<SingleSessionBookingData>(json, relativePath, setup);
        }


        private void ThenSetsHasAttendedTo(bool hasAttended, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);
            var response = AuthenticatedGet<SingleSessionBookingData>(url, setup);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.hasAttended, Is.EqualTo(hasAttended));
        }
    }
}
