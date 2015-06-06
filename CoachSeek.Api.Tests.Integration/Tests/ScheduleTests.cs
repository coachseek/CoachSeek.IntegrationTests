﻿using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ScheduleTests : WebIntegrationTest
    {
        protected Guid AaronRemuera9To10For4WeeksCourseId { get; set; }
        protected Guid[] AaronRemuera9To10For4WeeksSessionIds { get; set; }
        protected Guid BobbyRemueraHolidayCampFor3DaysCourseId { get; set; }
        protected Guid[] BobbyRemueraHolidayCampFor3DaysSessionIds { get; set; }
        protected Guid AaronOrakeiMiniBlueFor2DaysCourseId { get; set; }
        protected Guid[] AaronOrakeiMiniBlueFor2DaysSessionIds { get; set; }

        protected Guid FredOnAaronOrakei14To15SessionId { get; set; }
        protected Guid BarneyOnAaronOrakei14To15SessionId { get; set; }
        protected Guid FredOnBobbyRemueraHolidayCampFor3DaysCourseId { get; set; }
        protected Guid FredOnAaronOrakeiMiniBlueFor2DaysCourseId { get; set; }
        protected Guid BarneyOnAaronOrakeiMiniBlueFor2DaysOnTheSecondDayId { get; set; }


        protected LocationOrakei Orakei { get; set; }
        protected LocationRemuera Remuera { get; set; }

        protected ServiceMiniRed MiniRed { get; set; }
        protected ServiceMiniBlue MiniBlue { get; set; }
        protected ServiceMiniGreen MiniGreen { get; set; }
        protected ServiceHolidayCamp HolidayCamp { get; set; }

        protected StandaloneAaronOrakei14To15 AaronOrakei14To15 { get; set; }
        protected StandaloneAaronOrakei16To17 AaronOrakei16To17 { get; set; }
        protected StandaloneBobbyRemuera12To13 BobbyRemuera12To13 { get; set; }

        protected CustomerFred Fred { get; set; }
        protected CustomerBarney Barney { get; set; }
        protected CustomerWilma Wilma { get; set; }
        protected CustomerBamBam BamBam { get; set; }


        //protected SessionBookingFredOnBobbyRemuera12To13 FredOnBobbyRemuera12To13 { get; set; }


        protected override string RelativePath
        {
            get { return "Sessions"; }
        }

        protected string AaronOrakei14To15SessionDescription
        {
            get
            {
                return string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 14:00",
                                     GetFormattedDateOneWeekOut());
            }
        }

        protected string AaronOrakei16To17SessionDescription
        {
            get
            {
                return string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 16:00",
                                     GetFormattedDateOneWeekOut());
            }
        }


        protected void FullySetupNewTestBusiness()
        {
            RegisterTestBusiness();
            FullySetupExistingTestBusiness();
        }

        protected void FullySetupExistingTestBusiness()
        {
            RegisterTestLocations();
            RegisterTestCoaches();
            RegisterTestServices();
            RegisterTestSessions();
            RegisterTestCourses();
            RegisterTestCustomers();
            BookCustomersOntoStandaloneSessions();
            BookCustomersOntoCourses();
            BookCustomersOntoCourseSessions();
        }


        private void RegisterTestLocations()
        {
            Orakei = new LocationOrakei();
            LocationRegistrar.RegisterLocation(Orakei, Business);

            Remuera = new LocationRemuera();
            LocationRegistrar.RegisterLocation(Remuera, Business);
        }

        private void RegisterTestCoaches()
        {
            Aaron = new CoachAaron();
            CoachRegistrar.RegisterCoach(Aaron, Business);

            Bobby = new CoachBobby();
            CoachRegistrar.RegisterCoach(Bobby, Business);
        }

        private void RegisterTestServices()
        {
            MiniRed = new ServiceMiniRed();
            ServiceRegistrar.RegisterService(MiniRed, Business);

            MiniBlue = new ServiceMiniBlue();
            ServiceRegistrar.RegisterService(MiniBlue, Business);

            HolidayCamp = new ServiceHolidayCamp();
            ServiceRegistrar.RegisterService(HolidayCamp, Business);
        }

        private void RegisterTestSessions()
        {
            AaronOrakei14To15 = new StandaloneAaronOrakei14To15(Aaron.Id, Orakei.Id, MiniRed.Id, GetDateFormatNumberOfDaysOut(21));
            RegisterTestSession(AaronOrakei14To15);

            AaronOrakei16To17 = new StandaloneAaronOrakei16To17(Aaron.Id, Orakei.Id, MiniRed.Id, GetDateFormatNumberOfDaysOut(7));
            RegisterTestSession(AaronOrakei16To17);
        }


        protected void RegisterTestSession(ExpectedStandaloneSession session)
        {
            var json = CreateSessionSaveCommandJson(session);
            var response = PostSession(json);
            if (response.Payload != null)
                session.Id = ((SessionData)response.Payload).id;
        }

        private void RegisterTestCourses()
        {
            RegisterAaronRemuera9To10For4Weeks();
            RegisterBobbyRemueraHolidayCampFor3Days();
            RegisterAaronOrakeiMiniBlueFor2Days();
        }

        private void RegisterAaronRemuera9To10For4Weeks()
        {
            var json = CreateSessionSaveCommandAaronRemuera9To10For4WeeksJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                AaronRemuera9To10For4WeeksCourseId = course.id;
                AaronRemuera9To10For4WeeksSessionIds = new Guid[5];
                for (var i = 0; i < 4; i++)
                    AaronRemuera9To10For4WeeksSessionIds[i] = course.sessions[i].id;
            }
        }

        private void RegisterBobbyRemueraHolidayCampFor3Days()
        {
            var json = CreateSessionSaveCommandBobbyRemueraHolidayCampFor3DaysJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                BobbyRemueraHolidayCampFor3DaysCourseId = course.id;
                BobbyRemueraHolidayCampFor3DaysSessionIds = new Guid[3];
                for (var i = 0; i < 3; i++)
                    BobbyRemueraHolidayCampFor3DaysSessionIds[i] = course.sessions[i].id;
            }
        }

        private void RegisterAaronOrakeiMiniBlueFor2Days()
        {
            var json = CreateSessionSaveCommandAaronOrakeiMiniBlueFor2DaysJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                AaronOrakeiMiniBlueFor2DaysCourseId = course.id;
                AaronOrakeiMiniBlueFor2DaysSessionIds = new Guid[2];
                for (var i = 0; i < 2; i++)
                    AaronOrakeiMiniBlueFor2DaysSessionIds[i] = course.sessions[i].id;
            }
        }


        protected ApiResponse WhenPostSession(string json)
        {
            return new TestAuthenticatedApiClient().Post<SessionData>(json, Business.UserName, Business.Password, "Sessions");
        }

        protected ApiResponse WhenPostCourse(string json)
        {
            return new TestAuthenticatedApiClient().Post<CourseData>(json, Business.UserName, Business.Password, "Sessions");
        }


        protected ApiResponse WhenTryCreateSession(ApiSessionSaveCommand command)
        {
            return WhenPostSession(JsonConvert.SerializeObject(command));
        }

        protected ApiResponse WhenTryUpdateSession(ApiSessionSaveCommand command)
        {
            return WhenPostSession(JsonConvert.SerializeObject(command));
        }

        protected ApiResponse WhenTryCreateCourse(ApiSessionSaveCommand command)
        {
            return WhenPostCourse(JsonConvert.SerializeObject(command));
        }

        protected ApiResponse WhenTryUpdateCourse(ApiSessionSaveCommand command)
        {
            return WhenPostCourse(JsonConvert.SerializeObject(command));
        }


        private Response PostSession(string json)
        {
            return Post<SessionData>(json, "Sessions");
        }
    
        private Response PostCourse(string json)
        {
            return Post<CourseData>(json, "Sessions");
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronOrakei14To15()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniRed.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(3), startTime = "14:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }
        protected ApiSessionSaveCommand CreateSessionSaveCommand(ExpectedStandaloneSession session)
        {
            return new ApiSessionSaveCommand
            {
                coach = session.Coach,
                location = session.Location,
                service = session.Service,
                timing = session.Timing,
                booking = session.Booking,
                repetition = session.Repetition,
                pricing = session.Pricing,
                presentation = session.Presentation
            };
        }

        private string CreateSessionSaveCommandJson(ExpectedStandaloneSession session)
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommand(session));
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronOrakei16To17()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniRed.Id },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "16:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronRemuera9To10()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Remuera.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniRed.Id },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "9:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 6, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronRemuera9To10For4Weeks()
        {
            var command = CreateSessionSaveCommandAaronRemuera9To10();
            command.repetition = new ApiRepetition {sessionCount = 4, repeatFrequency = "w"};

            return command;
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Remuera.Id },
                coach = new ApiCoachKey { id = Bobby.Id },
                service = new ApiServiceKey { id = HolidayCamp.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(2), startTime = "10:00", duration = 240 },
                booking = new ApiSessionBooking { studentCapacity = 20, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 3, repeatFrequency = "d"},
                pricing = new ApiPricing { sessionPrice = 120, coursePrice = 200},
                presentation = new ApiPresentation { colour = "yellow" }
            };
        }

        protected ApiSessionSaveCommand CreateSessionSaveComandAaronOrakeiMiniBlueFor2Days()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniBlue.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(1), startTime = "9:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 2, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 2, repeatFrequency = "d" },
                pricing = new ApiPricing { sessionPrice = 25, coursePrice = 40 },
                presentation = new ApiPresentation { colour = "blue" }
            };
        }

        private string CreateSessionSaveCommandAaronRemuera9To10For4WeeksJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandAaronRemuera9To10For4Weeks());
        }

        private string CreateSessionSaveCommandBobbyRemueraHolidayCampFor3DaysJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days());
        }

        private string CreateSessionSaveCommandAaronOrakeiMiniBlueFor2DaysJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveComandAaronOrakeiMiniBlueFor2Days());
        }

        private void RegisterTestCustomers()
        {
            Fred = new CustomerFred();
            CustomerRegistrar.RegisterCustomer(Fred, Business);

            Barney = new CustomerBarney();
            CustomerRegistrar.RegisterCustomer(Barney, Business);

            Wilma = new CustomerWilma();
            CustomerRegistrar.RegisterCustomer(Wilma, Business);
        }


        private void BookCustomersOntoStandaloneSessions()
        {
            BookFredFlintstoneOntoAaronOrakei14To15();
            BookBarneyRubbleOntoAaronOrakei14To15();
        }

        private void BookFredFlintstoneOntoAaronOrakei14To15()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakei14To15.Id, Fred.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                FredOnAaronOrakei14To15SessionId = ((BookingData)response.Payload).id;
        }

        private void BookBarneyRubbleOntoAaronOrakei14To15()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakei14To15.Id, Barney.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                BarneyOnAaronOrakei14To15SessionId = ((BookingData)response.Payload).id;
        }

        private void BookCustomersOntoCourses()
        {
            BookFredFlintstoneOntoAaronOrakeiMiniBlueFor2Days();
        }

        private void BookFredFlintstoneOntoAaronOrakeiMiniBlueFor2Days()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniBlueFor2DaysCourseId, Fred.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                FredOnAaronOrakeiMiniBlueFor2DaysCourseId = ((BookingData)response.Payload).id;
        }

        private void BookCustomersOntoCourseSessions()
        {
            BookBarneyRubbleOntoAaronOrakeiMiniBlueFor2DaysOnTheSecondDay();
        }

        private void BookBarneyRubbleOntoAaronOrakeiMiniBlueFor2DaysOnTheSecondDay()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniBlueFor2DaysSessionIds[1], Barney.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                BarneyOnAaronOrakeiMiniBlueFor2DaysOnTheSecondDayId = ((BookingData)response.Payload).id;
        }

        private string CreateNewBookingSaveCommand(Guid sessionId, Guid customerId)
        {
            var booking = new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = sessionId },
                customer = new ApiCustomerKey { id = customerId }
            };

            return JsonConvert.SerializeObject(booking);
        }

        private Response PostBooking(string json)
        {
            return Post<BookingData>(json, "Bookings");
        }


        protected void AssertSessionLocation(LocationKeyData location, Guid locationId, string locationName)
        {
            Assert.That(location, Is.Not.Null);
            Assert.That(location.id, Is.EqualTo(locationId));
            Assert.That(location.name, Is.EqualTo(locationName));
        }

        protected void AssertSessionCoach(CoachKeyData coach, Guid coachId, string coachName)
        {
            Assert.That(coach, Is.Not.Null);
            Assert.That(coach.id, Is.EqualTo(coachId));
            Assert.That(coach.name, Is.EqualTo(coachName));
        }

        protected void AssertSessionService(ServiceKeyData service, Guid serviceId, string serviceName)
        {
            Assert.That(service, Is.Not.Null);
            Assert.That(service.id, Is.EqualTo(serviceId));
            Assert.That(service.name, Is.EqualTo(serviceName));
        }

        protected void AssertSessionTiming(SessionTimingData timing, string startDate, string startTime, int duration)
        {
            Assert.That(timing, Is.Not.Null);
            Assert.That(timing.startDate, Is.EqualTo(startDate));
            Assert.That(timing.startTime, Is.EqualTo(startTime));
            Assert.That(timing.duration, Is.EqualTo(duration));
        }

        protected void AssertSessionBooking(SessionBookingData booking, int? studentCapacity, bool isOnlineBookable, int bookingCount = 0)
        {
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.studentCapacity, Is.EqualTo(studentCapacity));
            Assert.That(booking.isOnlineBookable, Is.EqualTo(isOnlineBookable));
            Assert.That(booking.bookings, Is.Not.Null);
            Assert.That(booking.bookings.Count, Is.EqualTo(bookingCount));
        }

        protected void AssertSessionPricing(PricingData pricing, decimal? sessionPrice, decimal? coursePrice)
        {
            Assert.That(pricing, Is.Not.Null);
            Assert.That(pricing.sessionPrice, Is.EqualTo(sessionPrice));
            Assert.That(pricing.coursePrice, Is.EqualTo(coursePrice));
        }

        protected void AssertSessionRepetition(RepetitionData repetition, int sessionCount, string repeatFrequency)
        {
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.sessionCount, Is.EqualTo(sessionCount));
            Assert.That(repetition.repeatFrequency, Is.EqualTo(repeatFrequency));
        }

        protected void AssertSessionPresentation(PresentationData presentation, string colour)
        {
            Assert.That(presentation, Is.Not.Null);
            Assert.That(presentation.colour, Is.EqualTo(colour));
        }


        protected string GetFormattedDateToday()
        {
            return GetDateFormatNumberOfWeeksOut(0);
        }

        protected string GetFormattedDateOneWeekOut()
        {
            return GetDateFormatNumberOfWeeksOut(1);
        }

        protected string GetFormattedDateTwoWeeksOut()
        {
            return GetDateFormatNumberOfWeeksOut(2);
        }

        protected string GetFormattedDateThreeWeeksOut()
        {
            return GetDateFormatNumberOfWeeksOut(3);
        }

        protected string GetDateFormatNumberOfWeeksOut(int numberOfWeeks)
        {
            var today = DateTime.Today;
            var weeksFromToday = today.AddDays(7 * numberOfWeeks);

            return weeksFromToday.ToString("yyyy-MM-dd");
        }

        protected string GetDateFormatNumberOfDaysOut(int numberOfDays)
        {
            var today = DateTime.Today;
            var daysFromToday = today.AddDays(numberOfDays);

            return daysFromToday.ToString("yyyy-MM-dd");
        }
    }
}
