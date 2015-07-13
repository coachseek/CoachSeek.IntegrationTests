using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionCreateNewTests : ScheduleTests
    {
        [Test]
        public void GivenNewSessionClashesWithExistingStandaloneSession_WhenTryCreateSession_ThenReturnSessionClashErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);

            var command = GivenNewSessionClashesWithExistingStandaloneSession(setup);
            var response = WhenTryCreateSession(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiMiniRed16To17.Id);
        }

        [Test]
        public void GivenNewSessionClashesWithExistingCourse_WhenTryCreateSession_ThenReturnSessionClashErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewSessionClashesWithExistingCourse(setup);
            var response = WhenTryCreateSession(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
        }

        [Test]
        public void GivenNewSessionWithZeroSessionCount_WhenTryCreateSession_ThenReturnInvalidSessionCountErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenNewSessionWithZeroSessionCount(setup);
            var response = WhenTryCreateSession(command, setup);
            AssertSingleError(response, "The sessionCount field is not valid.", "session.repetition.sessionCount");
        }

        [Test]
        public void GivenNewValidSession_WhenTryCreateSession_ThenCreateSession()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenNewValidSession(setup);
            var response = WhenTryCreateSession(command, setup);
            ThenCreateNewSession(response, setup);
        }


        private ApiSessionSaveCommand GivenNewSessionClashesWithExistingStandaloneSession(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed16To17);
            command.timing.startTime = "16:30";

            return command;
        }

        private ApiSessionSaveCommand GivenNewSessionClashesWithExistingCourse(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(15);

            return command;
        }
         
        private ApiSessionSaveCommand GivenNewValidSession(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(14);

            return command;
        }

        private ApiSessionSaveCommand GivenNewSessionWithZeroSessionCount(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.repetition.sessionCount = 0;

            return command;
        }

        private SessionData ThenCreateNewSession(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));

            AssertSessionLocation(session.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(session.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(session.service, setup.MiniRed.Id, setup.MiniRed.Name);

            AssertSessionTiming(session.timing, GetDateFormatNumberOfDaysOut(14), "14:00", 60);
            AssertSessionBooking(session.booking, 3, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            return session;
        }
    }
}
