using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Service
{
    [TestFixture]
    public class ServiceDeleteTests : ServiceTests
    {
        [Test]
        public void GivenNonExistentServiceId_WhenTryDeleteService_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentServiceId();
            var response = WhenTryDeleteService(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTryDeleteServiceAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();
            RegisterServiceMiniRed(setup);

            var id = GivenValidServiceId(setup);
            var response = WhenTryDeleteServiceAnonymously(id);
            AssertUnauthorised(response);
        }


        private Guid GivenNonExistentServiceId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidServiceId(SetupData setup)
        {
            return setup.MiniRed.Id;
        }


        private ApiResponse WhenTryDeleteService(Guid id, SetupData setup)
        {
            return AuthenticatedDelete(RelativePath, id.ToString(), setup);
        }

        private ApiResponse WhenTryDeleteServiceAnonymously(Guid id)
        {
            return AnonymousDelete(RelativePath, id.ToString());
        }
    }
}
