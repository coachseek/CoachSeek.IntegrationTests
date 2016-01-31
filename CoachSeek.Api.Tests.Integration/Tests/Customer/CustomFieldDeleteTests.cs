//using System.Collections.Generic;
//using System.Net;
//using Coachseek.API.Client.Models;
//using Coachseek.API.Client.Services;
//using CoachSeek.Api.Tests.Integration.Models;
//using NUnit.Framework;

//namespace CoachSeek.Api.Tests.Integration.Tests.Customer
//{
//    [TestFixture]
//    public class CustomFieldDeleteTests : CustomFieldTests
//    {
//        [Test]
//        public void GivenNonExistentKey_WhenTryDelete_ThenReturnNotFoundErrorResponse()
//        {
//            var setup = RegisterBusiness();
//            var key = CreateMedicalInfoCustomField(setup);

//            var nonexistentKey = GivenNonExistentKey();
//            var response = WhenTryDelete(nonexistentKey, setup);
//            AssertNotFound(response);
//        }

//        [Test]
//        public void GivenExistingKey_WhenTryDelete_ThenCustomFieldIsDeleted()
//        {
//            var setup = RegisterBusiness();

//            var key = GivenExistingKey(setup);
//            var response = WhenTryDelete(key, setup);
//            ThenCustomFieldIsDeleted(response, key, setup);
//        }


//        private ApiCustomFieldSaveCommand CreateNewCustomFieldSaveCommand()
//        {
//            return new ApiCustomFieldSaveCommand
//            {
//                type = "customer",
//                name = "Medical Info",
//                isRequired = true
//            };
//        }

//        private ApiResponse CreateNewCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
//        {
//            var json = JsonSerialiser.Serialise(command);
//            return AuthenticatedPost<CustomFieldTemplateData>(json, RelativePath, setup);
//        }

//        private string CreateMedicalInfoCustomField(SetupData setup)
//        {
//            var command = CreateNewCustomFieldSaveCommand();
//            var response = CreateNewCustomField(command, setup);
//            return ((CustomFieldTemplateData)response.Payload).key;
//        }

//        private string GivenNonExistentKey()
//        {
//            return "nonexistent";
//        }

//        private string GivenExistingKey(SetupData setup)
//        {
//            return CreateMedicalInfoCustomField(setup);
//        }

//        private ApiResponse WhenTryDelete(string key, SetupData setup)
//        {
//            var relativePath = string.Format("{0}?type=Customer&key={1}", RelativePath, key);
//            return AuthenticatedDelete(relativePath, setup);
//        }

//        private void ThenCustomFieldIsDeleted(ApiResponse response, string key, SetupData setup)
//        {
//            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

//            var relativePath = string.Format("{0}?type=Customer&key={1}", RelativePath, key);
//            var getResponse = AuthenticatedGet<List<CustomFieldTemplateData>>(relativePath, setup);
//            ThenReturnEmptyList(getResponse);
//        }

//        private void ThenReturnEmptyList(ApiResponse response)
//        {
//            var templates = (List<CustomFieldTemplateData>)response.Payload;
//            Assert.That(templates.Count, Is.EqualTo(0));
//        }
//    }
//}
