﻿using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ScheduleTests : WebIntegrationTest
    {
        private const string ORAKEI_NAME = "Orakei Tennis Club";
        private const string REMUERA_NAME = "Remuera Racquets Club";
        private const string AARON_FIRST_NAME = "Aaron";
        private const string BOBBY_FIRST_NAME = "Bobby";
        private const string SMITH_LAST_NAME = "Smith";
        private const string MINI_RED_NAME = "Mini Red";
        private const string MINI_BLUE_NAME = "Mini Blue";

        protected Guid OrakeiId { get; set; }
        protected Guid RemueraId { get; set; }
        protected Guid AaronId { get; set; }
        protected Guid BobbyId { get; set; }
        protected Guid MiniRedId { get; set; }
        protected Guid MiniBlueId { get; set; }
        protected Guid AaronOrakei2To3SessionId { get; set; }
        protected Guid AaronOrakei4To5SessionId { get; set; }

        protected void SetupFullTestBusiness()
        {
            RegisterTestBusiness();
            RegisterTestLocations();
            RegisterTestCoaches();
            RegisterTestServices();
            RegisterTestSessions();
            RegisterTestCourses();
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

        private void RegisterTestSessions()
        {
            RegisterAaronOrakei2To3();
            RegisterAaronOrakei4To5();
        }

        private void RegisterAaronOrakei2To3()
        {
            var json = CreateSessionSaveCommandAaronOrakei2To3(GetDateFormatNumberOfWeeksOut(3));
            var response = PostSession(json);
            AaronOrakei2To3SessionId = ((SessionData)response.Payload).id;
        }

        private void RegisterAaronOrakei4To5()
        {
            var json = CreateSessionSaveCommandAaronOrakei4To5();
            var response = PostSession(json);
            AaronOrakei4To5SessionId = ((SessionData)response.Payload).id;
        }

        private void RegisterTestCourses()
        {
            RegisterAaronRemuera9To10For8Weeks();
        }

        private void RegisterAaronRemuera9To10For8Weeks()
        {
            var json = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();
            var response = PostSession(json);
            AaronOrakei4To5SessionId = ((SessionData)response.Payload).id;
        }

        private Response PostSession(string json)
        {
            return Post<SessionData>(json, "Sessions");
        }

        private string CreateSessionSaveCommandAaronOrakei2To3(string startDate)
        {
            var service = new ApiSessionSaveCommand
            {
                businessId = BusinessId,
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = startDate, startTime = "14:00", duration = 60 }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateSessionSaveCommandAaronOrakei4To5()
        {
            var service = new ApiSessionSaveCommand
            {
                businessId = BusinessId,
                location = new ApiLocationKey { id = OrakeiId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetDateFormatOneWeekOut(), startTime = "16:00", duration = 60 }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateSessionSaveCommandAaronRemuera9To10For8Weeks()
        {
            var service = new ApiSessionSaveCommand
            {
                businessId = BusinessId,
                location = new ApiLocationKey { id = RemueraId },
                coach = new ApiCoachKey { id = AaronId },
                service = new ApiServiceKey { id = MiniRedId },
                timing = new ApiSessionTiming { startDate = GetDateFormatOneWeekOut(), startTime = "9:00", duration = 60 },
                repetition = new ApiRepetition { sessionCount = 8, repeatFrequency = "w" }
            };

            return JsonConvert.SerializeObject(service);
        }

        protected string GetDateFormatOneWeekOut()
        {
            return GetDateFormatNumberOfWeeksOut(1);
        }

        protected string GetDateFormatTwoWeeksOut()
        {
            return GetDateFormatNumberOfWeeksOut(2);
        }

        protected string GetDateFormatNumberOfWeeksOut(int numberOfWeeks)
        {
            var today = DateTime.Today;
            var twoWeeksFromToday = today.AddDays(7 * numberOfWeeks);

            return twoWeeksFromToday.ToString("yyyy-MM-dd");
        }
    }
}
