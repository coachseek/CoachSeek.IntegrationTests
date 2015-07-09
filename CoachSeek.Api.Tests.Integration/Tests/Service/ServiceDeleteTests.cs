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
        public void GivenNonExistentServiceId_WhenTryDelete_ThenReturnNotFound()
        {
            var id = GivenNonExistentServiceId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTryDeleteAnonymously_ThenReturnUnauthorised()
        {
            var id = GivenValidServiceId();
            var response = WhenTryDeleteAnonymously(id);
            AssertUnauthorised(response);
        }


        private Guid GivenNonExistentServiceId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidServiceId()
        {
            return MiniRedId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<ServiceData>("Services", id);
        }

        private ApiResponse WhenTryDeleteAnonymously(Guid id)
        {
            return DeleteAnonymously<ServiceData>("Services", id);
        }
    }
}
