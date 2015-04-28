using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    public class ServiceGetTests : ServiceTests
    {
        [TestFixture]
        public class AnonymousServiceGetTests : ServiceGetTests
        {
            [Test]
            public void GivenNoBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetByIdAnonymously(MiniRedId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetByIdAnonymously(MiniRedId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnService()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetByIdAnonymously(MiniRedId);
                ThenReturnService(response);
            }

            [Test]
            public void GivenNoBusinessDomain_WhenTryGetAllAnonymously_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetAllAnonymously_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetAllAnonymously_ThenReturnAllServices()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnAllServices(response);
            }


            private void GivenNoBusinessDomain()
            {
                Business.Domain = null;
            }

            private void GivenInvalidBusinessDomain()
            {
                Business.Domain = "abc123";
            }

            private void GivenValidBusinessDomain()
            {
                // Valid domain is already set.
            }


            private Response WhenTryGetByIdAnonymously(Guid serviceId)
            {
                var url = BuildGetByIdUrl(serviceId);
                return GetAnonymously<ServiceData>(url);
            }

            private Response WhenTryGetAllAnonymously()
            {
                var url = BuildGetAllUrl();
                return GetAnonymously<List<ServiceData>>(url);
            }


            private void ThenReturnNotAuthorised(Response response)
            {
                AssertUnauthorised(response);
            }
        }


        [TestFixture]
        public class AuthenticatedServiceGetTests : ServiceGetTests
        {
            [Test]
            public void WhenTryGetAll_ThenReturnAllServices()
            {
                var response = WhenTryGetAll();
                ThenReturnAllServices(response);
            }

            [Test]
            public void GivenInvalidServiceId_WhenTryGetById_ThenReturnNotFound()
            {
                var serviceId = GivenInvalidServiceId();
                var response = WhenTryGetById(serviceId);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidServiceId_WhenTryGetById_ThenReturnService()
            {
                var serviceId = GivenValidServiceId();
                var response = WhenTryGetById(serviceId);
                ThenReturnService(response);
            }
        }


        private Guid GivenInvalidServiceId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidServiceId()
        {
            return MiniRedId;
        }


        private Response WhenTryGetAll()
        {
            var url = BuildGetAllUrl();
            return AuthenticatedGet<List<ServiceData>>(url);
        }

        private Response WhenTryGetById(Guid serviceId)
        {
            var url = BuildGetByIdUrl(serviceId);
            return AuthenticatedGet<ServiceData>(url);
        }


        private void ThenReturnAllServices(Response response)
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

        private void ThenReturnNotFound(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnService(Response response)
        {
            var service = AssertSuccessResponse<ServiceData>(response);
            AssertMiniRedService(service);
        }

        private void AssertMiniRedService(ServiceData service)
        {
            Assert.That(service.id, Is.EqualTo(MiniRedId));
            Assert.That(service.name, Is.EqualTo(MINI_RED_NAME));
            Assert.That(service.description, Is.EqualTo(MINI_RED_DESCRIPTION));
            Assert.That(service.timing.duration, Is.EqualTo(45));
            Assert.That(service.booking.studentCapacity, Is.EqualTo(8));
            Assert.That(service.booking.isOnlineBookable, Is.EqualTo(false));
            Assert.That(service.presentation.colour, Is.EqualTo("red"));
            Assert.That(service.repetition.sessionCount, Is.EqualTo(10));
            Assert.That(service.repetition.repeatFrequency, Is.EqualTo("w"));
            Assert.That(service.pricing.sessionPrice, Is.EqualTo(12.5));
            Assert.That(service.pricing.coursePrice, Is.EqualTo(100));
        }
    }
}
