using System;
using System.Collections.Generic;
using System.Linq;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomFieldGetTests : CustomFieldTests
    {
        [Test]
        public void GivenNonExistentId_WhenTryGetById_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var id = GivenNonExistentId();
            var response = WhenTryGetById(id, setup);
            ThenReturnNotFound(response);
        }

        [Test]
        public void GivenExistingId_WhenTryGetById_ThenReturnCustomField()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var id = GivenExistingId(setup);
            var response = WhenTryGetById(id, setup);
            ThenReturnCustomField(response, setup);
        }

        [Test]
        public void GivenNonExistentType_WhenTryGetByTypeAndKey_ThenReturnEmptyList()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var type = GivenNonExistentType();
            var response = WhenTryGetByTypeAndKey(type, "medicalinfo", setup);
            ThenReturnEmptyList(response);
        }

        [Test]
        public void GivenNonExistentKey_WhenTryGetByTypeAndKey_ThenReturnEmptyList()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var key = GivenNonExistentKey();
            var response = WhenTryGetByTypeAndKey("customer", key, setup);
            ThenReturnEmptyList(response);
        }

        [Test]
        public void GivenExistingKey_WhenTryGetByTypeAndKey_ThenReturnSingleCustomField()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var key = GivenExistingKey();
            var response = WhenTryGetByTypeAndKey("customer", key, setup);
            ThenReturnSingleCustomField(response, setup);
        }

        [Test]
        public void GivenNoKey_WhenTryGetByTypeAndKey_ThenReturnAllCustomFieldsForTheType()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var key = GivenNoKey();
            var response = WhenTryGetByTypeAndKey("customer", key, setup);
            ThenReturnAllCustomFieldsForTheType(response);
        }

        [Test]
        public void GivenNoKey_WhenTryGetAnonymously_ThenReturnAllCustomFieldsForTheType()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);

            var key = GivenNoKey();
            var response = WhenTryGetAnonymously("customer", key, setup);
            ThenReturnAllCustomFieldsForTheType(response);
        }


        //var relativePath = string.Format("{0}?type={1}&key={2}", RelativePath, template.type, template.key);
        //var getResponse = AuthenticatedGet<CustomFieldTemplateData>(relativePath, setup);
        //var getTemplate = (CustomFieldTemplateData)getResponse.Payload;

        private void CreateCustomFields(SetupData setup)
        {
            CreateCustomerMedicalInfoCustomField(setup);
            CreateCustomerSchoolYearCustomField(setup);
            //CreateLocationAreaCodeCustomField(setup);
        }

        private void CreateCustomerMedicalInfoCustomField(SetupData setup)
        {
            var command = CreateNewCustomFieldSaveCommand("Customer", "Medical Info", true);
            var response = CreateNewCustomField(command, setup);
            setup.CustomFields.Add((CustomFieldTemplateData)response.Payload);
        }

        private void CreateCustomerSchoolYearCustomField(SetupData setup)
        {
            var command = CreateNewCustomFieldSaveCommand("customer", "School Year");
            var response = CreateNewCustomField(command, setup);
            setup.CustomFields.Add((CustomFieldTemplateData)response.Payload);
        }

        //private void CreateLocationAreaCodeCustomField(SetupData setup)
        //{
        //    var command = CreateNewCustomFieldSaveCommand("Location", "Area Code");
        //    var response = CreateNewCustomField(command, setup);
        //    setup.CustomFields.Add((CustomFieldTemplateData)response.Payload);
        //}


        private Guid GivenNonExistentId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingId(SetupData setup)
        {
            return setup.CustomFields[0].id;
        }

        private string GivenNonExistentType()
        {
            return "sometype";
        }

        private string GivenNonExistentKey()
        {
            return "somekey";
        }

        private string GivenExistingKey()
        {
            return "medicalinfo";
        }

        private string GivenNoKey()
        {
            return null;
        }

        private ApiResponse WhenTryGetById(Guid id, SetupData setup)
        {
            return AuthenticatedGet<CustomFieldTemplateData>(RelativePath, id, setup);
        }

        private ApiResponse WhenTryGetByTypeAndKey(string type, string key, SetupData setup)
        {
            var relativePath = string.Format("{0}{1}", RelativePath, BuildQueryString(type, key));
            return AuthenticatedGet<List<CustomFieldTemplateData>>(relativePath, setup);
        }

        private ApiResponse WhenTryGetAnonymously(string type, string key, SetupData setup)
        {
            var relativePath = string.Format("{0}{1}", RelativePath, BuildQueryString(type, key));
            return BusinessAnonymousGet<List<CustomFieldTemplateData>>(relativePath, setup);
        }


        private string BuildQueryString (string type, string key)
        {
            var queryString = string.Empty;
            if (type != null)
                queryString = string.Format("?type={0}", type);
            if (key != null)
            {
                if (queryString == string.Empty)
                    queryString = string.Format("?key={0}", key);
                else
                    queryString = string.Format("{0}&key={1}", queryString, key);
            }
            return queryString;
        }

        private void ThenReturnEmptyList(ApiResponse response)
        {
            var templates = (List<CustomFieldTemplateData>) response.Payload;
            Assert.That(templates.Count, Is.EqualTo(0));
        }

        private void ThenReturnSingleCustomField(ApiResponse response, SetupData setup)
        {
            var templates = (List<CustomFieldTemplateData>)response.Payload;
            Assert.That(templates.Count, Is.EqualTo(1));

            var template = templates.First();
            Assert.That(template.id, Is.EqualTo(setup.CustomFields[0].id));
            Assert.That(template.type, Is.EqualTo("customer"));
            Assert.That(template.key, Is.EqualTo("medicalinfo"));
            Assert.That(template.name, Is.EqualTo("Medical Info"));
            Assert.That(template.isRequired, Is.True);
            Assert.That(template.isActive, Is.True);
        }

        private void ThenReturnCustomField(ApiResponse response, SetupData setup)
        {
            var template = (CustomFieldTemplateData)response.Payload;

            Assert.That(template.id, Is.EqualTo(setup.CustomFields[0].id));
            Assert.That(template.type, Is.EqualTo("customer"));
            Assert.That(template.key, Is.EqualTo("medicalinfo"));
            Assert.That(template.name, Is.EqualTo("Medical Info"));
            Assert.That(template.isRequired, Is.True);
            Assert.That(template.isActive, Is.True);
        }

        private void ThenReturnAllCustomFieldsForTheType(ApiResponse response)
        {
            var templates = (List<CustomFieldTemplateData>)response.Payload;
            Assert.That(templates.Count, Is.EqualTo(2));

            var firstTemplate = templates.First();
            Assert.That(firstTemplate.type, Is.EqualTo("customer"));
            Assert.That(firstTemplate.key, Is.EqualTo("medicalinfo"));
            Assert.That(firstTemplate.name, Is.EqualTo("Medical Info"));
            Assert.That(firstTemplate.isRequired, Is.True);
            Assert.That(firstTemplate.isActive, Is.True);

            var secondTemplate = templates[1];
            Assert.That(secondTemplate.type, Is.EqualTo("customer"));
            Assert.That(secondTemplate.key, Is.EqualTo("schoolyear"));
            Assert.That(secondTemplate.name, Is.EqualTo("School Year"));
            Assert.That(secondTemplate.isRequired, Is.False);
            Assert.That(secondTemplate.isActive, Is.True);
        }

        private ApiCustomFieldSaveCommand CreateNewCustomFieldSaveCommand(string type, string name, bool isRequired = false)
        {
            return new ApiCustomFieldSaveCommand
            {
                type = type,
                name = name,
                isRequired = isRequired
            };
        }

        private ApiResponse CreateNewCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return AuthenticatedPost<CustomFieldTemplateData>(json, RelativePath, setup);
        }
    }
}
