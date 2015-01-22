using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    [TestFixture]
    public class ServiceGetTests : WebIntegrationTest
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
        public void WhenGetAll_ThenReturnAllServicesResponse()
        {
            var response = WhenGetAll();
            ThenReturnAllServicesResponse(response);
        }

        [Test]
        public void GivenInvalidServiceId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var id = GivenInvalidServiceId();
            var response = WhenGetById(id);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidServiceId_WhenGetById_ThenReturnServiceResponse()
        {
            var id = GivenValidServiceId();
            var response = WhenGetById(id);
            ThenReturnServiceResponse(response);
        }



        private Guid GivenInvalidServiceId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidServiceId()
        {
            return MiniRedId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<ServiceData>>(url);
        }

        private Response WhenGetById(Guid serviceId)
        {
            var url = BuildGetByIdUrl(serviceId);
            return Get<ServiceData>(url);
        }


        private void ThenReturnAllServicesResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var services = (List<ServiceData>)response.Payload;
            Assert.That(services.Count, Is.EqualTo(2));
            var serviceOne = services[0];
            Assert.That(serviceOne.id, Is.EqualTo(MiniBlueId));
            Assert.That(serviceOne.name, Is.EqualTo(MINI_BLUE_NAME));
            var serviceTwo = services[1];
            Assert.That(serviceTwo.id, Is.EqualTo(MiniRedId));
            Assert.That(serviceTwo.name, Is.EqualTo(MINI_RED_NAME));
        }

        private void ThenReturnNotFoundResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private void ThenReturnServiceResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var service = (ServiceData)response.Payload;
            Assert.That(service.id, Is.EqualTo(MiniRedId));
            Assert.That(service.name, Is.EqualTo(MINI_RED_NAME));
            Assert.That(service.description, Is.EqualTo(MINI_RED_DESCRIPTION));
            Assert.That(service.timing.duration, Is.EqualTo(45));
            Assert.That(service.booking.studentCapacity, Is.EqualTo(7));
            Assert.That(service.booking.isOnlineBookable, Is.EqualTo(true));
            Assert.That(service.presentation.colour, Is.EqualTo("red"));
            Assert.That(service.repetition.sessionCount, Is.EqualTo(1));
            Assert.That(service.repetition.repeatFrequency, Is.Null);
            Assert.That(service.pricing.sessionPrice, Is.EqualTo(17.5));
            Assert.That(service.pricing.coursePrice, Is.Null);
        }
    }
}
