using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.EmailTemplate
{
    [TestFixture]
    public class EmailTemplateDeleteTests : EmailTemplateTests
    {
        [Test]
        public void GivenInvalidEmailTemplateType_WhenTryDeleteEmailTemplate_ThenReturnNotFoundError()
        {
            var setup = RegisterBusiness();

            var templateType = GivenInvalidEmailTemplateType();
            var response = WhenTryDeleteEmailTemplate(templateType, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidEmailTemplateType_WhenTryDeleteEmailTemplateAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();
            RegisterCustomSessionEmailTemplate(setup);

            var templateType = GivenValidEmailTemplateType();
            var response = WhenTryDeleteEmailTemplateAnonymously(templateType);
            AssertUnauthorised(response);
        }

        [Test]
        public void GivenDefaultEmailTemplate_WhenTryDeleteEmailTemplate_ThenSucceedsButDidNothing()
        {
            var setup = RegisterBusiness();

            var templateType = GivenDefaultEmailTemplate();
            var response = WhenTryDeleteEmailTemplate(templateType, setup);
            ThenSucceedsButDidNothing(response, setup);
        }

        [Test]
        public void GivenCustomisedEmailTemplate_WhenTryDeleteEmailTemplate_ThenDeleteCustomisedEmailTemplate()
        {
            var setup = RegisterBusiness();

            var templateType = GivenCustomisedEmailTemplate(setup);
            var response = WhenTryDeleteEmailTemplate(templateType, setup);
            ThenDeleteCustomisedEmailTemplate(response, setup);
        }


        private string GivenInvalidEmailTemplateType()
        {
            return Random.RandomString;
        }

        private string GivenValidEmailTemplateType()
        {
            return Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION;
        }

        private string GivenDefaultEmailTemplate()
        {
            return Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION;
        }

        private string GivenCustomisedEmailTemplate(SetupData setup)
        {
            RegisterCustomSessionEmailTemplate(setup);

            return Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION;
        }


        private ApiResponse WhenTryDeleteEmailTemplate(string templateType, SetupData setup)
        {
            return Delete(RelativePath, templateType, setup);
        }

        private ApiResponse WhenTryDeleteEmailTemplateAnonymously(string templateType)
        {
            return DeleteAnonymously(RelativePath, templateType);
        }

        private void ThenSucceedsButDidNothing(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = GetEmailTemplate(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            var template = AssertSuccessResponse<EmailTemplateData>(getResponse);
            AssertDefaultCustomerSessionBookingTemplate(template);
        }

        private void ThenDeleteCustomisedEmailTemplate(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = GetEmailTemplate(Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION, setup);
            var template = AssertSuccessResponse<EmailTemplateData>(getResponse);
            AssertDefaultCustomerSessionBookingTemplate(template);
        }
    }
}
