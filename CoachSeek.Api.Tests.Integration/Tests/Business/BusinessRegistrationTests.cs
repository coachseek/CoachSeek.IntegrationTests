using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using Newtonsoft.Json;
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
        public void GivenNoBusinessRegistrationCommand_WhenTryRegisterBusiness_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoBusinessRegistrationCommand();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyBusinessRegistrationCommand_WhenTryRegisterBusiness_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyBusinessRegistrationCommand();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenMissingProperties_WhenTryRegisterBusiness_ThenReturnMissingPropertiesErrorResponse()
        {
            var command = GivenMissingProperties();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnMissingPropertiesErrorResponse(response);
        }

        [Test]
        public void GivenMultipleErrorsOnProperties_WhenTryRegisterBusiness_ThenReturnMultipleErrorResponse()
        {
            var business = GivenMultipleErrorsOnProperties();
            var response = WhenTryRegisterBusiness(business);
            ThenReturnMultipleErrorResponse(response);
        }

        [Test]
        public void GivenInvalidCurrency_WhenTryRegisterBusiness_ThenReturnsCurrencyNotSupportedError()
        {
            var business = GivenInvalidCurrency();
            var response = WhenTryRegisterBusiness(business);
            ThenReturnsCurrencyNotSupportedError(response);
        }

        [Test]
        public void GivenDuplicateBusinessAdmin_WhenTryRegisterBusiness_ThenReturnDuplicateAdminErrorResponse()
        {
            GivenDuplicateBusinessAdmin();
            var response = WhenTryRegisterBusiness();
            ThenReturnDuplicateAdminErrorResponse(response);
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
        public void GivenNoCurrency_WhenTryRegisterBusiness_ThenUseNewZealandCurrency()
        {
            var business = GivenNoCurrency();
            var response = WhenTryRegisterBusiness(business);
            ThenUseNewZealandCurrency(response, business);
        }



        //[Test]
        //public void TestWelcomeEmail()
        //{
        //    var command = new ApiBusinessRegistrationCommand
        //    {
        //        business = new ApiBusiness { name = "Test Business" },
        //        admin = new ApiBusinessAdmin
        //        {
        //            firstName = "Olaf",
        //            lastName = "Thielke",
        //            email = "olaft@ihug.co.nz",
        //            password = "password1"
        //        }
        //    };

        //    var response = WhenTryRegisterBusiness(command);
        //    ThenReturnNewBusinessSuccessResponse(response);
        //}



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

            return JsonConvert.SerializeObject(registration);
        }

        private ExpectedBusiness GivenMultipleErrorsOnProperties()
        {
            return new ExpectedBusiness(Random.RandomString, 
                                        "ABCDE",
                                        "Bob",
                                        "Smith",
                                        "abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                                        "abcdefghijklmnopqrstuvwxyz01234567890");
        }

        private ExpectedBusiness GivenInvalidCurrency()
        {
            return new ExpectedBusiness(Random.RandomString, "XX", Random.RandomEmail);
        }

        private void GivenDuplicateBusinessAdmin()
        {
            RegisterFirstTime();
        }

        private ExpectedBusiness GivenUniqueBusinessAdmin()
        {
            return new ExpectedBusiness(Random.RandomString, "USD", Random.RandomEmail);
        }

        private ExpectedBusiness GivenNoCurrency()
        {
            return new ExpectedBusiness(Random.RandomString, "", Random.RandomEmail);
        }


        private void RegisterFirstTime()
        {
            Business = new RandomBusiness();
            BusinessRegistrar.RegisterBusiness(Business);
        }

        private Response WhenTryRegisterBusiness()
        {
            return BusinessRegistrar.RegisterBusiness(Business);
        }

        private Response WhenTryRegisterBusiness(ExpectedBusiness business)
        {
            return BusinessRegistrar.RegisterBusiness(business);
        }

        private Response WhenTryRegisterBusiness(ApiBusinessRegistrationCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryRegisterBusiness(json);
        }

        private Response WhenTryRegisterBusiness(string json)
        {
            return WebClient.AnonymousPost<RegistrationData>(json, RelativePath);
        }

        private Response WhenTryRegisterBusinessUsingHttp(ExpectedBusiness business)
        {
            return BusinessRegistrar.RegisterBusiness(business, "http");
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The business field is required.", "registration.business" },
                                                    { "The admin field is required.", "registration.admin" } });
        }

        private void ThenReturnMissingPropertiesErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The name field is required.", "registration.business.name" },
                                                    { "The firstName field is required.", "registration.admin.firstName" },
                                                    { "The lastName field is required.", "registration.admin.lastName" },
                                                    { "The email field is required.", "registration.admin.email" },
                                                    { "The password field is required.", "registration.admin.password" } });
        }

        private void ThenReturnMultipleErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(4));
            AssertApplicationError(errors[0], "registration.business.currency", "The field currency must be a string with a maximum length of 3.");
            AssertMultipleEmailErrors(errors[1], errors[2]);
            AssertApplicationError(errors[3], "registration.admin.password", "The field password must be a string with a maximum length of 20.");
        }

        private void AssertMultipleEmailErrors(ApplicationError error1, ApplicationError error2)
        {
            if (error1.message.Contains("maximum length"))
            {
                AssertApplicationError(error1, "registration.admin.email", "The field email must be a string with a maximum length of 100.");
                AssertApplicationError(error2, "registration.admin.email", "The email field is not a valid e-mail address.");
            }
            else
            {
                AssertApplicationError(error1, "registration.admin.email", "The email field is not a valid e-mail address.");
                AssertApplicationError(error2, "registration.admin.email", "The field email must be a string with a maximum length of 100.");
            }
        }

        private void ThenReturnsCurrencyNotSupportedError(Response response)
        {
            AssertSingleError(response, "This currency is not supported.", "registration.business.currency");
        }

        private void ThenReturnDuplicateAdminErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "registration.admin.email", "The user with this email address already exists.");
        }

        private void ThenReturnForbiddenError(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.Forbidden);
        }

        private void ThenCreateNewBusiness(Response response, ExpectedBusiness expectedBusiness)
        {
            AssertNewBusinessResponse(response, expectedBusiness);

            AssertBusinessGet(expectedBusiness);
        }

        private void ThenUseNewZealandCurrency(Response response, ExpectedBusiness expectedBusiness)
        {
            expectedBusiness.Currency = "NZD";

            AssertNewBusinessResponse(response, expectedBusiness);

            AssertBusinessGet(expectedBusiness);
        }


        private void AssertNewBusinessResponse(Response response, ExpectedBusiness expectedBusiness)
        {
            var registration = AssertSuccessResponse<RegistrationData>(response);

            var business = registration.business;
            Assert.That(business.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(business.name, Is.EqualTo(expectedBusiness.Name));
            Assert.That(business.domain, Is.EqualTo(expectedBusiness.Domain));
            Assert.That(business.currency, Is.EqualTo(expectedBusiness.Currency));
            Assert.That(business.paymentProvider, Is.Null);
            Assert.That(business.merchantAccountIdentifier, Is.Null);

            var admin = registration.admin;
            Assert.That(admin, Is.Not.Null);
            Assert.That(admin.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.firstName, Is.EqualTo(expectedBusiness.Admin.firstName));
            Assert.That(admin.lastName, Is.EqualTo(expectedBusiness.Admin.lastName));
            Assert.That(admin.email, Is.EqualTo(expectedBusiness.Admin.email));
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
            Assert.That(business.currency, Is.EqualTo(expectedBusiness.Currency));
            Assert.That(business.paymentProvider, Is.EqualTo(null));
            Assert.That(business.merchantAccountIdentifier, Is.EqualTo(null));
        }

        private void AssertBusinessGet(ExpectedBusiness expectedBusiness)
        {
            var getBusinessUrl = string.Format("{0}/Business", BaseUrl);
            Business = expectedBusiness;
            var response = AuthenticatedGet<BusinessData>(getBusinessUrl);
            var business = (BusinessData)response.Payload;

            AssertBusinessData(business, expectedBusiness);
        }
    }
}
