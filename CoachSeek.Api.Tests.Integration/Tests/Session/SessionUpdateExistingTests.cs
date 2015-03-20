using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionUpdateExistingTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNonExistentSessionId_WhenTryUpdateSession_ThenReturnNotFoundResponse()
        {
            var command = GivenNonExistentSessionId();
            var response = WhenTryUpdateSession(command);
            AssertNotFound(response);
        }

        [Test]
        public void GivenUpdatedSessionClashesWithItself_WhenTryUpdateSession_ThenSessionWasUpdatedResponse()
        {
            var command = GivenUpdatedSessionClashesWithItself();
            var response = WhenTryUpdateSession(command);
            ThenSessionWasUpdatedResponse(response, GetDateFormatNumberOfWeeksOut(3), "14:30");
        }

        [Test]
        public void GivenSessionWillNotClash_WhenTryUpdateSession_ThenSessionWasUpdatedResponse()
        {
            var command = ChangeTimeForSessionCoachedByAaronTo("11:30");
            var response = WhenTryUpdateSession(command);
            ThenSessionWasUpdatedResponse(response, GetDateFormatNumberOfWeeksOut(3), "11:30");
        }

        [Test]
        public void GivenCompletelyChangedNonClashingSession_WhenTryUpdateSession_ThenReturnCompletelyChangedSessionWasUpdatedResponse()
        {
            var command = GivenCompletelyChangedNonClashingSession();
            var response = WhenTryUpdateSession(command);
            ThenReturnCompletelyChangedSessionWasUpdatedResponse(response);
        }

        [Test]
        public void GivenSessionClashesWithAnotherSession_WhenTryUpdateSession_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenSessionClashesWithAnotherSession();
            var response = WhenTryUpdateSession(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronOrakei14To15SessionId.ToString()));
        }

        [Test]
        public void GivenSessionClashesWithAnotherSessionInCourse_WhenTryUpdateSession_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenSessionClashesWithAnotherSessionInCourse();
            var response = WhenTryUpdateSession(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For8WeeksSessionIds[2].ToString()));
        }

        [Test]
        public void GivenWantTurnSessionIntoCourse_WhenTryUpdateSession_ThenReturnsCannotChangeSessionToCourseError()
        {
            var command = GivenWantTurnSessionIntoCourse();
            var response = WhenTryUpdateSession(command);
            ThenReturnsCannotChangeSessionToCourseError(response);
        }


        private ApiSessionSaveCommand GivenWantTurnSessionIntoCourse()
        {
            var sessionCommand = CreateSessionSaveCommandAaronOrakei16To17();

            sessionCommand.id = AaronOrakei16To17SessionId;
            sessionCommand.repetition = new ApiRepetition { sessionCount = 6, repeatFrequency = "w" };

            return sessionCommand;
        }

        private void ThenReturnsCannotChangeSessionToCourseError(Response response)
        {
            AssertSingleError(response, "Cannot change a session to a course.");
        }


        private ApiSessionSaveCommand GivenNonExistentSessionId()
        {
            var session = GivenExistingSession();
            session.id = Guid.NewGuid();

            return session;
        }

        private ApiSessionSaveCommand GivenUpdatedSessionClashesWithItself()
        {
            var command = CreateSessionSaveCommandAaronOrakei14To15();

            command.id = AaronOrakei14To15SessionId;
            command.booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true };
            command.repetition = new ApiRepetition { sessionCount = 1 };
            command.presentation = new ApiPresentation { colour = "red" };
            command.pricing = new ApiPricing { sessionPrice = 19.95m };
            command.timing.startTime = "14:30";

            return command;
        }

        private ApiSessionSaveCommand GivenExistingSession()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronOrakei14To15SessionId,
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "14:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 12, isOnlineBookable = false },
                pricing = new ApiPricing { sessionPrice = 10, coursePrice = 80 },
                repetition = new ApiRepetition { sessionCount = 10, repeatFrequency = "w" },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        private ApiSessionSaveCommand GivenSessionClashesWithAnotherSession()
        {
            var command = CreateSessionSaveCommandAaronOrakei16To17();

            command.id = AaronOrakei16To17SessionId;
            command.service.id = MiniBlueId;
            command.location.id = RemueraId;

            command.booking = new ApiSessionBooking { studentCapacity = 5, isOnlineBookable = true };
            command.repetition = new ApiRepetition { sessionCount = 1 };
            command.presentation = new ApiPresentation { colour = "blue" };
            command.pricing = new ApiPricing { sessionPrice = 15 };

            // Should clash with AaronOrakei14To15Session
            command.timing.startDate = GetFormattedDateThreeWeeksOut();
            command.timing.startTime = "14:30";

            return command;
        }

        private ApiSessionSaveCommand GivenSessionClashesWithAnotherSessionInCourse()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronOrakei14To15SessionId,
                coach = new ApiCoachKey { id = AaronId },
                location = new ApiLocationKey { id = RemueraId },
                service = new ApiServiceKey { id = MiniBlueId },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(3), startTime = "9:45", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 1, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 60 },
                presentation = new ApiPresentation { colour = "blue " }
            };
        }

        private ApiSessionSaveCommand ChangeTimeForSessionCoachedByAaronTo(string startTime)
        {
            var command = CreateSessionSaveCommandAaronOrakei14To15();

            command.id = AaronOrakei14To15SessionId;
            command.booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true };
            command.repetition = new ApiRepetition { sessionCount = 1 };
            command.presentation = new ApiPresentation { colour = "red" };
            command.pricing = new ApiPricing { sessionPrice = 19.95m };
            command.timing.startTime = startTime;

            return command;
        }

        private ApiSessionSaveCommand GivenCompletelyChangedNonClashingSession()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronOrakei14To15SessionId,
                location = new ApiLocationKey { id = RemueraId },
                coach = new ApiCoachKey { id = BobbyId },
                service = new ApiServiceKey { id = MiniGreenId },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(2), startTime = "22:00", duration = 30 },
                booking = new ApiSessionBooking { studentCapacity = 7, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 15m },
                presentation = new ApiPresentation { colour = "green" }
            };
        }

        private SessionData ThenSessionWasUpdatedResponse(Response response, string startDate, string startTime)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.EqualTo(AaronOrakei14To15SessionId));

            AssertSessionLocation(session.location, OrakeiId, "Orakei Tennis Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, startDate, startTime, 60);
            AssertSessionBooking(session.booking, 13, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            return session;
        }

        private void ThenReturnCompletelyChangedSessionWasUpdatedResponse(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.id, Is.EqualTo(AaronOrakei14To15SessionId));

            AssertSessionLocation(session.location, RemueraId, REMUERA_NAME);
            AssertSessionCoach(session.coach, BobbyId, "Bobby Smith");
            AssertSessionService(session.service, MiniGreenId, "Mini Green");
            AssertSessionTiming(session.timing, GetFormattedDateTwoWeeksOut(), "22:00", 30);
            AssertSessionBooking(session.booking, 7, false);
            AssertSessionPricing(session.pricing, 15, null);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPresentation(session.presentation, "green");
        }
    }
}
