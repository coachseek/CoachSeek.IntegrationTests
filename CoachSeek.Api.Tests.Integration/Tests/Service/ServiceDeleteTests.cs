using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    [TestFixture]
    public class ServiceDeleteTests : WebIntegrationTest
    {
        private const string MINI_RED_NAME = "Mini Red";
        private const string MINI_RED_DESCRIPTION = "Mini Red Service";
        private const string MINI_BLUE_NAME = "Mini Blue";

        private Guid MiniRedId { get; set; }
        private Guid MiniBlueId { get; set; }
        private string NewServiceName { get; set; }
        private string NewServiceDescription { get; set; }
        private int? Duration { get; set; }
        private string Colour { get; set; }


        protected override string RelativePath
        {
            get { return "Services"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }

        private void RegisterMiniRedService()
        {
            var json = CreateNewServiceSaveCommand(MINI_RED_NAME, MINI_RED_DESCRIPTION, "red");
            var response = Post<ServiceData>(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewServiceSaveCommand(MINI_BLUE_NAME, "Mini Blue Description", "blue");
            var response = Post<ServiceData>(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private string CreateNewServiceSaveCommand(string name, string description, string colour)
        {
            var service = new ApiServiceSaveCommand
            {
                name = name,
                description = description,
                timing = new ApiServiceTiming { duration = 45 },
                booking = new ApiServiceBooking { studentCapacity = 7, isOnlineBookable = true },
                repetition = new ApiServiceRepetition { sessionCount = 1 },
                presentation = new ApiPresentation { colour = colour },
                pricing = new ApiPricing { sessionPrice = 17.5m }
            };

            return JsonConvert.SerializeObject(service);
        }


        [Test]
        public void GivenNonExistentServiceId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var id = GivenNonExistentServiceId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }


        private Guid GivenNonExistentServiceId()
        {
            return Guid.NewGuid();
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<ServiceData>("Services", id);
        }
    }
}
