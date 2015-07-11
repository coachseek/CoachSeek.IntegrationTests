﻿using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
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
            public void GivenNoBusinessDomain_WhenTryGetServiceByIdAnonymously_ThenReturnUnauthorised()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var businessDomain = GivenNoBusinessDomain();
                var response = WhenTryGetServiceByIdAnonymously(setup.MiniRed.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetServiceByIdAnonymously_ThenReturnUnauthorised()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var businessDomain = GivenInvalidBusinessDomain();
                var response = WhenTryGetServiceByIdAnonymously(setup.MiniRed.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetServiceByIdAnonymously_ThenReturnService()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetServiceByIdAnonymously(setup.MiniRed.Id, businessDomain);
                ThenReturnService(response, setup);
            }

            [Test]
            public void GivenNoBusinessDomain_WhenTryGetAllServicesAnonymously_ThenReturnNotAuthorised()
            {
                var businessDomain = GivenNoBusinessDomain();
                var response = WhenTryGetAllServicesAnonymously(businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetAllServicesAnonymously_ThenReturnNotAuthorised()
            {
                var businessDomain = GivenInvalidBusinessDomain();
                var response = WhenTryGetAllServicesAnonymously(businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetAllServicesAnonymously_ThenReturnAllServices()
            {
                var setup = RegisterBusiness();
                RegisterTestServices(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetAllServicesAnonymously(businessDomain);
                ThenReturnAllServices(response, setup);
            }


            private string GivenNoBusinessDomain()
            {
                return null;
            }

            private string GivenInvalidBusinessDomain()
            {
                return "abc123";
            }

            private string GivenValidBusinessDomain(SetupData setup)
            {
                return setup.Business.Domain;
            }


            private ApiResponse WhenTryGetServiceByIdAnonymously(Guid serviceId, string businessDomain)
            {
                var url = string.Format("{0}/{1}", RelativePath, serviceId);
                return new TestBusinessAnonymousApiClient().Get<ServiceData>(businessDomain, url);                
            }

            private ApiResponse WhenTryGetAllServicesAnonymously(string businessDomain)
            {
                return new TestBusinessAnonymousApiClient().Get<List<ServiceData>>(businessDomain, RelativePath);
            }


            private void ThenReturnUnauthorised(ApiResponse response)
            {
                AssertUnauthorised(response);
            }
        }


        [TestFixture]
        public class AuthenticatedServiceGetTests : ServiceGetTests
        {
            [Test]
            public void WhenTryGetAllServices_ThenReturnAllServices()
            {
                var setup = RegisterBusiness();
                RegisterTestServices(setup);

                var response = WhenTryGetAllServices(setup);
                ThenReturnAllServices(response, setup);
            }

            [Test]
            public void GivenInvalidServiceId_WhenTryGetServiceById_ThenReturnNotFound()
            {
                var setup = RegisterBusiness();

                var serviceId = GivenInvalidServiceId();
                var response = WhenTryGetServiceById(serviceId, setup);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidServiceId_WhenTryGetServiceById_ThenReturnService()
            {
                var setup = RegisterBusiness();
                RegisterServiceMiniRed(setup);

                var serviceId = GivenValidServiceId(setup);
                var response = WhenTryGetServiceById(serviceId, setup);
                ThenReturnService(response, setup);
            }
        }


        private Guid GivenInvalidServiceId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidServiceId(SetupData setup)
        {
            return setup.MiniRed.Id;
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

        private ApiResponse WhenTryGetAllServices(SetupData setup)
        {
            return new TestAuthenticatedApiClient().Get<List<ServiceData>>(setup.Business.UserName,
                                                                           setup.Business.Password,
                                                                           RelativePath);
        }

        private ApiResponse WhenTryGetServiceById(Guid serviceId, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, serviceId);
            return new TestAuthenticatedApiClient().Get<ServiceData>(setup.Business.UserName,
                                                                     setup.Business.Password,
                                                                     url);
        }


        private void ThenReturnAllServices(ApiResponse response, SetupData setup)
        {
            var services = AssertSuccessResponse<List<ServiceData>>(response);

            setup.HolidayCamp.Assert(services[0]);
            setup.MiniRed.Assert(services[1]);
        }

        private void ThenReturnService(ApiResponse response, SetupData setup)
        {
            var service = AssertSuccessResponse<ServiceData>(response);
            setup.MiniRed.Assert(service);
        }
    }
}
