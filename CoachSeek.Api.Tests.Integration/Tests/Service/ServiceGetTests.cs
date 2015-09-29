using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
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


            private string GivenValidBusinessDomain(SetupData setup)
            {
                return setup.Business.Domain;
            }


            private ApiResponse WhenTryGetServiceByIdAnonymously(Guid serviceId, string businessDomain)
            {
                var url = string.Format("{0}/{1}", RelativePath, serviceId);
                return BusinessAnonymousGet<ServiceData>(url, businessDomain);
            }

            private ApiResponse WhenTryGetAllServicesAnonymously(string businessDomain)
            {
                return BusinessAnonymousGet<List<ServiceData>>(RelativePath, businessDomain);
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


        private ApiResponse WhenTryGetAllServices(SetupData setup)
        {
            return AuthenticatedGet<List<ServiceData>>(RelativePath, setup);
        }

        private ApiResponse WhenTryGetServiceById(Guid serviceId, SetupData setup)
        {
            return AuthenticatedGet<ServiceData>(RelativePath, serviceId, setup);
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
