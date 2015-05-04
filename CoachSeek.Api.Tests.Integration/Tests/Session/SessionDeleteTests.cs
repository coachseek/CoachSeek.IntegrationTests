using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionDeleteTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenNonExistentSessionId_WhenTryDelete_ThenReturnNotFound()
        {
            var id = GivenNonExistentSessionId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenStandaloneSessionWithoutBookings_WhenTryDelete_ThenStandaloneSessionIsDeleted()
        {
            var id = GivenStandaloneSessionWithoutBookings();
            var response = WhenTryDelete(id);
            ThenStandaloneSessionIsDeleted(response);
        }

        [Test]
        public void GivenStandaloneSessionWithBookings_WhenTryDelete_ThenReturnCannotDeleteSessionError()
        {
            var id = GivenStandaloneSessionWithBookings();
            var response = WhenTryDelete(id);
            AssertSingleError(response, "Cannot delete session as it has one or more bookings.");
        }

        [Test]
        public void GivenCourseWithoutBookings_WhenTryDelete_ThenCourseIsDeleted()
        {
            var id = GivenCourseWithoutBookings();
            var response = WhenTryDelete(id);
            ThenCourseIsDeleted(response);
        }

        [Test]
        public void GivenSessionInCourseWithoutBookings_WhenTryDelete_ThenSessionInCourseIsDeleted()
        {
            var id = GivenSessionInCourseWithoutBookings();
            var response = WhenTryDelete(id);
            ThenSessionInCourseIsDeleted(response);
        }

        // TODO: Try and delete a session with bookings.

        private Guid GivenNonExistentSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenStandaloneSessionWithoutBookings()
        {
            return AaronOrakei16To17.Id;
        }

        private Guid GivenStandaloneSessionWithBookings()
        {
            return AaronOrakei14To15.Id;
        }

        private Guid GivenCourseWithoutBookings()
        {
            return BobbyRemueraHolidayCampFor3DaysCourseId;
        }

        private Guid GivenSessionInCourseWithoutBookings()
        {
            return BobbyRemueraHolidayCampFor3DaysSessionIds[1];
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<SessionData>("Sessions", id);
        }


        private void ThenStandaloneSessionIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<SessionData>("Sessions", AaronOrakei16To17.Id);
            AssertNotFound(getResponse);

            // Other sessions are still there.
            var getResponseSomeSession = AuthenticatedGet<SessionData>("Sessions", AaronOrakei14To15.Id);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = AuthenticatedGet<SessionData>("Sessions", AaronRemuera9To10For4WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
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
            var getResponseSomeSession = AuthenticatedGet<SessionData>("Sessions", AaronOrakei16To17.Id);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = AuthenticatedGet<SessionData>("Sessions", AaronRemuera9To10For4WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
        }

        private void ThenSessionInCourseIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponseSession2 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[1]);
            AssertNotFound(getResponseSession2);

            // Other sessions/courses are still there.
            var getResponseCourse = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysCourseId);
            AssertStatusCode(getResponseCourse.StatusCode, HttpStatusCode.OK);
            var getResponseSession1 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[0]);
            AssertStatusCode(getResponseSession1.StatusCode, HttpStatusCode.OK);
            var getResponseSession3 = AuthenticatedGet<SessionData>("Sessions", BobbyRemueraHolidayCampFor3DaysSessionIds[2]);
            AssertStatusCode(getResponseSession3.StatusCode, HttpStatusCode.OK);
            var getResponseSomeSession = AuthenticatedGet<SessionData>("Sessions", AaronOrakei16To17.Id);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = AuthenticatedGet<SessionData>("Sessions", AaronRemuera9To10For4WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
        }
    }
}
