using System;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseDeleteTests : ScheduleTests
    {
        [Test]
        public void GivenNonExistentCourseId_WhenTryDeleteCourse_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentCourseId();
            var response = WhenTryDeleteCourse(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenCourseWithoutBookings_WhenTryDeleteCourse_ThenCourseIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenCourseWithoutBookings(setup);
            var response = WhenTryDeleteCourse(id, setup);
            ThenCourseIsDeleted(response, setup);
        }

        [Test]
        public void GivenCourseWithBookings_WhenTryDelete_ThenReturnCannotDeleteCourseError()
        {
            var setup = RegisterBusiness();

            var id = GivenCourseWithBookings(setup);
            var response = WhenTryDeleteCourse(id, setup);
            AssertSingleError(response,
                              ErrorCodes.CourseHasBookingsCannotDelete, 
                              "Cannot delete course as it has one or more bookings.",
                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id.ToString());
        }


        private Guid GivenNonExistentCourseId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenCourseWithoutBookings(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
        }

        private Guid GivenCourseWithBookings(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
        }


        private ApiResponse WhenTryDeleteCourse(Guid id, SetupData setup)
        {
            return AuthenticatedDelete(RelativePath, id.ToString(), setup);
        }


        private void ThenCourseIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<SessionData>(RelativePath, setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            AssertNotFound(getResponse);
        }
    }
}
