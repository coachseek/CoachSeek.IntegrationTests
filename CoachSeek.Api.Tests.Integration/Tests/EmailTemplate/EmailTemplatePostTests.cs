using System;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.EmailTemplate
{
    [TestFixture]
    public class EmailTemplatePostTests : EmailTemplateTests
    {
        [Test]
        public void GivenNoEmailTemplateSaveCommand_WhenTryPost_ThenReturnNoDataError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoEmailTemplateSaveCommand();
            var response = WhenTryPost(command, Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            ThenReturnNoDataError(response);
        }

        [Test]
        public void GivenInvalidEmailTemplateType_WhenTryPost_ThenReturnInvalidTemplateTypeError()
        {
            var setup = RegisterBusiness();

            var templateType = GivenInvalidEmailTemplateType();
            var response = WhenTryPost(templateType, setup);
            ThenReturnInvalidTemplateTypeError(response);
        }

        [Test]
        public void GivenNoSubjectSaveCommand_WhenTryPost_ThenReturnSubjectRequiredError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoSubjectSaveCommand();
            var response = WhenTryPost(command, Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            ThenReturnSubjectRequiredError(response);
        }

        [Test]
        public void GivenValidEmailTemplateSaveCommand_WhenTryPostAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();

            var command = GivenValidEmailTemplateSaveCommand();
            var response = WhenTryPostAnonymously(command, Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            AssertUnauthorised(response);
        }

        [Test]
        public void GivenDefaultCustomerSessionBookingEmailTemplate_WhenTryPost_ThenCreateCustomisedCustomerSessionBookingEmailTemplate()
        {
            var setup = RegisterBusiness();

            GivenDefaultCustomerSessionBookingEmailTemplate();
            var response = WhenTryPost(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            ThenCreateCustomisedCustomerSessionBookingEmailTemplate(response, setup);
        }

        [Test]
        public void GivenCustomisedCustomerSessionBookingEmailTemplate_WhenTryPost_ThenUpdateCustomisedCustomerSessionBookingEmailTemplate()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomisedCustomerSessionBookingEmailTemplate(setup);
            var response = WhenTryPost(command, Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            ThenUpdateCustomisedCustomerSessionBookingEmailTemplate(response, setup);
        }


        private string GivenNoEmailTemplateSaveCommand()
        {
            return "";
        }

        private ApiEmailTemplateSaveCommand GivenNoSubjectSaveCommand()
        {
            return new ApiEmailTemplateSaveCommand
            {
                subject = "",
                body = "body"
            };
        }

        private string GivenInvalidEmailTemplateType()
        {
            return "bogus";
        }

        private void GivenDefaultCustomerSessionBookingEmailTemplate()
        {
            // Nothing to do
        }

        private ApiEmailTemplateSaveCommand GivenCustomisedCustomerSessionBookingEmailTemplate(SetupData setup)
        {
            WhenTryPost(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);

            return new ApiEmailTemplateSaveCommand
            {
                subject = "subject too",
                body = "body too"
            };
        }


        private void ThenReturnInvalidTemplateTypeError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.EmailTemplateTypeInvalid, "Email template type 'bogus' is not valid.", "bogus");
        }

        private void ThenReturnSubjectRequiredError(ApiResponse response)
        {
            AssertSingleError(response, "subject-required", "The Subject field is required.", null);
        }

        private void ThenCreateCustomisedCustomerSessionBookingEmailTemplate(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = GetEmailTemplate(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            var template = AssertSuccessResponse<EmailTemplateData>(getResponse);

            AssertCustomerSessionBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("subject"));
            Assert.That(template.body, Is.EqualTo("body"));
            AssertCustomerSessionBookingTemplatePlaceholders(template.placeholders);
        }

        private void ThenUpdateCustomisedCustomerSessionBookingEmailTemplate(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = GetEmailTemplate(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            var template = AssertSuccessResponse<EmailTemplateData>(getResponse);

            AssertCustomerSessionBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("subject too"));
            Assert.That(template.body, Is.EqualTo("body too"));
            AssertCustomerSessionBookingTemplatePlaceholders(template.placeholders);
        }


        private ApiEmailTemplateSaveCommand GivenValidEmailTemplateSaveCommand()
        {
            return new ApiEmailTemplateSaveCommand
            {
                subject = "subject",
                body = "body"
            };
        }


        private ApiResponse WhenTryPost(string templateType, SetupData setup)
        {
            var command = GivenValidEmailTemplateSaveCommand();
            var json = JsonSerialiser.Serialise(command);
            return WhenTryPost(json, templateType, setup);
        }

        private ApiResponse WhenTryPostAnonymously(ApiEmailTemplateSaveCommand command, string templateType, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, templateType);
            return new TestBusinessAnonymousApiClient().Post<LocationData>(json,
                                                                           setup.Business.Domain,
                                                                           relativePath);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertSingleError(response, "The name field is required.", "location.name");
        }

        private void ThenReturnInvalidLocationIdError(ApiResponse response)
        {
            AssertSingleError(response, "This location does not exist.", "location.id");
        }

        private void ThenReturnDuplicateLocationError(ApiResponse response)
        {
            AssertSingleError(response, "This location already exists.", "location.name");
        }

        private void ThenCreateNewLocation(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.name, Is.EqualTo("Mt Eden Squash Club"));
        }

        private void ThenReturnExistingLocationSuccessResponse(ApiResponse response, 
                                                               SetupData setup, 
                                                               string newLocationName = null)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.EqualTo(setup.Orakei.Id));
            Assert.That(location.name, Is.EqualTo(newLocationName ?? setup.Orakei.Name));
        }
    }
}
