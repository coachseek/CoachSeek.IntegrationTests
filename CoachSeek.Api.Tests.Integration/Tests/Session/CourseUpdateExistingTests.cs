using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseUpdateExistingTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNonExistentCourseId_WhenPost_ThenReturnNotFoundResponse()
        {
            var command = GivenNonExistentCourseId();
            var response = WhenPost(command);
            AssertNotFound(response);
        }

        [Test]
        public void GivenWantTurnCourseIntoSession_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToTurnCourseIntoSession();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }

        [Test]
        public void GivenWantToChangeRepetitionOfCourse_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToChangeRepetitionOfCourse();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }

        [Test]
        public void GivenWantToUpdateCourse_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToUpdateCourse();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourse_WhenPost_ThenReturnsSessionUpdatedResponse()
        {
            var command = GivenWantToUpdateSessionInCourse();
            var response = WhenPost(command);
            ThenReturnsSessionUpdatedResponse(response);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself_WhenPost_ThenReturnsSessionTimeUpdatedResponse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            var response = WhenPost(command);
            ThenReturnsSessionTimeUpdatedResponse(response);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse_WhenPost_ThenReturnsSessionClashingErrorResponse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse();
            var response = WhenPost(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For8WeeksSessionIds[6].ToString()));
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountGreaterThanOne_WhenPost_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(5);
            var response = WhenPost(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountLessThanOne_WhenPost_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(-3);
            var response = WhenPost(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency_WhenPost_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency();
            var response = WhenPost(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }


        private ApiSessionSaveCommand GivenNonExistentCourseId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();
            course.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenWantToTurnCourseIntoSession()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 1 };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToChangeRepetitionOfCourse()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 5, repeatFrequency = "d" };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourse()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.location = new ApiLocationKey { id = RemueraId };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourse()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronRemuera9To10For8WeeksSessionIds[2],
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = BobbyId },
                service = new ApiServiceKey { id = MiniGreenId },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(3), startTime = "10:30", duration = 45 },
                booking = new ApiSessionBooking { studentCapacity = 11, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 16 },
                presentation = new ApiPresentation { colour = "green" }
            };
        }

        private ApiSessionSaveCommand GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself()
        {
            var command = CreateSessionSaveCommandAaronRemuera9To10();
            command.id = AaronRemuera9To10For8WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(3);
            command.timing.startTime = "9:15";

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            command.id = AaronRemuera9To10For8WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(7);
            command.timing.startTime = "9:45";

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourseWithInvalidSessionCount(int sessionCount)
        {
            var command = GivenWantToUpdateSessionInCourse();
            command.repetition = new ApiRepetition { sessionCount = sessionCount };

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency()
        {
            var command = GivenWantToUpdateSessionInCourse();
            command.repetition = new ApiRepetition { sessionCount = 1, repeatFrequency = "w" };

            return command;
        }


        private void ThenReturnsCannotUpdateRepetitionOfCourseError(Response response)
        {
            AssertSingleError(response, "Cannot change the repetition of a course.");
        }

        private void ThenReturnsCannotUpdateCourseError(Response response)
        {
            AssertSingleError(response, "Course updates are not working yet.");
        }

        private void ThenReturnsSessionTimeUpdatedResponse(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For8WeeksSessionIds[2]));

            AssertSessionLocation(session.location, RemueraId, "Remuera Racquets Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "9:15", 60);
            AssertSessionBooking(session.booking, 13, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");
        }

        private void ThenReturnsSessionUpdatedResponse(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For8WeeksSessionIds[2]));

            AssertSessionLocation(session.location, OrakeiId, "Orakei Tennis Club");
            AssertSessionCoach(session.coach, BobbyId, "Bobby Smith");
            AssertSessionService(session.service, MiniGreenId, "Mini Green");

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "10:30", 45);
            AssertSessionBooking(session.booking, 11, false);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 16, null);
            AssertSessionPresentation(session.presentation, "green");
        }

        private void ThenReturnsInvalidRepetitionErrorResponse(Response response)
        {
            AssertSingleError(response, "Cannot change a session to a course.");
        }
    }
}
