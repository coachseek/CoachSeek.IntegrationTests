using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BookingPostTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }

        protected override string RelativePath
        {
            get { return "Bookings"; }
        }

        [TestFixture]
        public class BookingCommandTests : BookingPostTests
        {
            [Test]
            public void GivenNoBookingSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
            {
                var command = GivenNoBookingSaveCommand();
                var response = WhenPost(command);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptyBookingSaveCommand_WhenPost_ThenReturnMultipleErrors()
            {
                var command = GivenEmptyBookingSaveCommand();
                var response = WhenPost(command);
                AssertMultipleErrors(response, new[,] { { "The session field is required.", "booking.session" },
                                                        { "The customer field is required.", "booking.customer" } });
            }

            private string GivenNoBookingSaveCommand()
            {
                return "";
            }

            private string GivenEmptyBookingSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class BookingNewTests : BookingPostTests
        {
            [Test]
            public void GivenNonExistentSession_WhenPost_ThenReturNonExistentSessionError()
            {
                var command = GivenNonExistentSession();
                var response = WhenPost(command);
                ThenReturNonExistentSessionError(response);
            }
            [Test]
            public void GivenNonExistentCustomer_WhenPost_ThenReturNonExistentCustomerError()
            {
                var command = GivenNonExistentCustomer();
                var response = WhenPost(command);
                ThenReturNonExistentCustomerError(response);
            }




            private ApiBookingSaveCommand GivenNonExistentSession()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = Guid.NewGuid() },
                    customer = new ApiCustomerKey { id = FredId }
                };
            }

            private ApiBookingSaveCommand GivenNonExistentCustomer()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = AaronOrakei2To3SessionId },
                    customer = new ApiCustomerKey { id = Guid.NewGuid() }
                };
            }

            private void ThenReturNonExistentSessionError(Response response)
            {
                AssertSingleError(response, "This session does not exist.", "booking.session.id");
            }

            private void ThenReturNonExistentCustomerError(Response response)
            {
                AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
            }
        }




        //[TestFixture]
        //public class SessionExistingTests : SessionPostTests
        //{
        //    [Test]
        //    public void GivenNonExistentSessionId_WhenPost_ThenReturnInvalidSessionIdError()
        //    {
        //        var command = GivenNonExistentSessionId();
        //        var response = WhenPost(command);
        //        AssertSingleError(response, "This session does not exist.", "session.id");
        //    }

        //    [Test]
        //    public void GivenSessionClashesWithItself_WhenPost_ThenSessionWasUpdatedResponse()
        //    {
        //        var command = ChangeTimeForSessionCoachedByAaronTo("14:30");
        //        var response = WhenPost(command);
        //        ThenSessionWasUpdatedResponse(response, "14:30");
        //    }

        //    [Test]
        //    public void GivenSessionWillNotClash_WhenPost_ThenSessionWasUpdatedResponse()
        //    {
        //        var command = ChangeTimeForSessionCoachedByAaronTo("11:30");
        //        var response = WhenPost(command);
        //        ThenSessionWasUpdatedResponse(response, "11:30");
        //    }

        //    [Test]
        //    public void GivenSessionClashesWithAnotherSession_WhenPost_ThenReturnSessionClashErrorResponse()
        //    {
        //        var command = GivenSessionClashesWithAnotherSession();
        //        var response = WhenPost(command);
        //        AssertSingleError(response, "This session clashes with one or more sessions.");
        //    }

        //    [Test]
        //    public void GivenSessionClashesWithAnotherSessionInCourse_WhenPost_ThenReturnSessionClashErrorResponse()
        //    {
        //        var command = GivenSessionClashesWithAnotherSessionInCourse();
        //        var response = WhenPost(command);
        //        AssertSingleError(response, "This session clashes with one or more sessions.");
        //    }



        //    private ApiSessionSaveCommand GivenNonExistentSessionId()
        //    {
        //        var session = GivenExistingSession();
        //        session.id = Guid.NewGuid();

        //        return session;
        //    }

        //    private ApiSessionSaveCommand GivenExistingSession()
        //    {
        //        return new ApiSessionSaveCommand
        //        {
        //            id = AaronOrakei2To3SessionId,
        //            location = new ApiLocationKey { id = OrakeiId },
        //            coach = new ApiCoachKey { id = AaronId },
        //            service = new ApiServiceKey { id = MiniRedId },
        //            timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "14:00", duration = 60 }
        //        };
        //    }

        //    private ApiSessionSaveCommand GivenSessionClashesWithAnotherSession()
        //    {
        //        // Should clash with AaronOrakei4To5Session
        //        return new ApiSessionSaveCommand
        //        {
        //            id = AaronOrakei2To3SessionId,
        //            location = new ApiLocationKey { id = OrakeiId },
        //            coach = new ApiCoachKey { id = AaronId },
        //            service = new ApiServiceKey { id = MiniRedId },
        //            timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "15:30", duration = 60 }
        //        };
        //    }

        //    private ApiSessionSaveCommand GivenSessionClashesWithAnotherSessionInCourse()
        //    {
        //        return new ApiSessionSaveCommand
        //        {
        //            id = AaronOrakei2To3SessionId,
        //            coach = new ApiCoachKey { id = AaronId },
        //            location = new ApiLocationKey { id = RemueraId },
        //            service = new ApiServiceKey { id = MiniBlueId },
        //            timing = new ApiSessionTiming
        //            {
        //                startDate = GetDateFormatNumberOfWeeksOut(3),
        //                startTime = "9:45",
        //                duration = 60
        //            },
        //            booking = new ApiSessionBooking
        //            {
        //                studentCapacity = 1,
        //                isOnlineBookable = false
        //            },
        //            pricing = new ApiPricing
        //            {
        //                sessionPrice = 60
        //            }
        //        };
        //    }

        //    private ApiSessionSaveCommand ChangeTimeForSessionCoachedByAaronTo(string startTime)
        //    {
        //        return new ApiSessionSaveCommand
        //        {
        //            id = AaronOrakei2To3SessionId,
        //            location = new ApiLocationKey { id = OrakeiId },
        //            coach = new ApiCoachKey { id = AaronId },
        //            service = new ApiServiceKey { id = MiniRedId },
        //            timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = startTime, duration = 60 }
        //        };
        //    }

        //    private SessionData ThenSessionWasUpdatedResponse(Response response, string startTime)
        //    {
        //        Assert.That(response, Is.Not.Null);
        //        AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

        //        Assert.That(response.Payload, Is.InstanceOf<SessionData>());
        //        var session = (SessionData)response.Payload;

        //        Assert.That(session, Is.Not.Null);

        //        Assert.That(session.id, Is.EqualTo(AaronOrakei2To3SessionId));
        //        Assert.That(session.location, Is.Not.Null);
        //        Assert.That(session.location.id, Is.EqualTo(OrakeiId));
        //        Assert.That(session.coach, Is.Not.Null);
        //        Assert.That(session.coach.id, Is.EqualTo(AaronId));
        //        Assert.That(session.service, Is.Not.Null);
        //        Assert.That(session.service.id, Is.EqualTo(MiniRedId));

        //        var timing = session.timing;
        //        Assert.That(timing, Is.Not.Null);
        //        Assert.That(timing.startDate, Is.EqualTo(GetFormattedDateOneWeekOut()));
        //        Assert.That(timing.startTime, Is.EqualTo(startTime));
        //        Assert.That(timing.duration, Is.EqualTo(60));

        //        var booking = session.booking;
        //        Assert.That(booking, Is.Not.Null);
        //        Assert.That(booking.studentCapacity, Is.EqualTo(13));
        //        Assert.That(booking.isOnlineBookable, Is.True);

        //        var pricing = session.pricing;
        //        Assert.That(pricing, Is.Not.Null);
        //        Assert.That(pricing.sessionPrice, Is.EqualTo(19.95));
        //        Assert.That(pricing.coursePrice, Is.Null);

        //        var repetition = session.repetition;
        //        Assert.That(repetition, Is.Not.Null);
        //        Assert.That(repetition.sessionCount, Is.EqualTo(1));
        //        Assert.That(repetition.repeatFrequency, Is.Null);

        //        var presentation = session.presentation;
        //        Assert.That(presentation, Is.Not.Null);
        //        Assert.That(presentation.colour, Is.EqualTo("red"));

        //        return session;
        //    }
        //}


        private Response WhenPost(string json)
        {
            return Post<BookingData>(json);
        }

        private Response WhenPost(ApiBookingSaveCommand command)
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
