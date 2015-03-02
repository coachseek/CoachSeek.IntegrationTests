using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    public abstract class SessionPostTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }

        protected override string RelativePath
        {
            get { return "Sessions"; }
        }

        [TestFixture]
        public class SessionCommandTests : SessionPostTests
        {
            [Test]
            public void GivenNoSessionSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
            {
                var command = GivenNoSessionSaveCommand();
                var response = WhenPost(command);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptySessionSaveCommand_WhenPost_ThenReturnMultipleErrors()
            {
                var command = GivenEmptySessionSaveCommand();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The service field is required.", "session.service" },
                                                        { "The location field is required.", "session.location" },
                                                        { "The coach field is required.", "session.coach" },
                                                        { "The timing field is required.", "session.timing" } });
            }

            private string GivenNoSessionSaveCommand()
            {
                return "";
            }

            private string GivenEmptySessionSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class SessionNewTests : SessionPostTests
        {
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
            public void GivenNewSessionWithValues_WhenPost_ThenCreateSessionAndOverrideServiceDefaults()
            {
                var command = GivenNewSessionWithValues();
                var response = WhenPost(command);
                ThenOverrideServiceDefaults(response);
            }

            [Test]
            public void GivenNewSessionWithMissingValues_WhenPost_ThenCreateSessionAndUseServiceDefaults()
            {
                var command = GivenNewSessionWithMissingValues();
                var response = WhenPost(command);
                ThenGetServiceDefaults(response);
            }


            private ApiSessionSaveCommand GivenNewSessionWithValues()
            {
                return new ApiSessionSaveCommand
                {
                    service = new ApiServiceKey { id = MiniRedId },
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    timing = new ApiSessionTiming
                    {
                        startDate = "2014-11-09",
                        startTime = "15:30",
                        duration = 45
                    },
                    booking = new ApiSessionBooking
                    {
                        studentCapacity = 10,
                        isOnlineBookable = true
                    },
                    pricing = new ApiPricing
                    {
                        sessionPrice = 12
                    },
                    presentation = new ApiPresentation
                    {
                        colour = "Red"
                    }
                };
            }

            private ApiSessionSaveCommand GivenNewSessionWithMissingValues()
            {
                return new ApiSessionSaveCommand
                {
                    service = new ApiServiceKey { id = MiniRedId },
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    timing = new ApiSessionTiming
                    {
                        startDate = "2014-11-11",
                        startTime = "16:45"
                    }
                };
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
                    timing = new ApiSessionTiming
                    {
                        startDate = GetFormattedDateTwoWeeksOut(),
                        startTime = "9:30",
                        duration = 30
                    },
                    booking = new ApiSessionBooking
                    {
                        studentCapacity = 9,
                        isOnlineBookable = true
                    },
                    pricing = new ApiPricing
                    {
                        sessionPrice = 25
                    }
                };
            }

            private ApiSessionSaveCommand CreateSessionCoachedByAaronAt(string startTime)
            {
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei14To15SessionId,
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    service = new ApiServiceKey { id = MiniRedId },
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = startTime, duration = 60 }
                };
            }

            private SessionData ThenSessionWasCreatedResponse(Response response, string startTime)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<SessionData>());
                var session = (SessionData)response.Payload;

                Assert.That(session, Is.Not.Null);

                Assert.That(session.parentId, Is.Null);
                Assert.That(session.id, Is.EqualTo(AaronOrakei14To15SessionId));
                Assert.That(session.location, Is.Not.Null);
                Assert.That(session.location.id, Is.EqualTo(OrakeiId));
                Assert.That(session.coach, Is.Not.Null);
                Assert.That(session.coach.id, Is.EqualTo(AaronId));
                Assert.That(session.service, Is.Not.Null);
                Assert.That(session.service.id, Is.EqualTo(MiniRedId));

                var timing = session.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.startDate, Is.EqualTo(GetFormattedDateOneWeekOut()));
                Assert.That(timing.startTime, Is.EqualTo(startTime));
                Assert.That(timing.duration, Is.EqualTo(60));

                var booking = session.booking;
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(13));
                Assert.That(booking.isOnlineBookable, Is.True);

                var pricing = session.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(19.95));
                Assert.That(pricing.coursePrice, Is.Null);

                var repetition = session.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(1));
                Assert.That(repetition.repeatFrequency, Is.Null);

                var presentation = session.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("red"));

                return session;
            }
        }

        [TestFixture]
        public class CourseNewTests : SessionPostTests
        {
            [Test]
            public void GivenNewCourseClashesWithStandaloneSession_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenNewCourseClashesWithStandaloneSession();
                var response = WhenPost(command);
                var error = AssertSingleError(response, "This session clashes with one or more sessions.");
                Assert.That(error.data, Is.StringContaining(AaronOrakei14To15SessionId.ToString()));
            }

            [Test]
            public void GivenNewCourseWithTooManySessions_WhenPost_ThenTooManySessionInCourseErrorResponse()
            {
                var command = GivenNewCourseWithTooManySessions();
                var response = WhenPost(command);
                AssertSingleError(response, "The maximum number of daily sessions is 30.", "session.repetition.sessionCount");
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
                    pricing = new ApiPricing { sessionPrice = 20 },
                    repetition = new ApiRepetition { sessionCount = 100, repeatFrequency = "d" },
                    booking = new ApiSessionBooking { studentCapacity = 10, isOnlineBookable = false },
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
                    pricing = new ApiPricing { sessionPrice = 12 },
                    repetition = new ApiRepetition { sessionCount = 2, repeatFrequency = "d" },
                    booking = new ApiSessionBooking { studentCapacity = 51, isOnlineBookable = false },
                };
            }

            private ApiSessionSaveCommand GivenNewCourseWithZeroSessionPrice()
            {
                var command = GivenNewCourseCommand();

                command.pricing = new ApiPricing {sessionPrice = 0};

                return command;
            }

            private ApiSessionSaveCommand GivenNewCourseWith24HourStartTime()
            {
                var command = GivenNewCourseCommand();

                command.timing.startTime = "21:30";

                return command;
            }


            private SessionData ThenCreatesCourseWithZeroSessionPrice(Response response)
            {
                var session = AssertSuccessResponse<SessionData>(response);

                Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(session.location, Is.Not.Null);
                Assert.That(session.location.id, Is.EqualTo(RemueraId));
                Assert.That(session.coach, Is.Not.Null);
                Assert.That(session.coach.id, Is.EqualTo(AaronId));
                Assert.That(session.service, Is.Not.Null);
                Assert.That(session.service.id, Is.EqualTo(MiniGreenId));

                var timing = session.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.startDate, Is.EqualTo(GetFormattedDateOneWeekOut()));
                Assert.That(timing.startTime, Is.EqualTo("2:00"));
                Assert.That(timing.duration, Is.EqualTo(60));

                var booking = session.booking;
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(51));
                Assert.That(booking.isOnlineBookable, Is.False);

                var pricing = session.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(0));
                Assert.That(pricing.coursePrice, Is.EqualTo(60));

                var repetition = session.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(2));
                Assert.That(repetition.repeatFrequency, Is.EqualTo("d"));

                var presentation = session.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("green"));

                return session;
            }

            private SessionData ThenCreatesCourseWith24HrStartTime(Response response)
            {
                var session = AssertSuccessResponse<SessionData>(response);

                Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(session.location, Is.Not.Null);
                Assert.That(session.location.id, Is.EqualTo(RemueraId));
                Assert.That(session.coach, Is.Not.Null);
                Assert.That(session.coach.id, Is.EqualTo(AaronId));
                Assert.That(session.service, Is.Not.Null);
                Assert.That(session.service.id, Is.EqualTo(MiniGreenId));

                var timing = session.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.startDate, Is.EqualTo(GetFormattedDateOneWeekOut()));
                Assert.That(timing.startTime, Is.EqualTo("21:30"));
                Assert.That(timing.duration, Is.EqualTo(60));

                var booking = session.booking;
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(51));
                Assert.That(booking.isOnlineBookable, Is.False);

                var pricing = session.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(12));
                Assert.That(pricing.coursePrice, Is.EqualTo(60));

                var repetition = session.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(2));
                Assert.That(repetition.repeatFrequency, Is.EqualTo("d"));

                var presentation = session.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("green"));

                return session;
            }
        }


        [TestFixture]
        public class SessionExistingTests : SessionPostTests
        {
            [Test]
            public void GivenNonExistentSessionId_WhenPost_ThenReturnNotFoundResponse()
            {
                var command = GivenNonExistentSessionId();
                var response = WhenPost(command);
                AssertNotFound(response);
            }

            [Test]
            public void GivenUpdatedSessionClashesWithItself_WhenPost_ThenSessionWasUpdatedResponse()
            {
                var command = GivenUpdatedSessionClashesWithItself();
                var response = WhenPost(command);
                ThenSessionWasUpdatedResponse(response, GetDateFormatNumberOfWeeksOut(3), "14:30");
            }

            [Test]
            public void GivenSessionWillNotClash_WhenPost_ThenSessionWasUpdatedResponse()
            {
                var command = ChangeTimeForSessionCoachedByAaronTo("11:30");
                var response = WhenPost(command);
                ThenSessionWasUpdatedResponse(response, GetDateFormatNumberOfWeeksOut(3), "11:30");
            }

            [Test]
            public void GivenSessionClashesWithAnotherSession_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithAnotherSession();
                var response = WhenPost(command);
                var error = AssertSingleError(response, "This session clashes with one or more sessions.");
                Assert.That(error.data, Is.StringContaining(AaronOrakei14To15SessionId.ToString()));
            }

            [Test]
            public void GivenSessionClashesWithAnotherSessionInCourse_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithAnotherSessionInCourse();
                var response = WhenPost(command);
                var error = AssertSingleError(response, "This session clashes with one or more sessions.");
                Assert.That(error.data, Is.StringContaining(AaronRemuera9To10For8WeeksSessionIds[2].ToString()));
            }

            [Test]
            public void GivenWantTurnSessionIntoCourse_WhenPost_ThenReturnsCannotChangeSessionToCourseError()
            {
                var command = GivenWantTurnSessionIntoCourse();
                var response = WhenPost(command);
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
                AssertSingleError(response, "Cannot change from a standalone session to a course.");
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
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "14:00", duration = 60 }
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
                    timing = new ApiSessionTiming
                    {
                        startDate = GetDateFormatNumberOfWeeksOut(3),
                        startTime = "9:45",
                        duration = 60
                    },
                    booking = new ApiSessionBooking
                    {
                        studentCapacity = 1,
                        isOnlineBookable = false
                    },
                    pricing = new ApiPricing
                    {
                        sessionPrice = 60
                    }
                };
            }

            private ApiSessionSaveCommand ChangeTimeForSessionCoachedByAaronTo(string startTime)
            {
                var command = CreateSessionSaveCommandAaronOrakei14To15();

                command.id = AaronOrakei14To15SessionId;
                command.booking = new ApiSessionBooking {studentCapacity = 13, isOnlineBookable = true};
                command.repetition = new ApiRepetition { sessionCount = 1 };
                command.presentation = new ApiPresentation { colour = "red" };
                command.pricing = new ApiPricing { sessionPrice = 19.95m };
                command.timing.startTime = startTime;

                return command;
            }

            private SessionData ThenSessionWasUpdatedResponse(Response response, string startDate, string startTime)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<SessionData>());
                var session = (SessionData)response.Payload;

                Assert.That(session, Is.Not.Null);

                Assert.That(session.id, Is.EqualTo(AaronOrakei14To15SessionId));
                Assert.That(session.location, Is.Not.Null);
                Assert.That(session.location.id, Is.EqualTo(OrakeiId));
                Assert.That(session.coach, Is.Not.Null);
                Assert.That(session.coach.id, Is.EqualTo(AaronId));
                Assert.That(session.service, Is.Not.Null);
                Assert.That(session.service.id, Is.EqualTo(MiniRedId));

                var timing = session.timing;
                Assert.That(timing, Is.Not.Null);
                Assert.That(timing.startDate, Is.EqualTo(startDate));
                Assert.That(timing.startTime, Is.EqualTo(startTime));
                Assert.That(timing.duration, Is.EqualTo(60));

                var booking = session.booking;
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.studentCapacity, Is.EqualTo(13));
                Assert.That(booking.isOnlineBookable, Is.True);

                var pricing = session.pricing;
                Assert.That(pricing, Is.Not.Null);
                Assert.That(pricing.sessionPrice, Is.EqualTo(19.95));
                Assert.That(pricing.coursePrice, Is.Null);

                var repetition = session.repetition;
                Assert.That(repetition, Is.Not.Null);
                Assert.That(repetition.sessionCount, Is.EqualTo(1));
                Assert.That(repetition.repeatFrequency, Is.Null);

                var presentation = session.presentation;
                Assert.That(presentation, Is.Not.Null);
                Assert.That(presentation.colour, Is.EqualTo("red"));

                return session;
            }
        }

        [TestFixture]
        public class CourseExistingTests : SessionPostTests
        {
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
                courseCommand.repetition = new ApiRepetition {sessionCount = 1};

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
                courseCommand.location = new ApiLocationKey {id = RemueraId};

                return courseCommand;
            }

            private void ThenReturnsCannotUpdateRepetitionOfCourseError(Response response)
            {
                AssertSingleError(response, "Cannot change the repetition of a course.");
            }

            private void ThenReturnsCannotUpdateCourseError(Response response)
            {
                AssertSingleError(response, "Course updates are not working yet.");
            }
        }


        private Response WhenPost(string json)
        {
            return Post<SessionData>(json);
        }

        private Response WhenPost(ApiSessionSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenPost(json);
        }


        private SessionData ThenOverrideServiceDefaults(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<SessionData>());
            var session = (SessionData)response.Payload;

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.location, Is.Not.Null);
            Assert.That(session.location.id, Is.EqualTo(OrakeiId));
            Assert.That(session.coach, Is.Not.Null);
            Assert.That(session.coach.id, Is.EqualTo(AaronId));
            Assert.That(session.service, Is.Not.Null);
            Assert.That(session.service.id, Is.EqualTo(MiniRedId));

            var timing = session.timing;
            Assert.That(timing, Is.Not.Null);
            Assert.That(timing.startDate, Is.EqualTo("2014-11-09"));
            Assert.That(timing.startTime, Is.EqualTo("15:30"));
            Assert.That(timing.duration, Is.EqualTo(45));

            var booking = session.booking;
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.studentCapacity, Is.EqualTo(10));
            Assert.That(booking.isOnlineBookable, Is.True);

            var pricing = session.pricing;
            Assert.That(pricing, Is.Not.Null);
            Assert.That(pricing.sessionPrice, Is.EqualTo(12));
            Assert.That(pricing.coursePrice, Is.Null);

            var repetition = session.repetition;
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.sessionCount, Is.EqualTo(1));
            Assert.That(repetition.repeatFrequency, Is.Null);

            var presentation = session.presentation;
            Assert.That(presentation, Is.Not.Null);
            Assert.That(presentation.colour, Is.EqualTo("red"));

            return session;
        }

        private SessionData ThenGetServiceDefaults(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<SessionData>());
            var session = (SessionData)response.Payload;

            Assert.That(session, Is.Not.Null);
            Assert.That(session.parentId, Is.Null);
            Assert.That(session.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.location, Is.Not.Null);
            Assert.That(session.location.id, Is.EqualTo(OrakeiId));
            Assert.That(session.coach, Is.Not.Null);
            Assert.That(session.coach.id, Is.EqualTo(AaronId));
            Assert.That(session.service, Is.Not.Null);
            Assert.That(session.service.id, Is.EqualTo(MiniRedId));

            var timing = session.timing;
            Assert.That(timing, Is.Not.Null);
            Assert.That(timing.startDate, Is.EqualTo("2014-11-11"));
            Assert.That(timing.startTime, Is.EqualTo("16:45"));
            Assert.That(timing.duration, Is.EqualTo(75));

            var booking = session.booking;
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.studentCapacity, Is.EqualTo(13));
            Assert.That(booking.isOnlineBookable, Is.True);

            var pricing = session.pricing;
            Assert.That(pricing, Is.Not.Null);
            Assert.That(pricing.sessionPrice, Is.EqualTo(19.95));
            Assert.That(pricing.coursePrice, Is.Null);

            var repetition = session.repetition;
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.sessionCount, Is.EqualTo(1));
            Assert.That(repetition.repeatFrequency, Is.Null);

            var presentation = session.presentation;
            Assert.That(presentation, Is.Not.Null);
            Assert.That(presentation.colour, Is.EqualTo("red"));

            return session;
        }
    }
}
