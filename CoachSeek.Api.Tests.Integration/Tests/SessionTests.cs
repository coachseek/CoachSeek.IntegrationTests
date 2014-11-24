using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class SessionTests : ScheduleTests
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
            AssertMultipleErrors(response, new[,] { { "The businessId field is required.", "session.businessId" }, 
                                                    { "The service field is required.", "session.service" },
                                                    { "The location field is required.", "session.location" },
                                                    { "The coach field is required.", "session.coach" },
                                                    { "The timing field is required.", "session.timing" } });
        }

        [Test]
        public void GivenNonExistentBusinessId_WhenPost_ThenReturnInvalidBusinessIdError()
        {
            var command = GivenNonExistentBusinessId();
            var response = WhenPost(command);
            AssertSingleError(response, "This business does not exist.", "session.businessId");
        }

        [Test]
        public void GivenSingleSessionWithValues_WhenPost_ThenOverrideServiceDefaults()
        {
            var command = GivenSingleSessionWithValues();
            var response = WhenPost(command);
            ThenOverrideServiceDefaults(response);
        }

        [Test]
        public void GivenSingleSessionMissingValues_WhenPost_ThenGetServiceDefaults()
        {
            var command = GivenSingleSessionMissingValues();
            var response = WhenPost(command);
            ThenGetServiceDefaults(response);
        }

        [Test]
        public void GivenSessionClashesWithExistingSession_WhenPost_ThenReturnSessionClashErrorResponse()
        {
            var command = GivenClashingSingleSession();
            var response = WhenPost(command);
            AssertSingleError(response, "This session clashes with another session.");
        }


        private string GivenNoSessionSaveCommand()
        {
            return "";
        }

        private string GivenEmptySessionSaveCommand()
        {
            return "{}";
        }

        private ApiSessionSaveCommand GivenNonExistentBusinessId()
        {
            return new ApiSessionSaveCommand
            {
                businessId = Guid.Empty,
                service = new ApiServiceKey { id = Guid.NewGuid() },
                location = new ApiLocationKey { id = Guid.NewGuid() },
                coach = new ApiCoachKey { id = Guid.NewGuid() },
                timing = new ApiSessionTiming { startDate = RandomString, startTime = RandomString }
            };
        }

        private ApiSessionSaveCommand GivenSingleSessionWithValues()
        {
            return new ApiSessionSaveCommand
            {
                businessId = BusinessId,
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

        private ApiSessionSaveCommand GivenSingleSessionMissingValues()
        {
            return new ApiSessionSaveCommand
            {
                businessId = BusinessId,
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

        private ApiSessionSaveCommand GivenClashingSingleSession()
        {
            return new ApiSessionSaveCommand
            {
                businessId = BusinessId,
                coach = new ApiCoachKey { id = AaronId },
                location = new ApiLocationKey { id = RemueraId },
                service = new ApiServiceKey { id = MiniBlueId },
                timing = new ApiSessionTiming
                {
                    startDate = GetDateFormatOneWeekOut(),
                    startTime = "14:30",
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
