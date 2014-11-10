using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class SessionTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Sessions"; }
        }

        [Test]
        public void GivenNoSessionSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoSessionSaveCommand();
            var response = WhenPost(command);
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptySessionSaveCommand_WhenPost_ThenReturnMultipleErrors()
        {
            var command = GivenEmptySessionSaveCommand();
            var response = WhenPost(command);
            AssertMultipleErrors(response, new[,] { { "The businessId field is required.", "session.businessId" }, 
                                                    { "The service field is required.", "session.service" },
                                                    { "The location field is required.", "session.location" },
                                                    { "The coach field is required.", "session.coach" },
                                                    { "The startDate field is required.", "session.startDate" },
                                                    { "The startTime field is required.", "session.startTime" },
                                                    { "The duration field is required.", "session.duration" },
                                                    { "The studentCapacity field is required.", "session.studentCapacity" } });
        }

        [Test]
        public void GivenNonExistentBusinessId_WhenPost_ThenReturnInvalidBusinessIdError()
        {
            var command = GivenNonExistentBusinessId();
            var response = WhenPost(command);
            AssertSingleError(response, "This business does not exist.", "session.businessId");
        }


        private string GivenNoSessionSaveCommand()
        {
            return "";
        }

        private string GivenEmptySessionSaveCommand()
        {
            return "{}";
        }

        private ApiSessionSaveCommand GivenNonExistentBusinessId()
        {
            return new ApiSessionSaveCommand
            {
                businessId = Guid.Empty,
                service = new ApiServiceKey { id = Guid.NewGuid() },
                location = new ApiLocationKey { id = Guid.NewGuid() },
                coach = new ApiCoachKey { id = Guid.NewGuid() },
                startDate = RandomString,
                startTime = RandomString,
                studentCapacity = 10,
                isOnlineBookable = true,
                colour = "blue"
            };
        }



        private Response WhenPost(string json)
        {
            return Post<SessionData>(json);
        }

        private Response WhenPost(ApiSessionSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenPost(json);
        }
    }
}
