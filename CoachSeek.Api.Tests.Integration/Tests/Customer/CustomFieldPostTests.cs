using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    public class CustomFieldPostTests : CustomFieldTests
    {
        [TestFixture]
        public class CustomFieldCommandTests : CustomFieldPostTests
        {
            [Test]
            public void GivenNoCustomFieldSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoCustomFieldSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnNoDataError(response);
            }

            [Test]
            public void GivenEmptyCustomFieldSaveCommand_WhenTryPost_ThenReturnRootRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyCustomFieldSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnRootRequiredError(response);
            }


            private string GivenNoCustomFieldSaveCommand()
            {
                return "";
            }

            private string GivenEmptyCustomFieldSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class CustomFieldNewTests : CustomFieldPostTests
        {
            [Test]
            public void GivenEmptyStringForType_WhenTryCreateNewCustomField_ThenReturnMissingTypeError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyStringForType();
                var response = WhenTryCreateNewCustomField(command, setup);
                ThenReturnMissingTypeError(response);
            }

            [Test]
            public void GivenTypeIsNotCustomer_WhenTryCreateNewCustomField_ThenReturnInvalidTypeError()
            {
                var setup = RegisterBusiness();

                var command = GivenTypeIsNotCustomer();
                var response = WhenTryCreateNewCustomField(command, setup);
                ThenReturnInvalidTypeError(response);
            }

            [Test]
            public void GivenKeyAlreadyExists_WhenTryCreateNewCustomField_ThenReturnDuplicateKeyError()
            {
                var setup = RegisterBusiness();
                CreateCustomFields(setup);

                var command = GivenKeyAlreadyExists();
                var response = WhenTryCreateNewCustomField(command, setup);
                ThenReturnCustomFieldDuplicateError(response);
            }

            [Test]
            public void GivenValidNewCustomFieldData_WhenTryCreateNewCustomField_ThenCreateNewCustomField()
            {
                var setup = RegisterBusiness();

                var command = GivenValidNewCustomFieldData();
                var response = WhenTryCreateNewCustomField(command, setup);
                ThenCreateNewCustomField(response, setup);
            }


            private ApiCustomFieldSaveCommand GivenEmptyStringForType()
            {
                var command = CreateNewCustomFieldSaveCommand();
                command.type = string.Empty;

                return command;
            }

            private ApiCustomFieldSaveCommand GivenKeyAlreadyExists()
            {
                var command = CreateNewCustomFieldSaveCommand();

                return command;
            }

            private ApiCustomFieldSaveCommand GivenValidNewCustomFieldData()
            {
                return CreateNewCustomFieldSaveCommand();
            }


            private ApiResponse WhenTryCreateNewCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
            {
                var json = JsonSerialiser.Serialise(command);
                return WhenTryPost(json, setup);
            }


            private ApiCustomFieldSaveCommand GivenTypeIsNotCustomer()
            {
                var command = CreateNewCustomFieldSaveCommand();
                command.type = "broccoli";

                return command;
            }
        }


        [TestFixture]
        public class CustomFieldExistingTests : CustomFieldPostTests
        {
            [Test]
            public void GivenTypeIsNotCustomer_WhenTryUpdateExistingCustomField_ThenReturnInvalidTypeError()
            {
                var setup = RegisterBusiness();

                var command = GivenTypeIsNotCustomer();
                var response = WhenTryUpdateExistingCustomField(command, setup);
                ThenReturnInvalidTypeError(response);
            }

            [Test]
            public void GivenNonExistentCustomFieldId_WhenTryUpdateExistingCustomField_ThenReturnInvalidCustomFieldIdError()
            {
                var setup = RegisterBusiness();

                var command = GivenNonExistentCustomFieldId();
                var response = WhenTryUpdateExistingCustomField(command, setup);
                ThenReturnInvalidCustomFieldIdError(response, command.id.Value);
            }

            [Test]
            public void GivenWantToUpdateKeyButKeyAlreadyExists_WhenTryUpdateExistingCustomField_ThenReturnCustomFieldDuplicateError()
            {
                var setup = RegisterBusiness();
                CreateCustomFields(setup);

                var command = GivenWantToUpdateKeyButKeyAlreadyExists(setup);
                var response = WhenTryUpdateExistingCustomField(command, setup);
                ThenReturnCustomFieldDuplicateError(response);
            }

            [Test]
            public void GivenWantToUpdateForTheSameKey_WhenTryUpdateExistingCustomField_ThenUpdateCustomField()
            {
                var setup = RegisterBusiness();
                CreateCustomFields(setup);

                var command = GivenWantToUpdateForTheSameKey(setup);
                var response = WhenTryUpdateExistingCustomField(command, setup);
                ThenUpdateCustomField(response, setup);
            }

            [Test]
            public void GivenWantToUpdateWithDifferentButUniqueKey_WhenTryUpdateExistingCustomField_ThenUpdateCustomFieldWithoutKey()
            {
                var setup = RegisterBusiness();
                CreateCustomFields(setup);

                var command = GivenWantToUpdateWithDifferentButUniqueKey(setup);
                var response = WhenTryUpdateExistingCustomField(command, setup);
                ThenUpdateCustomFieldWithoutKey(response, setup);
            }


            private ApiCustomFieldSaveCommand GivenTypeIsNotCustomer()
            {
                var command = CreateUpdateCustomFieldSaveCommand();
                command.type = "broccoli";

                return command;
            }

            private ApiCustomFieldSaveCommand GivenNonExistentCustomFieldId()
            {
                var command = CreateUpdateCustomFieldSaveCommand();

                return command;
            }

            private ApiCustomFieldSaveCommand GivenWantToUpdateKeyButKeyAlreadyExists(SetupData setup)
            {
                return new ApiCustomFieldSaveCommand
                {
                    id = setup.CustomFields[1].id,
                    type = "customer",
                    name = " medical   inFo  ",
                    isRequired = true
                };
            }

            private ApiCustomFieldSaveCommand GivenWantToUpdateForTheSameKey(SetupData setup)
            {
                return new ApiCustomFieldSaveCommand
                {
                    id = setup.CustomFields[0].id,
                    type = "customer",
                    name = "Emergency Phone",
                    isRequired = false
                };
            }

            private ApiCustomFieldSaveCommand GivenWantToUpdateWithDifferentButUniqueKey(SetupData setup)
            {
                return new ApiCustomFieldSaveCommand
                {
                    id = setup.CustomFields[0].id,
                    type = "customer",
                    name = "Medical-Info:",
                    isRequired = false,
                    isActive = false
                };
            }


            private ApiResponse WhenTryUpdateExistingCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
            {
                var json = JsonSerialiser.Serialise(command);
                return WhenTryPost(json, setup);
            }


            private void ThenReturnInvalidCustomFieldIdError(ApiResponse response, Guid templateId)
            {
                AssertSingleError(response, ErrorCodes.CustomFieldTemplateIdInvalid, "This custom field template does not exist.", templateId.ToString());
            }

            private void ThenUpdateCustomField(ApiResponse response, SetupData setup)
            {
                var template = AssertSuccessResponse<CustomFieldTemplateData>(response);

                Assert.That(template.id, Is.EqualTo(setup.CustomFields[0].id));
                Assert.That(template.type, Is.EqualTo("customer"));
                Assert.That(template.key, Is.EqualTo("emergencyphone"));
                Assert.That(template.name, Is.EqualTo("Emergency Phone"));
                Assert.That(template.isRequired, Is.False);

                var getResponse = AuthenticatedGet<CustomFieldTemplateData>(RelativePath, template.id, setup);
                var getTemplate = (CustomFieldTemplateData)getResponse.Payload;

                Assert.That(getTemplate.id, Is.EqualTo(template.id));
                Assert.That(getTemplate.type, Is.EqualTo(template.type));
                Assert.That(getTemplate.key, Is.EqualTo(template.key));
                Assert.That(getTemplate.name, Is.EqualTo(template.name));
                Assert.That(getTemplate.isRequired, Is.EqualTo(template.isRequired));
            }

            private void ThenUpdateCustomFieldWithoutKey(ApiResponse response, SetupData setup)
            {
                var template = AssertSuccessResponse<CustomFieldTemplateData>(response);

                Assert.That(template.id, Is.EqualTo(setup.CustomFields[0].id));
                Assert.That(template.type, Is.EqualTo("customer"));
                Assert.That(template.key, Is.EqualTo("medicalinfo"));
                Assert.That(template.name, Is.EqualTo("Medical-Info:"));
                Assert.That(template.isRequired, Is.False);
                Assert.That(template.isActive, Is.False);

                var getResponse = AuthenticatedGet<CustomFieldTemplateData>(RelativePath, template.id, setup);
                var getTemplate = (CustomFieldTemplateData)getResponse.Payload;

                Assert.That(getTemplate.id, Is.EqualTo(template.id));
                Assert.That(getTemplate.type, Is.EqualTo(template.type));
                Assert.That(getTemplate.key, Is.EqualTo(template.key));
                Assert.That(getTemplate.name, Is.EqualTo(template.name));
                Assert.That(getTemplate.isRequired, Is.EqualTo(template.isRequired));
                Assert.That(getTemplate.isActive, Is.EqualTo(template.isActive));
            }
        }

        //[TestFixture]
        //public class CustomFieldSetIsActiveTests : CustomFieldPostTests
        //{
        //    [Test]
        //    public void GivenIsActiveButWantInactive_WhenTryToUpdateIsActive_ThenSetsIsActiveToFalse()
        //    {
        //        var setup = RegisterBusiness();
        //        CreateCustomFields(setup);

        //        var command = GivenIsActiveButWantInactive();
        //        WhenTryToUpdateIsActive(command, setup);
        //        ThenSetsIsActiveTo(false, setup);
        //    }

        //    [Test]
        //    public void GivenIsInactiveButWantActive_WhenTryToUpdateIsActive_ThenSetsIsActiveToTrue()
        //    {
        //        var setup = RegisterBusiness();
        //        CreateCustomFields(setup);
        //        SetCustomFieldToInactive(setup);

        //        var command = GivenIsInactiveButWantActive();
        //        WhenTryToUpdateIsActive(command, setup);
        //        ThenSetsIsActiveTo(true, setup);
        //    }


        //    private void SetCustomFieldToInactive(SetupData setup)
        //    {
        //        WhenTryToUpdateIsActive(GivenIsActiveButWantInactive(), setup);
        //    }

        //    private ApiCustomFieldsSetIsActiveCommand GivenIsActiveButWantInactive()
        //    {
        //        return new ApiCustomFieldsSetIsActiveCommand { isActive = false };
        //    }

        //    private ApiCustomFieldsSetIsActiveCommand GivenIsInactiveButWantActive()
        //    {
        //        return new ApiCustomFieldsSetIsActiveCommand { isActive = true };
        //    }

        //    private void WhenTryToUpdateIsActive(ApiCustomFieldsSetIsActiveCommand command, SetupData setup)
        //    {
        //        var json = JsonSerialiser.Serialise(command);
        //        var relativePath = string.Format("{0}/{1}", RelativePath, setup.CustomFields[0].id);

        //        WhenTryToUpdateIsActive(json, relativePath, setup);
        //    }

        //    protected void WhenTryToUpdateIsActive(string json, string relativePath, SetupData setup)
        //    {
        //        var response = AuthenticatedPost<CustomFieldTemplateData>(json, relativePath, setup);
        //    }

        //    private void ThenSetsIsActiveTo(bool isActive, SetupData setup)
        //    {
        //        var url = string.Format("{0}/{1}", RelativePath, setup.CustomFields[0].id);
        //        var response = AuthenticatedGet<CustomFieldTemplateData>(url, setup);
        //        var template = (CustomFieldTemplateData)response.Payload;

        //        Assert.That(template.isActive, Is.EqualTo(isActive));
        //    }
        //}


        private void CreateCustomFields(SetupData setup)
        {
            CreateCustomerMedicalInfoCustomField(setup);
            CreateCustomerSchoolYearCustomField(setup);
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

        private ApiResponse CreateNewCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return AuthenticatedPost<CustomFieldTemplateData>(json, RelativePath, setup);
        }

        private ApiCustomFieldSaveCommand CreateNewCustomFieldSaveCommand(string type,
                                                                          string name,
                                                                          bool isRequired = false)
        {
            return new ApiCustomFieldSaveCommand
            {
                type = type,
                name = name,
                isRequired = isRequired
            };
        }

        private ApiCustomFieldSaveCommand CreateNewCustomFieldSaveCommand()
        {
            return new ApiCustomFieldSaveCommand
            {
                type = "customer",
                name = "Medical Info",
                isRequired = true
            };
        }

        private ApiCustomFieldSaveCommand CreateUpdateCustomFieldSaveCommand()
        {
            return new ApiCustomFieldSaveCommand
            {
                id = Guid.NewGuid(),
                type = "customer",
                name = "Medical Info",
                key = "medicalinfo",
                isRequired = true
            };
        }

        private ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return AuthenticatedPost<CustomFieldTemplateData>(json, RelativePath, setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "type-required", "The Type field is required.", null },
                                                    { "name-required", "The Name field is required.", null },
                                                    { "isrequired-required", "The IsRequired field is required.", null } });
        }

        private void ThenReturnMissingTypeError(ApiResponse response)
        {
            AssertSingleError(response, "type-required", "The Type field is required.");
        }

        private void ThenReturnInvalidTypeError(ApiResponse response)
        {
            AssertSingleError(response, "customfieldtemplatetype-invalid", "The custom field template type 'broccoli' is not valid.", "broccoli");
        }

        private void ThenReturnCustomFieldDuplicateError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.CustomFieldTemplateDuplicate,
                "Custom Field Template of type 'customer' and for key 'medicalinfo' already exists.", "customer:medicalinfo");
        }

        private void ThenCreateNewCustomField(ApiResponse response, SetupData setup)
        {
            var template = AssertSuccessResponse<CustomFieldTemplateData>(response);

            Assert.That(template.type, Is.EqualTo("customer"));
            Assert.That(template.key, Is.EqualTo("medicalinfo"));
            Assert.That(template.name, Is.EqualTo("Medical Info"));
            Assert.That(template.isRequired, Is.True);

            var relativePath = string.Format("{0}?type={1}&key={2}", RelativePath, template.type, template.key);
            var getResponse = AuthenticatedGet<List<CustomFieldTemplateData>>(relativePath, setup);
            var getTemplate = ((List<CustomFieldTemplateData>)getResponse.Payload)[0];

            Assert.That(getTemplate.type, Is.EqualTo(template.type));
            Assert.That(getTemplate.key, Is.EqualTo(template.key));
            Assert.That(getTemplate.name, Is.EqualTo(template.name));
            Assert.That(getTemplate.isRequired, Is.EqualTo(template.isRequired));
        }
    }
}
