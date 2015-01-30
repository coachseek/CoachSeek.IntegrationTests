﻿using System;
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
            public void GivenSessionWithValues_WhenPost_ThenOverrideServiceDefaults()
            {
                var command = GivenSessionWithValues();
                var response = WhenPost(command);
                ThenOverrideServiceDefaults(response);
            }

            [Test]
            public void GivenSessionWithMissingValues_WhenPost_ThenGetServiceDefaults()
            {
                var command = GivenSessionWithMissingValues();
                var response = WhenPost(command);
                ThenGetServiceDefaults(response);
            }

            [Test]
            public void GivenSessionClashesWithExistingSession_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithExistingSession();
                var response = WhenPost(command);
                AssertSingleError(response, "This session clashes with one or more sessions.");
            }

            [Test]
            public void GivenSessionClashesWithExistingCourse_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithExistingCourse();
                var response = WhenPost(command);
                AssertSingleError(response, "This session clashes with one or more sessions.");
            }

            [Test]
            public void GivenCourseClashesWithExistingSession_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenCourseClashesWithExistingSession();
                var response = WhenPost(command);
                AssertSingleError(response, "This session clashes with one or more sessions.");
            }

            [Test]
            public void GivenSessionWillNotClash_WhenPost_ThenSessionWasUpdatedResponse()
            {
                var command = CreateSessionCoachedByAaronAt("10:45");
                var response = WhenPost(command);
                ThenSessionWasCreatedResponse(response, "10:45");
            }

            [Test]
            public void GivenNewCourseWithZeroSessionPrice_WhenPost_ThenCourseWasUpdatedResponse()
            {
                var command = GivenNewCourseWithZeroSessionPrice();
                var response = WhenPost(command);
                ThenCourseWasCreatedResponse(response);
            }


            private ApiSessionSaveCommand GivenSessionWithValues()
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

            private ApiSessionSaveCommand GivenSessionWithMissingValues()
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

            private ApiSessionSaveCommand GivenSessionClashesWithExistingSession()
            {
                return new ApiSessionSaveCommand
                {
                    coach = new ApiCoachKey { id = AaronId },
                    location = new ApiLocationKey { id = OrakeiId },
                    service = new ApiServiceKey { id = MiniBlueId },
                    timing = new ApiSessionTiming
                    {
                        startDate = GetFormattedDateOneWeekOut(),
                        startTime = "16:30",
                        duration = 60
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

            private ApiSessionSaveCommand GivenSessionClashesWithExistingCourse()
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

            private ApiSessionSaveCommand GivenCourseClashesWithExistingSession()
            {
                return new ApiSessionSaveCommand
                {
                    coach = new ApiCoachKey { id = AaronId },
                    location = new ApiLocationKey { id = OrakeiId },
                    service = new ApiServiceKey { id = MiniBlueId },
                    timing = new ApiSessionTiming
                    {
                        startDate = GetDateFormatNumberOfWeeksOut(1),
                        startTime = "13:30",
                        duration = 60
                    },
                    booking = new ApiSessionBooking
                    {
                        studentCapacity = 4,
                        isOnlineBookable = false
                    },
                    presentation = new ApiPresentation
                    {
                        colour = "blue" 
                    },
                    repetition = new ApiRepetition
                    {
                        sessionCount = 6,
                        repeatFrequency = "2w"
                    },
                    pricing = new ApiPricing
                    {
                        sessionPrice = 30
                    }
                };
            }

            private ApiSessionSaveCommand GivenNewCourseWithZeroSessionPrice()
            {
                return new ApiSessionSaveCommand
                {
                    service = new ApiServiceKey { id = MiniGreenId },
                    location = new ApiLocationKey { id = RemueraId },
                    coach = new ApiCoachKey { id = AaronId },
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "01:30", duration = 60 },
                    pricing = new ApiPricing { sessionPrice = 0 },
                    repetition = new ApiRepetition { sessionCount = 2, repeatFrequency = "d" },
                    booking = new ApiSessionBooking { studentCapacity = 51, isOnlineBookable = false },
                };
            }

            private ApiSessionSaveCommand CreateSessionCoachedByAaronAt(string startTime)
            {
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei2To3SessionId,
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

                Assert.That(session.id, Is.EqualTo(AaronOrakei2To3SessionId));
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

            private SessionData ThenCourseWasCreatedResponse(Response response)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<SessionData>());
                var session = (SessionData)response.Payload;

                Assert.That(session, Is.Not.Null);

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
                Assert.That(timing.startTime, Is.EqualTo("1:30"));
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
        }


        [TestFixture]
        public class SessionExistingTests : SessionPostTests
        {
            [Test]
            public void GivenNonExistentSessionId_WhenPost_ThenReturnInvalidSessionIdError()
            {
                var command = GivenNonExistentSessionId();
                var response = WhenPost(command);
                AssertSingleError(response, "This session does not exist.", "session.id");
            }

            [Test]
            public void GivenSessionClashesWithItself_WhenPost_ThenSessionWasUpdatedResponse()
            {
                var command = ChangeTimeForSessionCoachedByAaronTo("14:30");
                var response = WhenPost(command);
                ThenSessionWasUpdatedResponse(response, "14:30");
            }

            [Test]
            public void GivenSessionWillNotClash_WhenPost_ThenSessionWasUpdatedResponse()
            {
                var command = ChangeTimeForSessionCoachedByAaronTo("11:30");
                var response = WhenPost(command);
                ThenSessionWasUpdatedResponse(response, "11:30");
            }

            [Test]
            public void GivenSessionClashesWithAnotherSession_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithAnotherSession();
                var response = WhenPost(command);
                AssertSingleError(response, "This session clashes with one or more sessions.");
            }

            [Test]
            public void GivenSessionClashesWithAnotherSessionInCourse_WhenPost_ThenReturnSessionClashErrorResponse()
            {
                var command = GivenSessionClashesWithAnotherSessionInCourse();
                var response = WhenPost(command);
                AssertSingleError(response, "This session clashes with one or more sessions.");
            }



            private ApiSessionSaveCommand GivenNonExistentSessionId()
            {
                var session = GivenExistingSession();
                session.id = Guid.NewGuid();

                return session;
            }

            private ApiSessionSaveCommand GivenExistingSession()
            {
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei2To3SessionId,
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    service = new ApiServiceKey { id = MiniRedId },
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "14:00", duration = 60 }
                };
            }

            private ApiSessionSaveCommand GivenSessionClashesWithAnotherSession()
            {
                // Should clash with AaronOrakei4To5Session
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei2To3SessionId,
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    service = new ApiServiceKey { id = MiniRedId },
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "15:30", duration = 60 }
                };
            }

            private ApiSessionSaveCommand GivenSessionClashesWithAnotherSessionInCourse()
            {
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei2To3SessionId,
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
                return new ApiSessionSaveCommand
                {
                    id = AaronOrakei2To3SessionId,
                    location = new ApiLocationKey { id = OrakeiId },
                    coach = new ApiCoachKey { id = AaronId },
                    service = new ApiServiceKey { id = MiniRedId },
                    timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = startTime, duration = 60 }
                };
            }

            private SessionData ThenSessionWasUpdatedResponse(Response response, string startTime)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<SessionData>());
                var session = (SessionData)response.Payload;

                Assert.That(session, Is.Not.Null);

                Assert.That(session.id, Is.EqualTo(AaronOrakei2To3SessionId));
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
