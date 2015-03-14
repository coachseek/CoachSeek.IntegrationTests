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
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNonExistentSessionId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var id = GivenNonExistentSessionId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenExistingStandaloneSessionWithoutBookings_WhenTryDelete_ThenStandaloneSessionIsDeleted()
        {
            var id = GivenExistingStandaloneSessionWithoutBookings();
            var response = WhenTryDelete(id);
            ThenStandaloneSessionIsDeleted(response);
        }

        [Test]
        public void GivenExistingCourse_WhenTryDelete_ThenCourseIsDeleted()
        {
            var id = GivenExistingCourse();
            var response = WhenTryDelete(id);
            ThenCourseIsDeleted(response);
        }


        private Guid GivenNonExistentSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingStandaloneSessionWithoutBookings()
        {
            return AaronOrakei16To17SessionId;
        }

        private Guid GivenExistingCourse()
        {
            return BobbyRemueraHolidayCampFor2DaysCourseId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<SessionData>("Sessions", id);
        }


        private void ThenStandaloneSessionIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = Get<SessionData>("Sessions", AaronOrakei16To17SessionId);
            AssertNotFound(getResponse);

            // Other sessions are still there.
            var getResponseSomeSession = Get<SessionData>("Sessions", AaronOrakei14To15SessionId);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = Get<SessionData>("Sessions", AaronRemuera9To10For8WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
        }

        private void ThenCourseIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponseCourse = Get<SessionData>("Sessions", BobbyRemueraHolidayCampFor2DaysCourseId);
            AssertNotFound(getResponseCourse);
            var getResponseSession1 = Get<SessionData>("Sessions", BobbyRemueraHolidayCampFor2DaysSessionIds[0]);
            AssertNotFound(getResponseSession1);
            var getResponseSession2 = Get<SessionData>("Sessions", BobbyRemueraHolidayCampFor2DaysSessionIds[1]);
            AssertNotFound(getResponseSession2);

            // Other sessions are still there.
            var getResponseSomeSession = Get<SessionData>("Sessions", AaronOrakei16To17SessionId);
            AssertStatusCode(getResponseSomeSession.StatusCode, HttpStatusCode.OK);
            var getResponseSomeCourse = Get<SessionData>("Sessions", AaronRemuera9To10For8WeeksCourseId);
            AssertStatusCode(getResponseSomeCourse.StatusCode, HttpStatusCode.OK);
        }
    }
}
