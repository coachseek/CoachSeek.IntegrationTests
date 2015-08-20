using System;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionDeleteTests : ScheduleTests
    {
        [Test]
        public void GivenNonExistentSessionId_WhenTryDeleteSession_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentSessionId();
            var response = WhenTryDeleteSession(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenStandaloneSessionWithoutBookings_WhenTryDeleteSession_ThenStandaloneSessionIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenStandaloneSessionWithoutBookings(setup);
            var response = WhenTryDeleteSession(id, setup);
            ThenStandaloneSessionIsDeleted(response, setup);
        }

        [Test]
        public void GivenStandaloneSessionWithBookings_WhenTryDeleteSession_ThenReturnCannotDeleteSessionError()
        {
            var setup = RegisterBusiness();

            var id = GivenStandaloneSessionWithBookings(setup);
            var response = WhenTryDeleteSession(id, setup);
            AssertSingleError(response, 
                              ErrorCodes.SessionHasBookingsCannotDelete, 
                              "Cannot delete session as it has one or more bookings.",
                              id.ToString());
        }

        [Test]
        public void GivenSessionInCourseWithoutBookings_WhenTryDeleteSession_ThenSessionInCourseIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenSessionInCourseWithoutBookings(setup);
            var response = WhenTryDeleteSession(id, setup);
            ThenSessionInCourseIsDeleted(response, setup);
        }

        // TODO: Try and delete a session with bookings.

        private Guid GivenNonExistentSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenStandaloneSessionWithoutBookings(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            
            return setup.AaronOrakeiMiniRed14To15.Id;
        }

        private Guid GivenStandaloneSessionWithBookings(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            return setup.AaronOrakeiMiniRed14To15.Id;
        }

        private Guid GivenSessionInCourseWithoutBookings(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id;
        }


        private ApiResponse WhenTryDeleteSession(Guid id, SetupData setup)
        {
            return Delete(RelativePath, id.ToString(), setup);
        }

        private ApiResponse WhenTryDeleteSessionAnonymously(Guid id)
        {
            return DeleteAnonymously(RelativePath, id.ToString());
        }


        private void ThenStandaloneSessionIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<SessionData>(RelativePath, setup.AaronOrakeiMiniRed14To15.Id, setup);
            AssertNotFound(getResponse);
        }

        private void ThenSessionInCourseIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);
             
            var getResponse = AuthenticatedGet<SessionData>(RelativePath, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup);
            AssertNotFound(getResponse);
        }
    }
}
