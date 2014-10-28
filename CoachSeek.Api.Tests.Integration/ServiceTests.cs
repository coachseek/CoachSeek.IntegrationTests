using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration
{
    [TestFixture]
    public class ServiceTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Services"; }
        }

        [Test]
        public void GivenNoServiceSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoServiceSaveCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }


        private string GivenNoServiceSaveCommand()
        {
            return "";
        }


        private Response WhenPost(string json)
        {
            return Post<ServiceData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], null, "Please post us some data!");
        }
    }
}
