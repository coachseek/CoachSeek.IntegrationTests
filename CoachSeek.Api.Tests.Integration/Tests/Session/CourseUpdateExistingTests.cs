﻿using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class  CourseUpdateExistingTests : ScheduleTests
    {
        [Test]
        public void GivenNonExistentCourseId_WhenTryUpdateCourse_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNonExistentCourseId(setup);
            var response = WhenTryUpdateCourse(command, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenNonExistentLocationId_WhenTryUpdateCourse_ThenReturnInvalidLocationErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNonExistentLocationId(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnInvalidLocationErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentCoachId_WhenTryUpdateCourse_ThenReturnInvalidCoachErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNonExistentCoachId(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnInvalidCoachErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentServiceId_WhenTryUpdateCourse_ThenReturnInvalidServiceErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNonExistentServiceId(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnInvalidServiceErrorResponse(response);
        }

        [Test]
        public void GivenWantTurnCourseIntoSession_WhenTryUpdateCourse_ThenReturnsCannotUpdateRepetitionOfCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToTurnCourseIntoSession(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsCannotUpdateRepetitionOfCourseError(response);
        }

        [Test]
        public void GivenWantToChangeRepetitionOfCourse_WhenTryUpdateCourse_ThenReturnsCannotUpdateRepetitionOfCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToChangeRepetitionOfCourse(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsCannotUpdateRepetitionOfCourseError(response);
        }

        [Test]
        public void GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate_WhenTryUpdateCourse_ThenUpdatesCourseButLeavesSessionStartDatesTheSame()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCoachBobby(setup);
            RegisterLocationRemuera(setup);

            var command = GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenUpdatesCourseButLeavesSessionStartDatesTheSame(response, setup);
        }


        [Test]
        public void GivenWantToUpdateCourseAndChangeCourseStartDate_WhenTryUpdateCourse_ThenUpdatesCourseAndMovesSessionStartDates()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateCourseAndChangeCourseStartDate(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenUpdatesCourseAndMovesSessionStartDates(response, setup);
        }

        [Test]
        public void GivenWantToUpdateCourseSession_WhenTryUpdateCourse_ThenUpdateCourseSession()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCoachBobby(setup);
            RegisterLocationRemuera(setup);

            var command = GivenWantToUpdateCourseSession(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenUpdateCourseSession(response, setup);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself_WhenTryUpdateCourse_ThenReturnsSessionTimeUpdatedResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsSessionTimeUpdatedResponse(response);
        }

        [Test]
        public void GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse_WhenTryUpdateCourse_ThenReturnsSessionClashingErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse();
            var response = WhenTryUpdateCourse(command, setup);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For4WeeksSessionIds[3].ToString()));
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountGreaterThanOne_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(5, setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsInvalidRepetitionError(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithSessionCountLessThanOne_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionErrorResponse()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateSessionInCourseWithInvalidSessionCount(-3, setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsInvalidRepetitionError(response);
        }

        [Test]
        public void GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency_WhenTryUpdateCourse_ThenReturnsInvalidRepetitionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency(setup);
            var response = WhenTryUpdateCourse(command, setup);
            ThenReturnsInvalidRepetitionError(response);
        }


        private ApiSessionSaveCommand GivenNonExistentCourseId(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = Guid.NewGuid();

            return command;
        }

        private ApiSessionSaveCommand GivenNonExistentLocationId(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.location.id = Guid.NewGuid();

            return command;
        }

        private ApiSessionSaveCommand GivenNonExistentCoachId(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.coach.id = Guid.NewGuid();

            return command;
        }

        private ApiSessionSaveCommand GivenNonExistentServiceId(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.service.id = Guid.NewGuid();

            return command;
        }

        private ApiSessionSaveCommand GivenWantToTurnCourseIntoSession(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.repetition = new ApiRepetition { sessionCount = 1 };

            return command;
        }

        private ApiSessionSaveCommand GivenWantToChangeRepetitionOfCourse(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.repetition = new ApiRepetition { sessionCount = 6, repeatFrequency = "d" };

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourseButLeaveCourseStartingAtSameDate(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;

            command.location = new ApiLocationKey { id = setup.Remuera.Id };
            command.coach = new ApiCoachKey { id = setup.Bobby.Id };
            command.booking = new ApiSessionBooking { studentCapacity = 20, isOnlineBookable = true };
            command.pricing = new ApiPricing { sessionPrice = 80, coursePrice = 200 };
            command.presentation = new ApiPresentation { colour = "orange" };

            command.timing.startTime = "9:30";
            command.timing.duration = 300;

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourseAndChangeCourseStartDate(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Id;
            command.timing.startDate = GetDateFormatNumberOfDaysOut(4);

            return command;
        }

        private void ThenUpdatesCourseButLeavesSessionStartDatesTheSame(ApiResponse response, SetupData setup)
        {
            var course = AssertSuccessResponse<CourseData>(response);

            Assert.That(course.parentId, Is.Null);
            Assert.That(course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));

            AssertSessionLocation(course.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(course.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(course.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(14), "9:30", 300);
            AssertSessionBooking(course.booking, 20, true);
            AssertSessionRepetition(course.repetition, 3, "d");
            AssertSessionPricing(course.pricing, 80, 200);
            AssertSessionPresentation(course.presentation, "orange");
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSession = course.sessions[0];
            Assert.That(firstSession, Is.Not.Null);
            Assert.That(firstSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(firstSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id));
            AssertSessionLocation(firstSession.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(firstSession.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(firstSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(firstSession.timing, GetDateFormatNumberOfDaysOut(14), "9:30", 300);
            AssertSessionBooking(firstSession.booking, 20, true);
            AssertSessionRepetition(firstSession.repetition, 1, null);
            AssertSessionPricing(firstSession.pricing, 80, null);
            AssertSessionPresentation(firstSession.presentation, "orange");

            var secondSession = course.sessions[1];
            Assert.That(secondSession, Is.Not.Null);
            Assert.That(secondSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(secondSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id));
            AssertSessionLocation(secondSession.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(secondSession.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(secondSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(secondSession.timing, GetDateFormatNumberOfDaysOut(15), "9:30", 300);
            AssertSessionBooking(secondSession.booking, 20, true);
            AssertSessionRepetition(secondSession.repetition, 1, null);
            AssertSessionPricing(secondSession.pricing, 80, null);
            AssertSessionPresentation(secondSession.presentation, "orange");

            var thirdSession = course.sessions[2];
            Assert.That(thirdSession, Is.Not.Null);
            Assert.That(thirdSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(thirdSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id));
            AssertSessionLocation(thirdSession.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(thirdSession.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(thirdSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(thirdSession.timing, GetDateFormatNumberOfDaysOut(16), "9:30", 300);
            AssertSessionBooking(thirdSession.booking, 20, true);
            AssertSessionRepetition(thirdSession.repetition, 1, null);
            AssertSessionPricing(thirdSession.pricing, 80, null);
            AssertSessionPresentation(thirdSession.presentation, "orange");
        }

        private void ThenUpdatesCourseAndMovesSessionStartDates(ApiResponse response, SetupData setup)
        {
            var course = AssertSuccessResponse<CourseData>(response);

            Assert.That(course.parentId, Is.Null);
            Assert.That(course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(4), "9:00", 360);
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSession = course.sessions[0];
            AssertSessionTiming(firstSession.timing, GetDateFormatNumberOfDaysOut(4), "9:00", 360);

            var secondSession = course.sessions[1];
            AssertSessionTiming(secondSession.timing, GetDateFormatNumberOfDaysOut(5), "9:00", 360);

            var thirdSession = course.sessions[2];
            AssertSessionTiming(thirdSession.timing, GetDateFormatNumberOfDaysOut(6), "9:00", 360);
        }

        private void ThenUpdateCourseSession(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(session.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id));

            AssertSessionLocation(session.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(session.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(session.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(session.timing, GetDateFormatNumberOfDaysOut(15), "9:30", 300);
            AssertSessionBooking(session.booking, 20, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 80, null);
            AssertSessionPresentation(session.presentation, "orange");

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = (CourseData)getResponse.Payload;

            AssertSessionLocation(course.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(course.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(course.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(14), "9:00", 360);
            AssertSessionBooking(course.booking, 3, true);
            AssertSessionRepetition(course.repetition, 3, "d");
            AssertSessionPricing(course.pricing, 50, 120);
            AssertSessionPresentation(course.presentation, "yellow");
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSession = course.sessions[0];
            Assert.That(firstSession, Is.Not.Null);
            Assert.That(firstSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(firstSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id));
            AssertSessionLocation(firstSession.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(firstSession.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(firstSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(firstSession.timing, GetDateFormatNumberOfDaysOut(14), "9:00", 360);
            AssertSessionBooking(firstSession.booking, 3, true);
            AssertSessionRepetition(firstSession.repetition, 1, null);
            AssertSessionPricing(firstSession.pricing, 50, null);
            AssertSessionPresentation(firstSession.presentation, "yellow");

            var secondSession = course.sessions[1];
            Assert.That(secondSession, Is.Not.Null);
            Assert.That(secondSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(secondSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id));
            AssertSessionLocation(secondSession.location, setup.Remuera.Id, setup.Remuera.Name);
            AssertSessionCoach(secondSession.coach, setup.Bobby.Id, setup.Bobby.Name);
            AssertSessionService(secondSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(secondSession.timing, GetDateFormatNumberOfDaysOut(15), "9:30", 300);
            AssertSessionBooking(secondSession.booking, 20, true);
            AssertSessionRepetition(secondSession.repetition, 1, null);
            AssertSessionPricing(secondSession.pricing, 80, null);
            AssertSessionPresentation(secondSession.presentation, "orange");

            var thirdSession = course.sessions[2];
            Assert.That(thirdSession, Is.Not.Null);
            Assert.That(thirdSession.parentId, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(thirdSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id));
            AssertSessionLocation(thirdSession.location, setup.Orakei.Id, setup.Orakei.Name);
            AssertSessionCoach(thirdSession.coach, setup.Aaron.Id, setup.Aaron.Name);
            AssertSessionService(thirdSession.service, setup.HolidayCamp.Id, setup.HolidayCamp.Name);
            AssertSessionTiming(thirdSession.timing, GetDateFormatNumberOfDaysOut(16), "9:00", 360);
            AssertSessionBooking(thirdSession.booking, 3, true);
            AssertSessionRepetition(thirdSession.repetition, 1, null);
            AssertSessionPricing(thirdSession.pricing, 50, null);
            AssertSessionPresentation(thirdSession.presentation, "yellow");
        }

        private void ChangeTimingForLastSessionInCourse(SetupData setup)
        {
            var sessionCommand = CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days();
            sessionCommand.id = BobbyRemueraHolidayCampFor3DaysSessionIds[2];
            sessionCommand.repetition = new ApiRepetition {sessionCount = 1};
            sessionCommand.timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(6), startTime = "11:00", duration = 180 };
            sessionCommand.presentation = new ApiPresentation { colour = "mid-blue" };

            WhenTryUpdateSession(sessionCommand, setup);
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourseSession(SetupData setup)
        {
            var command = CreateSessionSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1]);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id;

            command.location = new ApiLocationKey { id = setup.Remuera.Id };
            command.coach = new ApiCoachKey { id = setup.Bobby.Id };
            command.booking = new ApiSessionBooking { studentCapacity = 20, isOnlineBookable = true };
            command.pricing = new ApiPricing { sessionPrice = 80, coursePrice = 200 };
            command.presentation = new ApiPresentation { colour = "orange" };

            command.timing.startTime = "9:30";
            command.timing.duration = 300;

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourse()
        {
            return new ApiSessionSaveCommand
            {
                id = AaronRemuera9To10For4WeeksSessionIds[2],
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Bobby.Id },
                service = new ApiServiceKey { id = MiniGreen.Id },
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
            command.id = AaronRemuera9To10For4WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(3);
            command.timing.startTime = "9:15";

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithAnotherSessionInSameCourse()
        {
            var command = GivenWantToUpdateTimeForSessionInCourseSoThatItClashesWithItself();
            command.id = AaronRemuera9To10For4WeeksSessionIds[2];
            command.timing.startDate = GetDateFormatNumberOfWeeksOut(4);
            command.timing.startTime = "9:45";

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourseWithInvalidSessionCount(int sessionCount, SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id;
            command.repetition = new ApiRepetition { sessionCount = sessionCount };

            return command;
        }

        private ApiSessionSaveCommand GivenWantToUpdateSessionInCourseWithInvalidRepeatFrequency(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id;
            command.repetition = new ApiRepetition { sessionCount = 1, repeatFrequency = "w" };

            return command;
        }


        private void ThenReturnInvalidLocationErrorResponse(ApiResponse response)
        {
            AssertSingleError(response, "Invalid location.");
        }

        private void ThenReturnInvalidCoachErrorResponse(ApiResponse response)
        {
            AssertSingleError(response, "Invalid coach.");
        }

        private void ThenReturnInvalidServiceErrorResponse(ApiResponse response)
        {
            AssertSingleError(response, "Invalid service.");
        }

        private void ThenReturnsCannotUpdateRepetitionOfCourseError(ApiResponse response)
        {
            AssertSingleError(response, "Cannot change the repetition of a course.");
        }

        private void ThenReturnsSessionTimeUpdatedResponse(ApiResponse response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[2]));

            AssertSessionLocation(session.location, Remuera.Id, Remuera.Name);
            AssertSessionCoach(session.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(session.service, MiniRed.Id, MiniRed.Name);

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "9:15", 60);
            AssertSessionBooking(session.booking, 6, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");
        }

        private void ThenReturnsSessionUpdatedResponse(ApiResponse response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[2]));

            AssertSessionLocation(session.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(session.coach, Bobby.Id, Bobby.Name);
            AssertSessionService(session.service, MiniGreen.Id, MiniGreen.Name);

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "10:30", 45);
            AssertSessionBooking(session.booking, 11, false);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 16, null);
            AssertSessionPresentation(session.presentation, "green");
        }

        private void ThenReturnsInvalidRepetitionError(ApiResponse response)
        {
            AssertSingleError(response, "Cannot change a session to a course.");
        }
    }
}
