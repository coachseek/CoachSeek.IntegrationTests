using System.Collections.Generic;
using System.Text;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.EmailTemplate
{
    public abstract class EmailTemplateTests : ScheduleTests
    {
        protected override string RelativePath { get { return "EmailTemplates"; } }

        protected void RegisterCustomSessionEmailTemplate(SetupData setup)
        {
            var command = new ApiEmailTemplateSaveCommand
            {
                subject = "Online Booking confirmation with <<BusinessName>>",
                body = GetCustomisedCustomerSessionBookingTemplateBody()
            };
            WhenTryPost(command, Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING, setup);
        }

        protected void RegisterCustomCourseEmailTemplate(SetupData setup)
        {
            var command = new ApiEmailTemplateSaveCommand
            {
                subject = "Online Booking confirmation with <<BusinessName>>",
                body = GetCustomisedCustomerCourseBookingTemplateBody()
            };
            WhenTryPost(command, Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING, setup);
        }


        protected ApiResponse GetEmailTemplate(string templateType, SetupData setup)
        {
            var relativePath = string.Format("{0}/{1}", RelativePath, templateType);
            return AuthenticatedGet<EmailTemplateData>(relativePath, setup);
        }

        protected ApiResponse WhenTryPost(ApiEmailTemplateSaveCommand command, string templateType, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryPost(json, templateType, setup);
        }

        protected ApiResponse WhenTryPost(string json, string templateType, SetupData setup)
        {
            var relativePath = string.Format("{0}/{1}", RelativePath, templateType);
            return new TestAuthenticatedApiClient().Post<LocationData>(json,
                                                                       setup.Business.UserName,
                                                                       setup.Business.Password,
                                                                       relativePath);
        }


        protected void AssertCustomerSessionBookingTemplateSubject(string templateType)
        {
            Assert.That(templateType, Is.EqualTo("CustomerSessionBooking"));
        }

        protected void AssertCustomerCourseBookingTemplateSubject(string templateType)
        {
            Assert.That(templateType, Is.EqualTo("CustomerCourseBooking"));
        }

        protected void AssertDefaultCustomerSessionBookingTemplate(EmailTemplateData template)
        {
            AssertCustomerSessionBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("Confirmation of Booking with <<BusinessName>>"));
            Assert.That(template.body, Is.EqualTo(GetDefaultCustomerSessionBookingTemplateBody()));
            AssertCustomerSessionBookingTemplatePlaceholders(template.placeholders);
        }

        protected void AssertDefaultCustomerCourseBookingTemplate(EmailTemplateData template)
        {
            AssertCustomerCourseBookingTemplateSubject(template.type);
            Assert.That(template.subject, Is.EqualTo("Confirmation of Booking with <<BusinessName>>"));
            Assert.That(template.body, Is.EqualTo(GetDefaultCustomerCourseBookingTemplateBody()));
            AssertCustomerCourseBookingTemplatePlaceholders(template.placeholders);
        }

        protected void AssertCustomerSessionBookingTemplatePlaceholders(IList<string> placeholders)
        {
            Assert.That(placeholders[0], Is.EqualTo("<<BusinessName>>"));
            Assert.That(placeholders[1], Is.EqualTo("<<CustomerFirstName>>"));
            Assert.That(placeholders[2], Is.EqualTo("<<CustomerLastName>>"));
            Assert.That(placeholders[3], Is.EqualTo("<<CustomerEmail>>"));
            Assert.That(placeholders[4], Is.EqualTo("<<CustomerPhone>>"));
            Assert.That(placeholders[5], Is.EqualTo("<<LocationName>>"));
            Assert.That(placeholders[6], Is.EqualTo("<<CoachFirstName>>"));
            Assert.That(placeholders[7], Is.EqualTo("<<CoachLastName>>"));
            Assert.That(placeholders[8], Is.EqualTo("<<ServiceName>>"));
            Assert.That(placeholders[9], Is.EqualTo("<<Date>>"));
            Assert.That(placeholders[10], Is.EqualTo("<<StartTime>>"));
            Assert.That(placeholders[11], Is.EqualTo("<<Duration>>"));
            Assert.That(placeholders[12], Is.EqualTo("<<CurrencySymbol>>"));
            Assert.That(placeholders[13], Is.EqualTo("<<SessionPrice>>"));
        }

        protected void AssertCustomerCourseBookingTemplatePlaceholders(IList<string> placeholders)
        {
            Assert.That(placeholders[0], Is.EqualTo("<<BusinessName>>"));
            Assert.That(placeholders[1], Is.EqualTo("<<CustomerFirstName>>"));
            Assert.That(placeholders[2], Is.EqualTo("<<CustomerLastName>>"));
            Assert.That(placeholders[3], Is.EqualTo("<<CustomerEmail>>"));
            Assert.That(placeholders[4], Is.EqualTo("<<CustomerPhone>>"));
            Assert.That(placeholders[5], Is.EqualTo("<<LocationName>>"));
            Assert.That(placeholders[6], Is.EqualTo("<<CoachFirstName>>"));
            Assert.That(placeholders[7], Is.EqualTo("<<CoachLastName>>"));
            Assert.That(placeholders[8], Is.EqualTo("<<ServiceName>>"));
            Assert.That(placeholders[9], Is.EqualTo("<<StartDate>>"));
            Assert.That(placeholders[10], Is.EqualTo("<<StartTime>>"));
            Assert.That(placeholders[11], Is.EqualTo("<<Duration>>"));
            Assert.That(placeholders[12], Is.EqualTo("<<SessionCount>>"));
            Assert.That(placeholders[13], Is.EqualTo("<<RepeatFrequency>>"));
            Assert.That(placeholders[14], Is.EqualTo("<<BookedSessionsList>>"));
            Assert.That(placeholders[15], Is.EqualTo("<<CurrencySymbol>>"));
            Assert.That(placeholders[16], Is.EqualTo("<<BookingPrice>>"));
        }

        protected string GetDefaultCustomerSessionBookingTemplateBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi <<CustomerFirstName>>!");
            builder.AppendLine("Thank you for booking with <<BusinessName>>!");
            builder.AppendLine("");
            builder.AppendLine("Your booking details are below:");
            builder.AppendLine("Location: <<LocationName>>");
            builder.AppendLine("Coach: <<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("Service: <<ServiceName>>");
            builder.AppendLine("Date, Time & Duration: <<Date>> at <<StartTime>> for <<Duration>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<SessionPrice>>");
            builder.AppendLine("");
            builder.AppendLine("Your details:");
            builder.AppendLine("Name: <<CustomerFirstName>> <<CustomerLastName>>");
            builder.AppendLine("Email: <<CustomerEmail>>");
            builder.AppendLine("Phone: <<CustomerPhone>>");
            builder.AppendLine("");
            builder.AppendLine("Thank you for your business! We're looking forward to seeing you there!");
            builder.AppendLine("");
            builder.AppendLine("Warm regards,");
            builder.AppendLine("");
            builder.AppendLine("<<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("<<BusinessName>>");

            return builder.ToString();
        }

        protected string GetCustomisedCustomerSessionBookingTemplateBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi <<CustomerFirstName>>!");
            builder.AppendLine("Thank you for booking with <<BusinessName>>!");
            builder.AppendLine("");
            builder.AppendLine("Your booking details are below:");
            builder.AppendLine("Location: <<LocationName>>");
            builder.AppendLine("Coach: <<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("Service: <<ServiceName>>");
            builder.AppendLine("Date, Time & Duration: <<Date>> at <<StartTime>> for <<Duration>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<SessionPrice>>");
            builder.AppendLine("");
            builder.AppendLine("Your details:");
            builder.AppendLine("Name: <<CustomerFirstName>> <<CustomerLastName>>");
            builder.AppendLine("Email: <<CustomerEmail>>");
            builder.AppendLine("Phone: <<CustomerPhone>>");
            builder.AppendLine("");
            builder.AppendLine("Please pay the due amount before the session to the following bank account:");
            builder.AppendLine("ASB Bank");
            builder.AppendLine("12-1234-0123456-00");
            builder.AppendLine("");
            builder.AppendLine("Thank you for your business! We're looking forward to seeing you there!");
            builder.AppendLine("");
            builder.AppendLine("Warm regards,");
            builder.AppendLine("");
            builder.AppendLine("<<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("<<BusinessName>>");

            return builder.ToString();
        }

        protected string GetDefaultCustomerCourseBookingTemplateBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi <<CustomerFirstName>>!");
            builder.AppendLine("Thank you for booking with <<BusinessName>>!");
            builder.AppendLine("");
            builder.AppendLine("Your booking details are below:");
            builder.AppendLine("Location: <<LocationName>>");
            builder.AppendLine("Coach: <<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("Service: <<ServiceName>>");
            builder.AppendLine("Timing: Starting on <<StartDate>> at <<StartTime>> for <<Duration>>, occurring for a total of <<SessionCount>> <<RepeatFrequency>>");
            builder.AppendLine("<<BookedSessionsList>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<BookingPrice>>");
            builder.AppendLine("");
            builder.AppendLine("Your details:");
            builder.AppendLine("Name: <<CustomerFirstName>> <<CustomerLastName>>");
            builder.AppendLine("Email: <<CustomerEmail>>");
            builder.AppendLine("Phone: <<CustomerPhone>>");
            builder.AppendLine("");
            builder.AppendLine("Thank you for your business! We're looking forward to seeing you there!");
            builder.AppendLine("");
            builder.AppendLine("Warm regards,");
            builder.AppendLine("");
            builder.AppendLine("<<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("<<BusinessName>>");

            return builder.ToString();
        }

        protected string GetCustomisedCustomerCourseBookingTemplateBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi <<CustomerFirstName>>!");
            builder.AppendLine("Thank you for booking with <<BusinessName>>!");
            builder.AppendLine("");
            builder.AppendLine("Your booking details are below:");
            builder.AppendLine("Location: <<LocationName>>");
            builder.AppendLine("Coach: <<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("Service: <<ServiceName>>");
            builder.AppendLine("Timing: Starting on <<StartDate>> at <<StartTime>> for <<Duration>>, occurring for a total of <<SessionCount>> <<RepeatFrequency>>");
            builder.AppendLine("<<BookedSessionsList>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<BookingPrice>>");
            builder.AppendLine("");
            builder.AppendLine("Your details:");
            builder.AppendLine("Name: <<CustomerFirstName>> <<CustomerLastName>>");
            builder.AppendLine("Email: <<CustomerEmail>>");
            builder.AppendLine("Phone: <<CustomerPhone>>");
            builder.AppendLine("");
            builder.AppendLine("Please pay the due amount before the session to the following bank account:");
            builder.AppendLine("ASB Bank");
            builder.AppendLine("12-1234-0123456-00");
            builder.AppendLine("");
            builder.AppendLine("Thank you for your business! We're looking forward to seeing you there!");
            builder.AppendLine("");
            builder.AppendLine("Warm regards,");
            builder.AppendLine("");
            builder.AppendLine("<<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("<<BusinessName>>");

            return builder.ToString();
        }
    }
}
