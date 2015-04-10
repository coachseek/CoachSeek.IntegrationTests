using System;
using CoachSeek.Api.Tests.Integration.Models;
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


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();

            SetupAddtitional();
        }

        protected virtual void SetupAddtitional()
        { }


        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }


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
