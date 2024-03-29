﻿using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseCreateNewTests : ScheduleTests
    {
        [Test]
        public void GivenNewCourseClashesWithStandaloneSession_WhenTryCreateCourse_ThenReturnSessionClashError()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseClashesWithStandaloneSession(setup);
            var response = WhenTryCreateCourse(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiMiniRed14To15.Id);
        }

        [Test]
        public void GivenNewCourseClashesWithAnotherCourse_WhenTryCreateCourse_ThenReturnSessionClashError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseClashesWithAnotherCourse(setup);
            var response = WhenTryCreateCourse(command, setup);
            AssertSessionClashError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
        }

        // TODO: Course clashes with course session.

        [Test]
        public void GivenNewCourseWithTooManyDailySessions_WhenTryCreateCourse_ThenReturnTooManySessionInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithTooManyDailySessions(setup);
            var response = WhenTryCreateCourse(command, setup);
            AssertSingleError(response,
                              ErrorCodes.CourseExceedsMaximumNumberOfDailySessions,
                              "31 exceeds the maximum number of daily sessions in a course of 30.",
                              "Maximum Allowed Daily Session Count: 30; Specified Session Count: 31");
        }

        [Test]
        public void GivenNewCourseWithTooManyWeeklySessions_WhenTryCreateCourse_ThenReturnTooManySessionInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithTooManyWeeklySessions(setup);
            var response = WhenTryCreateCourse(command, setup);
            AssertSingleError(response,
                              ErrorCodes.CourseExceedsMaximumNumberOfWeeklySessions,
                              "27 exceeds the maximum number of weekly sessions in a course of 26.",
                              "Maximum Allowed Weekly Session Count: 26; Specified Session Count: 27");
        }

        [Test]
        public void GivenNewCourseWithNeitherSessionNorCoursePrice_WhenTryCreateCourse_ThenReturnWithNoPriceError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithNeitherSessionNorCoursePrice(setup);
            var response = WhenTryCreateCourse(command, setup);
            AssertSingleError(response,
                              ErrorCodes.CourseMustHavePrice,
                              "A course must have at least a session or course price.");
        }

        [Test]
        public void GivenNewCourseWithCoursePriceOnly_WhenTryCreateCourse_ThenCreatesCourseWithCoursePriceOnly()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithCoursePriceOnly(setup);
            var response = WhenTryCreateCourse(command, setup);
            ThenCreatesCourseWithCoursePriceOnly(response, setup);
        }

        [Test]
        public void GivenNewCourseWithSessionPriceOnly_WhenTryCreateCourse_ThenCreatesCourseWithSessionPriceOnly()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithSessionPriceOnly(setup);
            var response = WhenTryCreateCourse(command, setup);
            ThenCreatesCourseWithSessionPriceOnly(response, setup);
        }

        [Test]
        public void GivenNewCourseWithBothSessionPriceAndCoursePrice_WhenTryCreateCourse_ThenCreatesCourseWithSessionPriceAndCoursePrice()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithBothSessionPriceAndCoursePrice(setup);
            var response = WhenTryCreateCourse(command, setup);
            ThenCreatesCourseWithSessionPriceAndCoursePrice(response, setup);
        }

        [Test]
        public void GivenNewCourseWithZeroSessionPrice_WhenTryCreateCourse_ThenCreatesCourseWithZeroSessionPrice()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWithZeroSessionPrice(setup);
            var response = WhenTryCreateCourse(command, setup);
            ThenCreatesCourseWithZeroSessionPrice(response, setup);
        }

        [Test]
        public void GivenNewCourseWith24HourStartTime_WhenTryCreateCourse_ThenCreatesCourseWith24HrStartTime()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNewCourseWith24HourStartTime(setup);
            var response = WhenTryCreateCourse(command, setup);
            ThenCreatesCourseWith24HrStartTime(response, setup);
        }


        private ApiSessionSaveCommand GivenNewCourseClashesWithStandaloneSession(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(20);

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseClashesWithAnotherCourse(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(16);

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithTooManyDailySessions(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.repetition = new ApiRepetition(31, "d");

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithTooManyWeeklySessions(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.repetition = new ApiRepetition(27, "w");

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithNeitherSessionNorCoursePrice(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.pricing = new ApiPricing { sessionPrice = null, coursePrice = null };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithCoursePriceOnly(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.pricing = new ApiPricing { sessionPrice = null, coursePrice = 120 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithSessionPriceOnly(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.pricing = new ApiPricing { sessionPrice = 37.5m, coursePrice = null };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithBothSessionPriceAndCoursePrice(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.pricing = new ApiPricing { sessionPrice = 37.5m, coursePrice = 100 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithZeroSessionPrice(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.pricing = new ApiPricing { sessionPrice = 0 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWith24HourStartTime(SetupData setup)
        {
            var command = CreateCourseSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days);
            command.timing.startDate = GetDateFormatNumberOfDaysOut(7);
            command.timing.startTime = "21:30";

            return command;
        }


        private void ThenCreatesCourseWithCoursePriceOnly(ApiResponse response, SetupData setup)
        {
            var courseResponse = AssertSuccessResponse<CourseData>(response);
            AssertSessionPricing(courseResponse.pricing, null, 120);

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, courseResponse.id, setup);
            var course = (CourseData)getResponse.Payload;
            AssertSessionPricing(courseResponse.pricing, null, 120);
        }

        private void ThenCreatesCourseWithSessionPriceOnly(ApiResponse response, SetupData setup)
        {
            var courseResponse = AssertSuccessResponse<CourseData>(response);
            AssertSessionPricing(courseResponse.pricing, 37.5m, null);

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, courseResponse.id, setup);
            var course = (CourseData)getResponse.Payload;
            AssertSessionPricing(course.pricing, 37.5m, null);
        }

        private void ThenCreatesCourseWithSessionPriceAndCoursePrice(ApiResponse response, SetupData setup)
        {
            var courseResponse = AssertSuccessResponse<CourseData>(response);
            AssertSessionPricing(courseResponse.pricing, 37.5m, 100);

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, courseResponse.id, setup);
            var course = (CourseData)getResponse.Payload;
            AssertSessionPricing(course.pricing, 37.5m, 100);
        }

        private void ThenCreatesCourseWithZeroSessionPrice(ApiResponse response, SetupData setup)
        {
            var courseResponse = AssertSuccessResponse<CourseData>(response);
            AssertSessionPricing(courseResponse.pricing, 0, null);

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, courseResponse.id, setup);
            var course = (CourseData)getResponse.Payload;
            AssertSessionPricing(course.pricing, 0, null);
        }

        private void ThenCreatesCourseWith24HrStartTime(ApiResponse response, SetupData setup)
        {
            var courseResponse = AssertSuccessResponse<CourseData>(response);
            AssertSessionTiming(courseResponse.timing, GetDateFormatNumberOfDaysOut(7), "21:30", 360);

            var getResponse = AuthenticatedGet<CourseData>(RelativePath, courseResponse.id, setup);
            var course = (CourseData)getResponse.Payload;
            AssertSessionTiming(course.timing, GetDateFormatNumberOfDaysOut(7), "21:30", 360);
        }
    }
}
