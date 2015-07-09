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
            var json = JsonConvert.SerializeObject(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);

            WhenTrySetAttendance(json, relativePath, setup);
        }

        protected void WhenTrySetAttendance(string json, string relativePath, SetupData setup)
        {
            var response = new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json,
                                                                                           setup.Business.UserName,
                                                                                           setup.Business.Password,
                                                                                           relativePath);
        }


        private void ThenSetsHasAttendedTo(bool hasAttended, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);
            var response = new TestAuthenticatedApiClient().Get<SingleSessionBookingData>(setup.Business.UserName,
                                                                                          setup.Business.Password, 
                                                                                          url);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.hasAttended, Is.EqualTo(hasAttended));
        }
    }
}
