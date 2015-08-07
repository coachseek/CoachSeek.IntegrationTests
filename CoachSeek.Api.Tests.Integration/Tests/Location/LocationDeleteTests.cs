using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    [TestFixture]
    public class LocationDeleteTests : LocationTests
    {
        [Test]
        public void GivenNonExistentLocationId_WhenTryDeleteLocation_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentLocationId();
            var response = WhenTryDeleteLocation(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTryDeleteLocationAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();
            RegisterLocationRemuera(setup);

            var id = GivenValidLocationId(setup);
            var response = WhenTryDeleteLocationAnonymously(id);
            AssertUnauthorised(response);
        }


        private Guid GivenNonExistentLocationId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidLocationId(SetupData setup)
        {
            return setup.Remuera.Id;
        }


        private ApiResponse WhenTryDeleteLocation(Guid id, SetupData setup)
        {
            return Delete(RelativePath, id.ToString(), setup);
        }

        private ApiResponse WhenTryDeleteLocationAnonymously(Guid id)
        {
            return DeleteAnonymously(RelativePath, id.ToString());
        }
    }
}
