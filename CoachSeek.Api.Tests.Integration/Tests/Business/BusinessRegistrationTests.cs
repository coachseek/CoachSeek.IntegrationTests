using System;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    [TestFixture]
    public class BusinessRegistrationTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "BusinessRegistration"; }
        }


        [Test]
        public void GivenNoBusinessRegistrationCommand_WhenTryRegisterBusiness_ThenReturnNoDataError()
        {
            var command = GivenNoBusinessRegistrationCommand();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnNoDataError(response);
        }

        [Test]
        public void GivenEmptyBusinessRegistrationCommand_WhenTryRegisterBusiness_ThenReturnRootRequiredError()
        {
            var command = GivenEmptyBusinessRegistrationCommand();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnRootRequiredError(response);
        }

        [Test]
        public void GivenMissingProperties_WhenTryRegisterBusiness_ThenReturnMissingPropertiesError()
        {
            var command = GivenMissingProperties();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnMissingPropertiesError(response);
        }

        [Test]
        public void GivenMultipleErrorsOnProperties_WhenTryRegisterBusiness_ThenReturnMultipleErrorResponse()
        {
            var business = GivenMultipleErrorsOnProperties();
            var response = WhenTryRegisterBusiness(business);
            ThenReturnMultipleErrorResponse(response);
        }

        [Test]
        public void GivenNoCurrency_WhenTryRegisterBusiness_ThenReturnNoCurrencyError()
        {
            var business = GivenNoCurrency();
            var response = WhenTryRegisterBusiness(business);
            ThenReturnNoCurrencyError(response);
        }

        [Test]
        public void GivenInvalidCurrency_WhenTryRegisterBusiness_ThenReturnsCurrencyNotSupportedError()
        {
            var business = GivenInvalidCurrency();
            var response = WhenTryRegisterBusiness(business);
            ThenReturnsCurrencyNotSupportedError(response);
        }

        [Test]
        public void GivenDuplicateBusinessAdmin_WhenTryRegisterBusiness_ThenReturnDuplicateAdminError()
        {
            var setup = GivenDuplicateBusinessAdmin();
            var response = WhenTryRegisterBusiness(setup);
            ThenReturnDuplicateAdminError(response, setup.Business.Admin.email);
        }

        [Test]
        public void GivenUniqueBusinessAdmin_WhenTryRegisterBusinessUsingHttp_ThenReturnForbiddenError()
        {
            var business = GivenUniqueBusinessAdmin();
            var response = WhenTryRegisterBusinessUsingHttp(business);
            ThenReturnForbiddenError(response);
        }

        [Test]
        public void GivenUniqueBusinessAdmin_WhenTryRegisterBusiness_ThenCreateNewBusiness()
        {
            var business = GivenUniqueBusinessAdmin();
            var response = WhenTryRegisterBusiness(business);
            ThenCreateNewBusiness(response, business);
        }

        [Test]
        public void GivenMissingSport_WhenTryRegisterBusiness_ThenCreateNewBusinessWithoutSport()
        {
            var business = GivenMissingSport();
            var response = WhenTryRegisterBusiness(business);
            ThenCreateNewBusinessWithoutSport(response, business);
        }


        private string GivenNoBusinessRegistrationCommand()
        {
            return "";
        }

        private string GivenEmptyBusinessRegistrationCommand()
        {
            return "{}";
        }

        private string GivenMissingProperties()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness(),
                admin = new ApiBusinessAdmin()
            };

            return JsonSerialiser.Serialise(registration);
        }

        private ExpectedBusiness GivenMultipleErrorsOnProperties()
        {
            return new ExpectedBusiness(Random.RandomString,
                                        "abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                                        "ABCDE",
                                        "Bob",
                                        "Smith",
                                        "abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                                        "abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                                        "abcdefghijklmnopqrstuvwxyz01234567890");
        }

        private ExpectedBusiness GivenInvalidCurrency()
        {
            return new ExpectedBusiness(Random.RandomString, "XX", Random.RandomEmail);
        }

        private ExpectedBusiness GivenMissingSport()
        {
            return new ExpectedBusiness(Random.RandomString,
                                        null,
                                        "USD",
                                        "Bob",
                                        "Smith",
                                        Random.RandomEmail,
                                        "0900COACHSEEK",
                                        "password1",
                                        DateTime.UtcNow);
        }

        private SetupData GivenDuplicateBusinessAdmin()
        {
            return RegisterBusiness();
        }

        private ExpectedBusiness GivenUniqueBusinessAdmin()
        {
            return new ExpectedBusiness(Random.RandomString, "Tennis", "USD", "Bob", "Smith", Random.RandomEmail, "021 123456", "password1");
        }

        private ExpectedBusiness GivenNoCurrency()
        {
            return new ExpectedBusiness(Random.RandomString, "", Random.RandomEmail);
        }

        private ApiResponse WhenTryRegisterBusiness(SetupData setup)
        {
            return BusinessRegistrar.RegisterBusiness(setup.Business);
        }

        private ApiResponse WhenTryRegisterBusiness(ExpectedBusiness business)
        {
            return BusinessRegistrar.RegisterBusiness(business);
        }

        private ApiResponse WhenTryRegisterBusiness(string json)
        {
            return AnonymousPost<RegistrationData>(json, RelativePath);
        }

        private ApiResponse WhenTryRegisterBusinessUsingHttp(ExpectedBusiness business)
        {
            return BusinessRegistrar.RegisterBusiness(business, "http");
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "business-required", "The Business field is required.", null },
                                                    { "admin-required", "The Admin field is required.", null } });
        }

        private void ThenReturnMissingPropertiesError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "name-required", "The Name field is required.", null },
                                                    { "currency-required", "The Currency field is required.", null },
                                                    { "firstname-required", "The FirstName field is required.", null },
                                                    { "lastname-required", "The LastName field is required.", null },
                                                    { "email-required", "The Email field is required.", null },
                                                    { "password-required", "The Password field is required.", null } });
        }

        private void ThenReturnMultipleErrorResponse(ApiResponse response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApiApplicationError[]>());
            var errors = (ApiApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(6));
            AssertApplicationError(errors[0], "currency-too-long", "The field Currency must be a string with a maximum length of 3.");
            AssertApplicationError(errors[1], "sport-too-long", "The field Sport must be a string with a maximum length of 100.");
            AssertMultipleEmailErrors(errors[2], errors[3]);
            AssertApplicationError(errors[4], "phone-too-long", "The field Phone must be a string with a maximum length of 50.");
            AssertApplicationError(errors[5], "password-too-long", "The field Password must be a string with a maximum length of 20.");
        }

        private void AssertMultipleEmailErrors(ApiApplicationError error1, ApiApplicationError error2)
        {
            if (error1.message.Contains("maximum length"))
            {
                AssertApplicationError(error1, "email-too-long", "The field Email must be a string with a maximum length of 100.", null);
                AssertApplicationError(error2, "email-invalid", "The Email field is not a valid e-mail address.", null);
            }
            else
            {
                AssertApplicationError(error1, "email-invalid", "The Email field is not a valid e-mail address.", null);
                AssertApplicationError(error2, "email-too-long", "The field Email must be a string with a maximum length of 100.", null);
            }
        }

        private void ThenReturnNoCurrencyError(ApiResponse response)
        {
            AssertSingleError(response, "currency-required", "The Currency field is required.");
        }

        private void ThenReturnsCurrencyNotSupportedError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.CurrencyNotSupported, "Currency 'XX' is not supported.", "XX");
        }

        private void ThenReturnDuplicateAdminError(ApiResponse response, string expextedEmail)
        {
            AssertSingleError(response, ErrorCodes.UserDuplicate, string.Format("The user with email address '{0}' already exists.", expextedEmail), expextedEmail);
        }

        private void ThenReturnForbiddenError(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.Forbidden);
        }

        private void ThenCreateNewBusiness(ApiResponse response, ExpectedBusiness expectedBusiness)
        {
            AssertNewBusinessResponse(response, expectedBusiness);

            AssertBusinessGet(expectedBusiness);
        }

        private void ThenCreateNewBusinessWithoutSport(ApiResponse response, ExpectedBusiness expectedBusiness)
        {
            AssertNewBusinessResponse(response, expectedBusiness);

            AssertBusinessGet(expectedBusiness);
        }


        private void AssertNewBusinessResponse(ApiResponse response, ExpectedBusiness expectedBusiness)
        {
            var registration = AssertSuccessResponse<RegistrationData>(response);

            var business = registration.business;
            Assert.That(business.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(business.name, Is.EqualTo(expectedBusiness.Name));
            Assert.That(business.domain, Is.EqualTo(expectedBusiness.Domain));
            Assert.That(business.sport, Is.EqualTo(expectedBusiness.Sport));
            AssertDateTime(business.authorisedUntil, expectedBusiness.AuthorisedUntil);
            Assert.That(business.subscriptionPlan, Is.EqualTo(expectedBusiness.SubscriptionPlan));
            Assert.That(business.payment.currency, Is.EqualTo(expectedBusiness.Payment.currency));
            Assert.That(business.payment.isOnlinePaymentEnabled, Is.False);
            Assert.That(business.payment.forceOnlinePayment, Is.False);
            Assert.That(business.payment.paymentProvider, Is.Null);
            Assert.That(business.payment.merchantAccountIdentifier, Is.Null);
            Assert.That(business.payment.useProRataPricing, Is.True);

            var admin = registration.admin;
            Assert.That(admin, Is.Not.Null);
            Assert.That(admin.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.firstName, Is.EqualTo(expectedBusiness.Admin.firstName));
            Assert.That(admin.lastName, Is.EqualTo(expectedBusiness.Admin.lastName));
            Assert.That(admin.email, Is.EqualTo(expectedBusiness.Admin.email));
            Assert.That(admin.phone, Is.EqualTo(expectedBusiness.Admin.phone));
            Assert.That(admin.username, Is.EqualTo(expectedBusiness.Admin.email));
            Assert.That(admin.passwordHash, Is.Not.EqualTo(expectedBusiness.Admin.password));
            Assert.That(admin.businessId, Is.EqualTo(business.id));
            Assert.That(admin.businessName, Is.EqualTo(business.name));

            expectedBusiness.Id = business.id;
        }

        public void AssertBusinessData(BusinessData business, ExpectedBusiness expectedBusiness)
        {
            Assert.That(business, Is.Not.Null);
            Assert.That(business.id, Is.EqualTo(expectedBusiness.Id));
            Assert.That(business.name, Is.EqualTo(expectedBusiness.Name));
            Assert.That(business.domain, Is.EqualTo(expectedBusiness.Domain));
            Assert.That(business.sport, Is.EqualTo(expectedBusiness.Sport));
            AssertDateTime(business.authorisedUntil, expectedBusiness.AuthorisedUntil);
            Assert.That(business.payment.currency, Is.EqualTo(expectedBusiness.Payment.currency));
            Assert.That(business.payment.isOnlinePaymentEnabled, Is.False);
            Assert.That(business.payment.forceOnlinePayment, Is.False);
            Assert.That(business.payment.paymentProvider, Is.EqualTo(null));
            Assert.That(business.payment.merchantAccountIdentifier, Is.EqualTo(null));
            Assert.That(business.payment.useProRataPricing, Is.True);
        }

        private void AssertBusinessGet(ExpectedBusiness expectedBusiness)
        {
            var response = AuthenticatedGet<BusinessData>("Business",
                                                          expectedBusiness.UserName,
                                                          expectedBusiness.Password);
            var business = (BusinessData)response.Payload;
            AssertBusinessData(business, expectedBusiness);
        }
    }
}
