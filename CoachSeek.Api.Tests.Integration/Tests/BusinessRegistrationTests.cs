using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class BusinessRegistrationTests : WebIntegrationTest
    {
        private string BusinessName { get; set; }
        private string Domain { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }

        protected override string RelativePath
        {
            get { return "BusinessRegistration"; }
        }


        [SetUp]
        public void Setup()
        {
            BusinessName = Random.RandomString;
            Domain = BusinessName;
            FirstName = "Isaac";
            LastName = "Newton";
            Email = Random.RandomEmail;
            Password = "password1";
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
            var command = GivenMultipleErrorsOnProperties();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnMultipleErrorResponse(response);
        }

        [Test]
        public void GivenDuplicateBusinessAdmin_WhenTryRegisterBusiness_ThenReturnDuplicateAdminErrorResponse()
        {
            GivenDuplicateBusinessAdmin();
            var response = WhenTryRegisterBusiness();
            ThenReturnDuplicateAdminErrorResponse(response);
        }

        [Test]
        public void GivenUniqueBusinessAdmin_WhenTryRegisterBusiness_ThenReturnNewBusinessSuccessResponse()
        {
            var command = GivenUniqueBusinessAdmin();
            var response = WhenTryRegisterBusiness(command);
            ThenReturnNewBusinessSuccessResponse(response);
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

            return JsonConvert.SerializeObject(registration);
        }

        private string GivenMultipleErrorsOnProperties()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = BusinessName },
                admin = new ApiBusinessAdmin
                {
                    firstName = FirstName,
                    lastName = LastName,
                    email = "abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-abcdefghijklmnopqrstuvwxyz01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    password = "abcdefghijklmnopqrstuvwxyz01234567890"
                }
            };

            return JsonConvert.SerializeObject(registration);
        }

        private void GivenDuplicateBusinessAdmin()
        {
            RegisterFirstTime();
        }

        private string GivenUniqueBusinessAdmin()
        {
            return CreateValidBusinessRegistrationCommand();
        }

        private void RegisterFirstTime()
        {
            Business = new RandomBusiness();
            BusinessRegistrar.RegisterBusiness(Business);
        }

        private string CreateValidBusinessRegistrationCommand()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = BusinessName },
                admin = new ApiBusinessAdmin
                {
                    firstName = FirstName,
                    lastName = LastName,
                    email = Email,
                    password = Password
                }
            };

            return JsonConvert.SerializeObject(registration);
        }

        private Response WhenTryRegisterBusiness()
        {
            return BusinessRegistrar.RegisterBusiness(Business);
        }

        private Response WhenTryRegisterBusiness(string json)
        {
            return WebClient.AnonymousPost<RegistrationData>(json, RelativePath);
        }

        private Response WhenPost(string json)
        {
            return PostAnonymously<RegistrationData>(json);
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
            Assert.That(errors.GetLength(0), Is.EqualTo(3));
            AssertMultipleEmailErrors(errors[0], errors[1]);
            AssertApplicationError(errors[2], "registration.admin.password", "The field password must be a string with a maximum length of 20.");
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

        private void ThenReturnDuplicateAdminErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "registration.admin.email", "The user with this email address already exists.");
        }

        private void ThenReturnNewBusinessSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<RegistrationData>());
            var registration = (RegistrationData)response.Payload;
            var business = registration.business;
            Assert.That(business.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(business.name, Is.EqualTo(BusinessName));
            Assert.That(business.domain, Is.EqualTo(Domain));
            var admin = registration.admin;
            Assert.That(admin, Is.Not.Null);
            Assert.That(admin.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.firstName, Is.EqualTo(FirstName));
            Assert.That(admin.lastName, Is.EqualTo(LastName));
            Assert.That(admin.email, Is.EqualTo(Email));
            Assert.That(admin.username, Is.EqualTo(Email));
            Assert.That(admin.passwordHash, Is.Not.EqualTo(Password));
            Assert.That(admin.businessId, Is.EqualTo(business.id));
            Assert.That(admin.businessName, Is.EqualTo(business.name));
        }
    }
}
