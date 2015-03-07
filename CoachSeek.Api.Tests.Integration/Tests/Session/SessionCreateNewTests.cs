using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionCreateNewTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNewSessionClashesWithStandaloneSession_WhenPost_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenNewSessionClashesWithStandaloneSession();
            var response = WhenPost(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(string.Format("{{{0}}}", AaronOrakei16To17SessionId)));
        }

        [Test]
        public void GivenNewSessionClashesWithExistingCourse_WhenPost_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenNewSessionClashesWithExistingCourse();
            var response = WhenPost(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For8WeeksSessionIds[1].ToString()));
        }

        [Test]
        public void GivenNewSessionWithZeroSessionCount_WhenPost_ThenReturnInvalidSessionCountErrorResponse()
        {
            var command = GivenNewSessionWithZeroSessionCount();
            var response = WhenPost(command);
            AssertSingleError(response, "The sessionCount field is not valid.", "session.repetition.sessionCount");
        }

        [Test]
        public void GivenNewValidSession_WhenPost_ThenCreateSession()
        {
            var command = GivenNewValidSession();
            var response = WhenPost(command);
            ThenCreateNewSession(response);
        }


        private ApiSessionSaveCommand GivenNewSessionClashesWithStandaloneSession()
        {
            var command = CreateSessionSaveCommandAaronOrakei16To17();
            command.timing.startTime = "16:30";

            return command;
        }

        private ApiSessionSaveCommand GivenNewSessionClashesWithExistingCourse()
        {
            return new ApiSessionSaveCommand
            {
                coach = new ApiCoachKey { id = AaronId },
                location = new ApiLocationKey { id = OrakeiId },
                service = new ApiServiceKey { id = MiniBlueId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateTwoWeeksOut(), startTime = "9:30", duration = 30 },
                booking = new ApiSessionBooking { studentCapacity = 9, isOnlineBookable = true },
                pricing = new ApiPricing { sessionPrice = 25 },
                repetition = new ApiRepetition { sessionCount = 1 },
                presentation = new ApiPresentation { colour = "blue" }
            };
        }

        private ApiSessionSaveCommand GivenNewValidSession()
        {
            return new ApiSessionSaveCommand
            {
                service = new ApiServiceKey { id = MiniRedId },
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                timing = new ApiSessionTiming { startDate = "2018-11-09", startTime = "15:30", duration = 45 },
                booking = new ApiSessionBooking { studentCapacity = 10, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 12 },
                presentation = new ApiPresentation { colour = "Red" }
            };
        }

        private ApiSessionSaveCommand GivenNewSessionWithZeroSessionCount()
        {
            var command = GivenNewValidSession();
            command.repetition.sessionCount = 0;

            return command;
        }

        private SessionData ThenCreateNewSession(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));

            AssertSessionLocation(session.location, OrakeiId, "Orakei Tennis Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, "2018-11-09", "15:30", 45);
            AssertSessionBooking(session.booking, 10, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 12, null);
            AssertSessionPresentation(session.presentation, "red");

            return session;
        }
    }
}
