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
        public void GivenNonExistentCourseId_WhenTryUpdateCourse_ThenReturnNotFoundResponse()
        {
            var command = GivenNonExistentCourseId();
            var response = WhenTryUpdateCourse(command);
            AssertNotFound(response);
        }

        [Test]
        public void GivenNonExistentLocationId_WhenTryUpdateCourse_ThenReturnInvalidLocationErrorResponse()
        {
            var command = GivenNonExistentLocationId();
            var response = WhenTryUpdateCourse(command);
            ThenReturnInvalidLocationErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentCoachId_WhenTryUpdateCourse_ThenReturnInvalidCoachErrorResponse()
        {
            var command = GivenNonExistentCoachId();
            var response = WhenTryUpdateCourse(command);
            ThenReturnInvalidCoachErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentServiceId_WhenTryUpdateCourse_ThenReturnInvalidServiceErrorResponse()
        {
            var command = GivenNonExistentServiceId();
            var response = WhenTryUpdateCourse(command);
            ThenReturnInvalidServiceErrorResponse(response);
        }

        [Test]
        public void GivenWantTurnCourseIntoSession_WhenTryUpdateCourse_ThenReturnsCannotUpdateRepetitionOfCourseError()
        {
            var command = GivenWantToTurnCourseIntoSession();
            var response = WhenTryUpdateCourse(command);
            ThenReturnsCannotUpdateRepetitionOfCourseError(response);
        }

        [Test]
        public void GivenWantToChangeRepetitionOfCourse_WhenTryUpdateCourse_ThenReturnsCannotUpdateRepetitionOfCourseError()
        {
            var command = GivenWantToChangeRepetitionOfCourse();
            var response = WhenTryUpdateCourse(command);
            ThenReturnsCannotUpdateRepetitionOfCourseError(response);
        }

        [Test]
        public void GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate_WhenTryUpdateCourse_ThenUpdatesCourseButLeavesSessionStartDatesTheSame()
        {
            var command = GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate();
            var response = WhenTryUpdateCourse(command);
            ThenUpdatesCourseButLeavesSessionStartDatesTheSame(response);
        }


        [Test]
        public void GivenWantToUpdateCourseAndChangeCourseStartDate_WhenTryUpdateCourse_ThenUpdatesCourseAndMovesSessionStartDates()
        {
            var command = GivenWantToUpdateCourseAndChangeCourseStartDate();
            var response = WhenTryUpdateCourse(command);
            ThenUpdatesCourseAndMovesSessionStartDates(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourse_WhenTryUpdateCourse_ThenReturnsSessionUpdatedResponse()
        {
            var command = GivenWantToUpdateSessionInCourse();
            var response = WhenTryUpdateCourse(command);
            ThenReturnsSessionUpdatedResponse(response);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself_WhenTryUpdateCourse_ThenReturnsSessionTimeUpdatedResponse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            var response = WhenTryUpdateCourse(command);
            ThenReturnsSessionTimeUpdatedResponse(response);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse_WhenTryUpdateCourse_ThenReturnsSessionClashingErrorResponse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse();
            var response = WhenTryUpdateCourse(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For5WeeksSessionIds[3].ToString()));
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountGreaterThanOne_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(5);
            var response = WhenTryUpdateCourse(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountLessThanOne_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(-3);
            var response = WhenTryUpdateCourse(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var command = GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency();
            var response = WhenTryUpdateCourse(command);
            ThenReturnsInvalidRepetitionErrorResponse(response);
        }


        private ApiSessionSaveCommand GivenNonExistentCourseId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();
            course.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenNonExistentLocationId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();
            course.id = AaronRemuera9To10For5WeeksCourseId;
            course.location.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenNonExistentCoachId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();
            course.id = AaronRemuera9To10For5WeeksCourseId;
            course.coach.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenNonExistentServiceId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();
            course.id = AaronRemuera9To10For5WeeksCourseId;
            course.service.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenWantToTurnCourseIntoSession()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();

            courseCommand.id = AaronRemuera9To10For5WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 1 };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToChangeRepetitionOfCourse()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For5Weeks();

            courseCommand.id = AaronRemuera9To10For5WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 3, repeatFrequency = "d" };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate()
        {
            // Move one of the sessions to a different time and date to make the test more relevant.
            ChangeTimingForLastSessionInCourse();


            var courseCommand = CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days();
            courseCommand.id = BobbyRemueraHolidayCampFor3DaysCourseId;

            courseCommand.location = new ApiLocationKey { id = Orakei.Id };
            courseCommand.coach = new ApiCoachKey { id = Aaron.Id };
            courseCommand.booking = new ApiSessionBooking {studentCapacity = 30, isOnlineBookable = true};
            courseCommand.pricing = new ApiPricing {sessionPrice = 150, coursePrice = 220};
            courseCommand.presentation = new ApiPresentation {colour = "orange"};

            courseCommand.timing.startTime = "9:30";
            courseCommand.timing.duration = 300;

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourseAndChangeCourseStartDate()
        {
            var command = GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate();
            command.timing.startDate = GetDateFormatNumberOfDaysOut(4);

            return command;
        }

        private void ThenUpdatesCourseButLeavesSessionStartDatesTheSame(Response response)
        {
            var course = AssertSuccessResponse<CourseData>(response);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.parentId, Is.Null);
            Assert.That(course.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            AssertSessionLocation(course.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(course.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(course.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(2), "9:30", 300);
            AssertSessionBooking(course.booking, 30, true);
            AssertSessionRepetition(course.repetition, 3, "d");
            AssertSessionPricing(course.pricing, 150, 220);
            AssertSessionPresentation(course.presentation, "orange");
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSession = course.sessions[0];
            Assert.That(firstSession, Is.Not.Null);
            Assert.That(firstSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(firstSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[0]));
            AssertSessionLocation(firstSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(firstSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(firstSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(firstSession.timing, GetDateFormatNumberOfDaysOut(2), "9:30", 300);
            AssertSessionBooking(firstSession.booking, 30, true);
            AssertSessionRepetition(firstSession.repetition, 1, null);
            AssertSessionPricing(firstSession.pricing, 150, null);
            AssertSessionPresentation(firstSession.presentation, "orange");

            var secondSession = course.sessions[1];
            Assert.That(secondSession, Is.Not.Null);
            Assert.That(secondSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(secondSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
            AssertSessionLocation(secondSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(secondSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(secondSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(secondSession.timing, GetDateFormatNumberOfDaysOut(3), "9:30", 300);
            AssertSessionBooking(secondSession.booking, 30, true);
            AssertSessionRepetition(secondSession.repetition, 1, null);
            AssertSessionPricing(secondSession.pricing, 150, null);
            AssertSessionPresentation(secondSession.presentation, "orange");

            var thirdSession = course.sessions[2];
            Assert.That(thirdSession, Is.Not.Null);
            Assert.That(thirdSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(thirdSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[2]));
            AssertSessionLocation(thirdSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(thirdSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(thirdSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(thirdSession.timing, GetDateFormatNumberOfDaysOut(6), "9:30", 300);
            AssertSessionBooking(thirdSession.booking, 30, true);
            AssertSessionRepetition(thirdSession.repetition, 1, null);
            AssertSessionPricing(thirdSession.pricing, 150, null);
            AssertSessionPresentation(thirdSession.presentation, "orange");
        }

        private void ThenUpdatesCourseAndMovesSessionStartDates(Response response)
        {
            var course = AssertSuccessResponse<CourseData>(response);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.parentId, Is.Null);
            Assert.That(course.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            AssertSessionLocation(course.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(course.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(course.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(4), "9:30", 300);
            AssertSessionBooking(course.booking, 30, true);
            AssertSessionRepetition(course.repetition, 3, "d");
            AssertSessionPricing(course.pricing, 150, 220);
            AssertSessionPresentation(course.presentation, "orange");
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSession = course.sessions[0];
            Assert.That(firstSession, Is.Not.Null);
            Assert.That(firstSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(firstSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[0]));
            AssertSessionLocation(firstSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(firstSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(firstSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(firstSession.timing, GetDateFormatNumberOfDaysOut(4), "9:30", 300);
            AssertSessionBooking(firstSession.booking, 30, true);
            AssertSessionRepetition(firstSession.repetition, 1, null);
            AssertSessionPricing(firstSession.pricing, 150, null);
            AssertSessionPresentation(firstSession.presentation, "orange");

            var secondSession = course.sessions[1];
            Assert.That(secondSession, Is.Not.Null);
            Assert.That(secondSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(secondSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
            AssertSessionLocation(secondSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(secondSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(secondSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(secondSession.timing, GetDateFormatNumberOfDaysOut(5), "9:30", 300);
            AssertSessionBooking(secondSession.booking, 30, true);
            AssertSessionRepetition(secondSession.repetition, 1, null);
            AssertSessionPricing(secondSession.pricing, 150, null);
            AssertSessionPresentation(secondSession.presentation, "orange");

            var thirdSession = course.sessions[2];
            Assert.That(thirdSession, Is.Not.Null);
            Assert.That(thirdSession.parentId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            Assert.That(thirdSession.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[2]));
            AssertSessionLocation(thirdSession.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(thirdSession.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(thirdSession.service, HolidayCampId, "Holiday Camp");
            AssertSessionTiming(thirdSession.timing, GetDateFormatNumberOfDaysOut(8), "9:30", 300);
            AssertSessionBooking(thirdSession.booking, 30, true);
            AssertSessionRepetition(thirdSession.repetition, 1, null);
            AssertSessionPricing(thirdSession.pricing, 150, null);
            AssertSessionPresentation(thirdSession.presentation, "orange");
        }

        private void ChangeTimingForLastSessionInCourse()
        {
            var sessionCommand = CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days();
            sessionCommand.id = BobbyRemueraHolidayCampFor3DaysSessionIds[2];
            sessionCommand.repetition = new ApiRepetition {sessionCount = 1};
            sessionCommand.timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(6), startTime = "11:00", duration = 180 };
            sessionCommand.presentation = new ApiPresentation { colour = "mid-blue" };

            WhenTryUpdateSession(sessionCommand);
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourse()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronRemuera9To10For5WeeksSessionIds[2],
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Bobby.Id },
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
            command.id = AaronRemuera9To10For5WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(3);
            command.timing.startTime = "9:15";

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            command.id = AaronRemuera9To10For5WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(4);
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


        private void ThenReturnInvalidLocationErrorResponse(Response response)
        {
            AssertSingleError(response, "Invalid location.");
        }

        private void ThenReturnInvalidCoachErrorResponse(Response response)
        {
            AssertSingleError(response, "Invalid coach.");
        }

        private void ThenReturnInvalidServiceErrorResponse(Response response)
        {
            AssertSingleError(response, "Invalid service.");
        }

        private void ThenReturnsCannotUpdateRepetitionOfCourseError(Response response)
        {
            AssertSingleError(response, "Cannot change the repetition of a course.");
        }

        private void ThenReturnsSessionTimeUpdatedResponse(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For5WeeksSessionIds[2]));

            AssertSessionLocation(session.location, Remuera.Id, Remuera.Name);
            AssertSessionCoach(session.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(session.service, MiniRed.Id, MiniRed.Name);

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
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For5WeeksSessionIds[2]));

            AssertSessionLocation(session.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(session.coach, Bobby.Id, Bobby.Name);
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
