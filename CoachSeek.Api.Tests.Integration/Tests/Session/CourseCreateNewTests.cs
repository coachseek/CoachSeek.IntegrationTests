using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseCreateNewTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNewCourseClashesWithStandaloneSession_WhenPost_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenNewCourseClashesWithStandaloneSession();
            var response = WhenPost(command);
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.data, Is.StringContaining(AaronOrakei14To15SessionId.ToString()));
        }

        // TODO: Course clashes with course session.

        [Test]
        public void GivenNewCourseWithTooManySessions_WhenPost_ThenReturnTooManySessionInCourseErrorResponse()
        {
            var command = GivenNewCourseWithTooManySessions();
            var response = WhenPost(command);
            AssertSingleError(response, "The maximum number of daily sessions is 30.", "session.repetition.sessionCount");
        }

        [Test]
        public void GivenNewCourseWithNeitherSessionNorCoursePrice_WhenPost_ThenReturnWithNoPriceErrorResponse()
        {
            var command = GivenNewCourseWithNeitherSessionNorCoursePrice();
            var response = WhenPost(command);
            AssertSingleError(response, "At least a session or course price must be specified.", "session.pricing");
        }

        [Test]
        public void GivenNewCourseWithCoursePriceOnly_WhenPost_ThenCreatesCourseWithCoursePriceOnly()
        {
            var command = GivenNewCourseWithCoursePriceOnly();
            var response = WhenPost(command);
            ThenCreatesCourseWithCoursePriceOnly(response);
        }

        [Test]
        public void GivenNewCourseWithSessionPriceOnly_WhenPost_ThenCreatesCourseWithSessionPriceOnly()
        {
            var command = GivenNewCourseWithSessionPriceOnly();
            var response = WhenPost(command);
            ThenCreatesCourseWithSessionPriceOnly(response);
        }

        [Test]
        public void GivenNewCourseWithBothSessionPriceAndCoursePrice_WhenPost_ThenCreatesCourseWithSessionPriceAndCoursePrice()
        {
            var command = GivenNewCourseWithBothSessionPriceAndCoursePrice();
            var response = WhenPost(command);
            ThenCreatesCourseWithSessionPriceAndCoursePrice(response);
        }

        [Test]
        public void GivenNewCourseWithZeroSessionPrice_WhenPost_ThenCreatesCourseWithZeroSessionPrice()
        {
            var command = GivenNewCourseWithZeroSessionPrice();
            var response = WhenPost(command);
            ThenCreatesCourseWithZeroSessionPrice(response);
        }

        [Test]
        public void GivenNewCourseWith24HourStartTime_WhenPost_ThenCreatesCourseWith24HrStartTime()
        {
            var command = GivenNewCourseWith24HourStartTime();
            var response = WhenPost(command);
            ThenCreatesCourseWith24HrStartTime(response);
        }


        private ApiSessionSaveCommand GivenNewCourseClashesWithStandaloneSession()
        {
            var command = CreateSessionSaveCommandAaronOrakei14To15();

            command.location = new ApiLocationKey { id = RemueraId };
            command.service = new ApiServiceKey { id = MiniBlueId };

            command.timing.startTime = "14:30";

            command.booking = new ApiSessionBooking { studentCapacity = 10, isOnlineBookable = false };
            command.repetition = new ApiRepetition { sessionCount = 6, repeatFrequency = "w" };
            command.pricing = new ApiPricing { sessionPrice = 15 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithTooManySessions()
        {
            return new ApiSessionSaveCommand
            {
                service = new ApiServiceKey { id = MiniGreenId },
                location = new ApiLocationKey { id = RemueraId },
                coach = new ApiCoachKey { id = AaronId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "03:30", duration = 30 },
                booking = new ApiSessionBooking { studentCapacity = 10, isOnlineBookable = false },
                pricing = new ApiPricing { sessionPrice = 20 },
                repetition = new ApiRepetition { sessionCount = 100, repeatFrequency = "d" },
                presentation = new ApiPresentation { colour = "green" }
            };
        }

        private ApiSessionSaveCommand GivenNewCourseCommand()
        {
            return new ApiSessionSaveCommand
            {
                service = new ApiServiceKey { id = MiniGreenId },
                location = new ApiLocationKey { id = RemueraId },
                coach = new ApiCoachKey { id = AaronId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "2:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 10, isOnlineBookable = false },
                pricing = new ApiPricing { sessionPrice = 12 },
                repetition = new ApiRepetition { sessionCount = 2, repeatFrequency = "d" },
                presentation = new ApiPresentation { colour = "green" }
            };
        }

        private ApiSessionSaveCommand GivenNewCourseWithNeitherSessionNorCoursePrice()
        {
            var command = GivenNewCourseCommand();
            command.pricing = new ApiPricing { sessionPrice = null, coursePrice = null };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithCoursePriceOnly()
        {
            var command = GivenNewCourseCommand();
            command.pricing = new ApiPricing { sessionPrice = null, coursePrice = 50 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithSessionPriceOnly()
        {
            var command = GivenNewCourseCommand();
            command.pricing = new ApiPricing { sessionPrice = 12.5m, coursePrice = null };

            return command;
        }
        private ApiSessionSaveCommand GivenNewCourseWithBothSessionPriceAndCoursePrice()
        {
            var command = GivenNewCourseCommand();
            command.pricing = new ApiPricing { sessionPrice = 12.5m, coursePrice = 75 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWithZeroSessionPrice()
        {
            var command = GivenNewCourseCommand();
            command.pricing = new ApiPricing { sessionPrice = 0 };

            return command;
        }

        private ApiSessionSaveCommand GivenNewCourseWith24HourStartTime()
        {
            var command = GivenNewCourseCommand();
            command.timing.startTime = "21:30";

            return command;
        }


        private SessionData ThenCreatesCourseWithCoursePriceOnly(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);
            AssertSessionPricing(session.pricing, null, 50);

            return session;
        }

        private SessionData ThenCreatesCourseWithSessionPriceOnly(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);
            AssertSessionPricing(session.pricing, 12.5m, null);

            return session;
        }

        private SessionData ThenCreatesCourseWithSessionPriceAndCoursePrice(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);
            AssertSessionPricing(session.pricing, 12.5m, 75);

            return session;
        }

        private SessionData ThenCreatesCourseWithZeroSessionPrice(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);
            AssertSessionPricing(session.pricing, 0, null);

            return session;
        }

        private SessionData ThenCreatesCourseWith24HrStartTime(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));

            AssertSessionLocation(session.location, RemueraId, "Remuera Racquets Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniGreenId, "Mini Green");

            AssertSessionTiming(session.timing, GetFormattedDateOneWeekOut(), "21:30", 60);
            AssertSessionBooking(session.booking, 10, false);
            AssertSessionRepetition(session.repetition, 2, "d");
            AssertSessionPricing(session.pricing, 12, null);
            AssertSessionPresentation(session.presentation, "green");

            return session;
        }
    }
}
