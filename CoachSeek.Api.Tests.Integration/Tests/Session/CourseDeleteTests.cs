using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseDeleteTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        //[Test]
        //public void GivenNonExistentCourseId_WhenTryDelete_ThenReturnNotFound()
        //{
        //    var id = GivenNonExistentCourseId();
        //    var response = WhenTryDelete(id);
        //    AssertNotFound(response);
        //}

        [Test]
        public void GivenCourseWithoutBookings_WhenTryDelete_ThenCourseIsDeleted()
        {
            var id = GivenCourseWithoutBookings();
            var response = WhenTryDelete(id);
            ThenCourseIsDeleted(response);
        }

        [Test]
        public void GivenCourseWithBookings_WhenTryDelete_ThenReturnCannotDeleteCourseError()
        {
            var id = GivenCourseWithBookings();
            var response = WhenTryDelete(id);
            AssertSingleError(response, "Cannot delete course as it has one or more bookings.");
        }


        private Guid GivenCourseWithoutBookings()
        {
            return BobbyRemueraHolidayCampFor3DaysCourseId;
        }

        private Guid GivenCourseWithBookings()
        {
            return AaronOrakeiMiniBlueFor2DaysCourseId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<SessionData>("Sessions", id);
        }


        private void ThenCourseIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponseCourse = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysCourseId);
            AssertNotFound(getResponseCourse);
            var getResponseSession1 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[0]);
            AssertNotFound(getResponseSession1);
            var getResponseSession2 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[1]);
            AssertNotFound(getResponseSession2);
            var getResponseSession3 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[2]);
            AssertNotFound(getResponseSession3);

            // Other sessions are still there.
            var getResponseSomeSession = AuthenticatedGet<SessionData>("Sessions", AaronOrakeiMiniRed16To17.Id);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = AuthenticatedGet<SessionData>("Sessions", AaronRemuera9To10For4WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
        }
    }
}
