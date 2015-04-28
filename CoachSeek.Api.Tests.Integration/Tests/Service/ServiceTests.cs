using System;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    public abstract class ServiceTests : WebIntegrationTest
    {
        protected const string MINI_RED_NAME = "Mini Red";
        protected const string MINI_RED_DESCRIPTION = "Mini Red Service";
        protected const string MINI_BLUE_NAME = "Mini Blue";

        protected Guid MiniRedId { get; set; }
        protected Guid MiniBlueId { get; set; }

        protected override string RelativePath { get { return "Services"; } }


        protected ServiceMiniRed MiniRed { get; set; }
        protected ServiceMiniBlue MiniBlue { get; set; }
        protected ServiceMiniGreen MiniGreen { get; set; }




        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();

            SetupAddtitional();
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }

        protected virtual void SetupAddtitional()
        { }

        //public static void RegisterTestService(ExpectedService service)
        //{
        //    var json = CreateNewServiceSaveCommand(service);
        //    var response = PostService(json);
        //    service.Id = ((ServiceData)response.Payload).id;
        //}

        //public static string CreateNewServiceSaveCommand(ExpectedService expectedService)
        //{
        //    var service = new ApiServiceSaveCommand
        //    {
        //        name = expectedService.Name,
        //        description = expectedService.Description,
        //        repetition = expectedService.Repetition,
        //        presentation = expectedService.Presentation
        //    };

        //    if (expectedService.Timing != null)
        //        service.timing = expectedService.Timing;
        //    if (expectedService.Pricing != null)
        //        service.pricing = expectedService.Pricing;
        //    if (expectedService.Booking != null)
        //        service.booking = expectedService.Booking;

        //    return JsonConvert.SerializeObject(service);
        //}

        //public static Response PostService(string json)
        //{
        //    return Post<ServiceData>(json, "Services");
        //}



        private void RegisterMiniRedService()
        {
            var json = CreateNewFullServiceSaveCommand(MINI_RED_NAME, MINI_RED_DESCRIPTION, "red");
            var response = Post<ServiceData>(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewPartialServiceSaveCommand(MINI_BLUE_NAME, "blue");
            var response = Post<ServiceData>(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private string CreateNewFullServiceSaveCommand(string name, string description, string colour)
        {
            var service = new ApiServiceSaveCommand
            {
                name = name,
                description = description,
                timing = new ApiServiceTiming { duration = 45 },
                booking = new ApiServiceBooking { studentCapacity = 8, isOnlineBookable = false },
                presentation = new ApiPresentation { colour = colour },
                repetition = new ApiServiceRepetition { sessionCount = 10, repeatFrequency = "w" },
                pricing = new ApiPricing { sessionPrice = 12.5m, coursePrice = 100 }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string CreateNewPartialServiceSaveCommand(string name, string colour)
        {
            var service = new ApiServiceSaveCommand
            {
                name = name,
                repetition = new ApiServiceRepetition { sessionCount = 1 },
                presentation = new ApiPresentation { colour = colour }
            };

            return JsonConvert.SerializeObject(service);
        }
    }
}
