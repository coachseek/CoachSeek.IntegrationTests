using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    [TestFixture]
    public class LocationDeleteTests : LocationTests
    {
        [Test]
        public void GivenNonExistentLocationId_WhenTryDelete_ThenReturnNotFound()
        {
            var id = GivenNonExistentLocationId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTryDeleteAnonymously_ThenReturnUnauthorised()
        {
            var id = GivenValidLocationId();
            var response = WhenTryDeleteAnonymously(id);
            AssertUnauthorised(response);
        }


        private Guid GivenNonExistentLocationId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidLocationId()
        {
            return RemueraId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<LocationData>("Locations", id);
        }

        private Response WhenTryDeleteAnonymously(Guid id)
        {
            return DeleteAnonymously<LocationData>("Locations", id);
        }
    }
}
