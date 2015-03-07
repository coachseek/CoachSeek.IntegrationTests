using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ScheduleTests : WebIntegrationTest
    {
        protected const string ORAKEI_NAME = "Orakei Tennis Club";
        protected const string REMUERA_NAME = "Remuera Racquets Club";
        protected const string AARON_FIRST_NAME = "Aaron";
        protected const string BOBBY_FIRST_NAME = "Bobby";
        protected const string SMITH_LAST_NAME = "Smith";
        protected const string MINI_RED_NAME = "Mini Red";
        protected const string MINI_BLUE_NAME = "Mini Blue";
        protected const string MINI_GREEN_NAME = "Mini Green";
        protected const string MINI_ORANGE_NAME = "Mini Orange";
        protected const string FRED_FIRST_NAME = "Fred";
        protected const string FLINTSTONE_LAST_NAME = "Flintstone";

        protected Guid OrakeiId { get; set; }
        protected Guid RemueraId { get; set; }
        protected Guid AaronId { get; set; }
        protected Guid BobbyId { get; set; }
        protected Guid MiniRedId { get; set; }
        protected Guid MiniBlueId { get; set; }
        protected Guid MiniGreenId { get; set; }
        protected Guid MiniOrangeId { get; set; }
        protected Guid AaronOrakei14To15SessionId { get; set; }
        protected Guid AaronOrakei16To17SessionId { get; set; }
        protected Guid AaronRemuera9To10For8WeeksCourseId { get; set; }
        protected Guid[] AaronRemuera9To10For8WeeksSessionIds { get; set; }
        protected Guid FredId { get; set; }

        protected void SetupFullTestBusiness()
        {
            RegisterTestBusiness();
            RegisterTestLocations();
            RegisterTestCoaches();
            RegisterTestServices();
            RegisterTestSessions();
            RegisterTestCourses();
            RegisterTestCustomers();
        }

        private void RegisterTestLocations()
        {
            RegisterOrakeiLocation();
            RegisterRemueraLocation();
        }

        private void RegisterOrakeiLocation()
        {
            var json = CreateNewLocationSaveCommand(ORAKEI_NAME);
            var response = PostLocation(json);
            OrakeiId = ((LocationData)response.Payload).id;
        }

        private void RegisterRemueraLocation()
        {
            var json = CreateNewLocationSaveCommand(REMUERA_NAME);
            var response = PostLocation(json);
            RemueraId = ((LocationData)response.Payload).id;
        }

        private Response PostLocation(string json)
        {
            return Post<LocationData>(json, "Locations");
        }

        private string CreateNewLocationSaveCommand(string name)
        {
            var location = new ApiLocationSaveCommand
            {
                name = name
            };

            return JsonConvert.SerializeObject(location);
        }

        private void RegisterTestCoaches()
        {
            RegisterAaronCoach();
            RegisterBobbyLocation();
        }

        private void RegisterAaronCoach()
        {
            var json = CreateNewCoachSaveCommand(AARON_FIRST_NAME, SMITH_LAST_NAME);
            var response = PostCoach(json);
            AaronId = ((CoachData)response.Payload).id;
        }

        private void RegisterBobbyLocation()
        {
            var json = CreateNewCoachSaveCommand(BOBBY_FIRST_NAME, SMITH_LAST_NAME);
            var response = PostCoach(json);
            BobbyId = ((CoachData)response.Payload).id;
        }

        private Response PostCoach(string json)
        {
            return Post<CoachData>(json, "Coaches");
        }

        private string CreateNewCoachSaveCommand(string firstName, string lastName)
        {
            var coach = new ApiCoachSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = RandomEmail,
                phone = RandomString,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }

        private ApiWeeklyWorkingHours SetupStandardWorkingHours()
        {
            return new ApiWeeklyWorkingHours
            {
                monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                tuesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                wednesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                thursday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                friday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                saturday = new ApiDailyWorkingHours(),
                sunday = new ApiDailyWorkingHours()
            };
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
            RegisterMiniGreenService();
            RegisterMiniOrangeService();
        }

        private void RegisterMiniRedService()
        {
            var json = CreateNewServiceSaveCommandWithDefaults(MINI_RED_NAME, "RED");
            var response = PostService(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewServiceSaveCommandWithoutDefaults(MINI_BLUE_NAME, "blue");
            var response = PostService(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniGreenService()
        {
            var json = CreateMiniGreenServiceSaveCommand();
            var response = PostService(json);
            MiniGreenId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniOrangeService()
        {
            var json = CreateMiniOrangeServiceSaveCommand();
            var response = PostService(json);
            MiniOrangeId = ((ServiceData)response.Payload).id;
        }

        private Response PostService(string json)
        {
            return Post<ServiceData>(json, "Services");
        }

        private string CreateNewServiceSaveCommandWithoutDefaults(string name, string colour)
        {
            var service = new ApiServiceSaveCommand
            {
                name = name,
                description = string.Format("{0} Service", name),
                repetition = new ApiServiceRepetition { sessionCount = 1 },
                presentation = new ApiPresentation { colour = colour }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateNewServiceSaveCommandWithDefaults(string name, string colour)
        {
            var service = new ApiServiceSaveCommand
            {
                name = name,
                description = string.Format("{0} Service", name),
                timing = new ApiServiceTiming { duration = 75 },
                booking = new ApiServiceBooking
                {
                    studentCapacity = 13,
                    isOnlineBookable = true
                },
                repetition = new ApiServiceRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = colour }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateMiniGreenServiceSaveCommand()
        {
            var service = new ApiServiceSaveCommand
            {
                name = MINI_GREEN_NAME,
                timing = new ApiServiceTiming { duration = 60 },
                booking = new ApiServiceBooking { studentCapacity = 51 },
                pricing = new ApiPricing { sessionPrice = 35, coursePrice = 60 },
                repetition = new ApiServiceRepetition { sessionCount = 2, repeatFrequency = "d" },
                presentation = new ApiPresentation { colour = "green" }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateMiniOrangeServiceSaveCommand()
        {
            var service = new ApiServiceSaveCommand
            {
                name = MINI_ORANGE_NAME,
                timing = new ApiServiceTiming { duration = 75 },
                booking = new ApiServiceBooking { studentCapacity = 6 },
                pricing = new ApiPricing { coursePrice = 125 },
                repetition = new ApiServiceRepetition { sessionCount = 5, repeatFrequency = "w" },
                presentation = new ApiPresentation { colour = "orange" }
            };

            return JsonConvert.SerializeObject(service);
        }

        private void RegisterTestSessions()
        {
            RegisterAaronOrakei14To15();
            RegisterAaronOrakei16To17();
        }

        private void RegisterAaronOrakei14To15()
        {
            var json = CreateSessionSaveCommandAaronOrakei14To15Json();
            var response = PostSession(json);
            AaronOrakei14To15SessionId = ((SessionData)response.Payload).id;
        }

        private void RegisterAaronOrakei16To17()
        {
            var json = CreateSessionSaveCommandAaronOrakei16To17Json();
            var response = PostSession(json);
            AaronOrakei16To17SessionId = ((SessionData)response.Payload).id;
        }

        private void RegisterTestCourses()
        {
            RegisterAaronRemuera9To10For8Weeks();
        }

        private void RegisterAaronRemuera9To10For8Weeks()
        {
            var json = CreateSessionSaveCommandAaronRemuera9To10For8WeeksJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            AaronRemuera9To10For8WeeksCourseId = course.id;
            AaronRemuera9To10For8WeeksSessionIds = new Guid[8];
            for(var i = 0; i < 8; i++)
                AaronRemuera9To10For8WeeksSessionIds[i] = course.sessions[i].id;
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
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(3), startTime = "14:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        private string CreateSessionSaveCommandAaronOrakei14To15Json()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandAaronOrakei14To15());
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronOrakei16To17()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "16:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        private string CreateSessionSaveCommandAaronOrakei16To17Json()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandAaronOrakei16To17());
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronRemuera9To10For8Weeks()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = RemueraId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "9:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 8, repeatFrequency = "w" },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        private string CreateSessionSaveCommandAaronRemuera9To10For8WeeksJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandAaronRemuera9To10For8Weeks());
        }

        private void RegisterTestCustomers()
        {
            RegisterFredFlintstoneCustomer();
        }

        private void RegisterFredFlintstoneCustomer()
        {
            var json = CreateNewCustomerSaveCommand(FRED_FIRST_NAME, FLINTSTONE_LAST_NAME);
            var response = PostCustomer(json);
            FredId = ((CustomerData)response.Payload).id;
        }

        private Response PostCustomer(string json)
        {
            return Post<CustomerData>(json, "Customers");
        }

        private string CreateNewCustomerSaveCommand(string firstName, string lastName)
        {
            var customer = new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = RandomEmail,
                phone = RandomString
            };

            return JsonConvert.SerializeObject(customer);
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

        protected void AssertSessionBooking(SessionBookingData booking, int? studentCapacity, bool isOnlineBookable)
        {
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.studentCapacity, Is.EqualTo(studentCapacity));
            Assert.That(booking.isOnlineBookable, Is.EqualTo(isOnlineBookable));
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
    }
}
