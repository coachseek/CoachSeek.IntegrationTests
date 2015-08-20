using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionUpdateExistingTests : ScheduleTests
    {
        [Test]
        public void GivenNonExistentSessionId_WhenTryUpdateSession_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenNonExistentSessionId(setup);
            var response = WhenTryUpdateSession(command, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenExistingSessionToBeUpdatedClashesWithItself_WhenTryUpdateSession_ThenSelfClashingSessionWillBeUpdated()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenExistingSessionToBeUpdatedClashesWithItself(setup);
            var response = WhenTryUpdateSession(command, setup);
            ThenSelfClashingSessionWillBeUpdated(response, setup);
        }

        [Test]
        public void GivenExistingSessionWillNotClash_WhenTryUpdateSession_ThenSessionWillBeUpdated()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenExistingSessionWillNotClash(setup);
            var response = WhenTryUpdateSession(command, setup);
            ThenSessionWillBeUpdated(response, setup);
        }

        [Test]
        public void GivenCompletelyChangedNonClashingSession_WhenTryUpdateSession_ThenSessionWillBeCompletelyUpdated()
        {
            var setup = RegisterBusiness();
            RegisterLocationRemuera(setup);
            RegisterCoachBobby(setup);
            RegisterServiceHolidayCamp(setup);
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenCompletelyChangedNonClashingSession(setup);
            var response = WhenTryUpdateSession(command, setup);
            ThenSessionWillBeCompletelyUpdated(response, setup);
        }

        [Test]
        public void GivenSessionClashesWithAnotherStandaloneSession_WhenTryUpdateSession_ThenReturnSessionClashError()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);

            var command = GivenSessionClashesWithAnotherStandaloneSession(setup);
            var response = WhenTryUpdateSession(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiMiniRed14To15.Id);
        }

        [Test]
        public void GivenSessionClashesWithAnotherSessionInCourse_WhenTryUpdateSession_ThenReturnSessionClashErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenSessionClashesWithAnotherSessionInCourse(setup);
            var response = WhenTryUpdateSession(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
        }

        [Test]
        public void GivenWantTurnSessionIntoCourse_WhenTryUpdateSession_ThenReturnsCannotChangeSessionToCourseError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantTurnSessionIntoCourse(setup);
            var response = WhenTryUpdateSession(command, setup);
            ThenReturnsCannotChangeSessionToCourseError(response);
        }


        private ApiSessionSaveCommand GivenWantTurnSessionIntoCourse(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = setup.AaronOrakeiMiniRed14To15.Id;
            command.repetition = new ApiRepetition { sessionCount = 6, repeatFrequency = "w" };

            return command;
        }

        private ApiSessionSaveCommand GivenNonExistentSessionId(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = Guid.NewGuid();

            return command;
        }

        private ApiSessionSaveCommand GivenExistingSessionToBeUpdatedClashesWithItself(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = setup.AaronOrakeiMiniRed14To15.Id;
            command.timing.startTime = "14:30";

            return command;
        }

        private ApiSessionSaveCommand GivenExistingSessionWillNotClash(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = setup.AaronOrakeiMiniRed14To15.Id;
            command.timing.startTime = "11:30";

            return command;
        }

        private ApiSessionSaveCommand GivenSessionClashesWithAnotherStandaloneSession(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = setup.AaronOrakeiMiniRed16To17.Id;
            command.timing.startTime = "14:30";

            return command;
        }

        private ApiSessionSaveCommand GivenSessionClashesWithAnotherSessionInCourse(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiMiniRed14To15);
            command.id = setup.AaronOrakeiMiniRed14To15.Id;
            command.timing.startDate = GetDateFormatNumberOfDaysOut(15);

            return command;
        }

        private ApiSessionSaveCommand GivenCompletelyChangedNonClashingSession(SetupData setup)
        {
            return new ApiSessionSaveCommand
            {
                id = setup.AaronOrakeiMiniRed14To15.Id,
                location = new ApiLocationKey { id = setup.Remuera.Id },
                coach = new ApiCoachKey { id = setup.Bobby.Id },
                service = new ApiServiceKey { id = setup.HolidayCamp.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(7), startTime = "22:00", duration = 30 },
                booking = new ApiSessionBooking { studentCapacity = 7, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 15m },
                presentation = new ApiPresentation { colour = "green" }
            };
        }

        private SessionData ThenSelfClashingSessionWillBeUpdated(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));

            AssertSessionLocation(session.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(session.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(session.service, setup.MiniRed.Id, setup.MiniRed.Name);

            AssertSessionTiming(session.timing, GetDateFormatNumberOfDaysOut(21), "14:30", 60);
            AssertSessionBooking(session.booking, 3, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            return session;
        }

        private SessionData ThenSessionWillBeUpdated(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));

            AssertSessionLocation(session.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(session.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(session.service, setup.MiniRed.Id, setup.MiniRed.Name);

            AssertSessionTiming(session.timing, GetDateFormatNumberOfDaysOut(21), "11:30", 60);
            AssertSessionBooking(session.booking, 3, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            return session;
        }

        private void ThenSessionWillBeCompletelyUpdated(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));

            AssertSessionLocation(session.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(session.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(session.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(session.timing, GetDateFormatNumberOfDaysOut(7), "22:00", 30);
            AssertSessionBooking(session.booking, 7, false);
            AssertSessionPricing(session.pricing, 15, null);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPresentation(session.presentation, "green");
        }

        private void ThenReturnsCannotChangeSessionToCourseError(ApiResponse response)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionChangeToCourseNotSupported, 
                              "Cannot change a session to a course.");
        }
    }
}
