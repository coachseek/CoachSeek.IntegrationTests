using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachDeleteTests : CoachTests
    {
        [Test]
        public void GivenNonExistentCoachId_WhenTryDeleteCoach_ThenReturnNotFoundError()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentCoachId();
            var response = WhenTryDeleteCoach(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidCoachId_WhenTryDeleteCoachAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();
            RegisterCoachAaron(setup);

            var id = GivenValidCoachId(setup);
            var response = WhenTryDeleteCoachAnonymously(id);
            AssertUnauthorised(response);
        }


        private Guid GivenNonExistentCoachId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCoachId(SetupData setup)
        {
            return setup.Aaron.Id;
        }


        private ApiResponse WhenTryDeleteCoach(Guid id, SetupData setup)
        {
            return Delete(RelativePath, id.ToString(), setup);
        }

        private ApiResponse WhenTryDeleteCoachAnonymously(Guid id)
        {
            return DeleteAnonymously(RelativePath, id.ToString());
        }
    }
}
