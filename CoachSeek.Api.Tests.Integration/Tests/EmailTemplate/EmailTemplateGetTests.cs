using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.EmailTemplate
{
    [TestFixture]
    public class EmailTemplateGetTests : EmailTemplateTests
    {
        [Test]
        public void GivenNoSavedEmailTemplateCustomisation_WhenTryGetAllEmailTemplates_ThenReturnAllDefaultTemplates()
        {
            var setup = RegisterBusiness();

            GivenNoSavedEmailTemplateCustomisation();
            var response = WhenTryGetAllEmailTemplates(setup);
            ThenReturnAllDefaultTemplates(response);
        }

        [Test]
        public void GivenHaveSavedSessionEmailTemplateCustomisation_WhenTryGetAllEmailTemplates_ThenReturnCustomisedSessionEmailTemplateAndDefaultCourseEmailTemplates()
        {
            var setup = RegisterBusiness();

            GivenHaveSavedSessionEmailTemplateCustomisation(setup);
            var response = WhenTryGetAllEmailTemplates(setup);
            ThenReturnCustomisedSessionEmailTemplateAndDefaultCourseEmailTemplates(response);
        }

        [Test]
        public void GivenHaveSavedCourseEmailTemplateCustomisation_WhenTryGetAllEmailTemplates_ThenReturnCustomisedCourseEmailTemplateAndDefaultSessionEmailTemplates()
        {
            var setup = RegisterBusiness();

            GivenHaveSavedCourseEmailTemplateCustomisation(setup);
            var response = WhenTryGetAllEmailTemplates(setup);
            ThenReturnCustomisedCourseEmailTemplateAndDefaultSessionEmailTemplates(response);
        }

        [Test]
        public void GivenHaveSavedSessionAndCourseEmailTemplateCustomisation_WhenTryGetAllEmailTemplates_ThenReturnCustomisedSessionAndCourseEmailTemplates()
        {
            var setup = RegisterBusiness();

            GivenHaveSavedSessionAndCourseEmailTemplateCustomisation(setup);
            var response = WhenTryGetAllEmailTemplates(setup);
            ThenReturnCustomisedSessionAndCourseEmailTemplates(response);
        }


        [Test]
        public void GivenInvalidEmailTemplateType_WhenTryGetEmailTemplateByType_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var templateType = GivenInvalidEmailTemplateType();
            var response = WhenTryGetEmailTemplateByType(templateType, setup);
            ThenReturnNotFound(response);
        }

        [Test]
        public void GivenValidDefaultEmailTemplateType_WhenTryGetEmailTemplateByType_ThenReturnDefaultEmailTemplate()
        {
            var setup = RegisterBusiness();

            var templateType = GivenValidDefaultEmailTemplateType();
            var response = WhenTryGetEmailTemplateByType(templateType, setup);
            ThenReturnDefaultEmailTemplate(response);
        }

        [Test]
        public void GivenValidCustomisedEmailTemplateType_WhenTryGetEmailTemplateByType_ThenReturnCustomisedEmailTemplate()
        {
            var setup = RegisterBusiness();

            var templateType = GivenValidCustomisedEmailTemplateType(setup);
            var response = WhenTryGetEmailTemplateByType(templateType, setup);
            ThenReturnCustomisedEmailTemplate(response);
        }


        private void GivenNoSavedEmailTemplateCustomisation()
        {
            // Nothing to do. 
        }

        private void GivenHaveSavedSessionEmailTemplateCustomisation(SetupData setup)
        {
            RegisterCustomSessionEmailTemplate(setup);
        }

        private void GivenHaveSavedCourseEmailTemplateCustomisation(SetupData setup)
        {
            RegisterCustomCourseEmailTemplate(setup);
        }

        private void GivenHaveSavedSessionAndCourseEmailTemplateCustomisation(SetupData setup)
        {
            RegisterCustomSessionEmailTemplate(setup);
            RegisterCustomCourseEmailTemplate(setup);
        }

        private string GivenInvalidEmailTemplateType()
        {
            return Random.RandomString;
        }

        private string GivenValidDefaultEmailTemplateType()
        {
            return Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING;
        }

        private string GivenValidCustomisedEmailTemplateType(SetupData setup)
        {
            RegisterCustomSessionEmailTemplate(setup);

            return Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING;
        }


        private ApiResponse WhenTryGetAllEmailTemplates(SetupData setup)
        {
            return AuthenticatedGet<List<EmailTemplateData>>(RelativePath, setup);
        }

        private ApiResponse WhenTryGetEmailTemplateByType(string templateType, SetupData setup)
        {
            var relativePath = string.Format("{0}/{1}", RelativePath, templateType);
            return AuthenticatedGet<EmailTemplateData>(relativePath, setup);
        }


        private void ThenReturnAllDefaultTemplates(ApiResponse response)
        {
            var templates = AssertSuccessResponse<List<EmailTemplateData>>(response);
            Assert.That(templates.Count, Is.EqualTo(2));

            AssertDefaultCustomerCourseBookingTemplate(templates[0]);
            AssertDefaultCustomerSessionBookingTemplate(templates[1]);
        }

        private void ThenReturnCustomisedSessionEmailTemplateAndDefaultCourseEmailTemplates(ApiResponse response)
        {
            var templates = AssertSuccessResponse<List<EmailTemplateData>>(response);
            Assert.That(templates.Count, Is.EqualTo(2));

            AssertDefaultCustomerCourseBookingTemplate(templates[0]);
            AssertCustomisedCustomerSessionBookingTemplate(templates[1]);
        }

        private void ThenReturnCustomisedCourseEmailTemplateAndDefaultSessionEmailTemplates(ApiResponse response)
        {
            var templates = AssertSuccessResponse<List<EmailTemplateData>>(response);
            Assert.That(templates.Count, Is.EqualTo(2));

            AssertCustomisedCustomerCourseBookingTemplate(templates[0]);
            AssertDefaultCustomerSessionBookingTemplate(templates[1]);
        }

        private void ThenReturnCustomisedSessionAndCourseEmailTemplates(ApiResponse response)
        {
            var templates = AssertSuccessResponse<List<EmailTemplateData>>(response);
            Assert.That(templates.Count, Is.EqualTo(2));

            AssertCustomisedCustomerCourseBookingTemplate(templates[0]);
            AssertCustomisedCustomerSessionBookingTemplate(templates[1]);
        }

        private void ThenReturnDefaultEmailTemplate(ApiResponse response)
        {
            var template = AssertSuccessResponse<EmailTemplateData>(response);

            AssertDefaultCustomerSessionBookingTemplate(template);
        }

        private void ThenReturnCustomisedEmailTemplate(ApiResponse response)
        {
            var template = AssertSuccessResponse<EmailTemplateData>(response);

            AssertCustomisedCustomerSessionBookingTemplate(template);
        }


        private void AssertCustomisedCustomerCourseBookingTemplate(EmailTemplateData template)
        {
            AssertCustomerCourseBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("Online Booking confirmation with <<BusinessName>>"));
            Assert.That(template.body, Is.EqualTo(GetCustomisedCustomerCourseBookingTemplateBody()));
            AssertCustomerCourseBookingTemplatePlaceholders(template.placeholders);
        }

        private void AssertCustomisedCustomerSessionBookingTemplate(EmailTemplateData template)
        {
            AssertCustomerSessionBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("Online Booking confirmation with <<BusinessName>>"));
            Assert.That(template.body, Is.EqualTo(GetCustomisedCustomerSessionBookingTemplateBody()));
            AssertCustomerSessionBookingTemplatePlaceholders(template.placeholders);
        }
    }
}
