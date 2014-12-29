﻿using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
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
            BusinessName = RandomString;
            Domain = BusinessName;
            FirstName = "Isaac";
            LastName = "Newton";
            Email = RandomEmail;
            Password = "password1";
        }


        [Test]
        public void GivenNoBusinessRegistrationCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoBusinessRegistrationCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyBusinessRegistrationCommand_WhenPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyBusinessRegistrationCommand();
            var response = WhenPost(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenMissingProperties_WhenPost_ThenReturnMissingPropertiesErrorResponse()
        {
            var command = GivenMissingProperties();
            var response = WhenPost(command);
            ThenReturnMissingPropertiesErrorResponse(response);
        }

        [Test]
        public void GivenMultipleErrorsOnProperties_WhenPost_ThenReturnMultipleErrorResponse()
        {
            var command = GivenMultipleErrorsOnProperties();
            var response = WhenPost(command);
            ThenReturnMultipleErrorResponse(response);
        }

        [Test]
        public void GivenDuplicateBusinessAdmin_WhenPost_ThenReturnDuplicateAdminErrorResponse()
        {
            var command = GivenDuplicateBusinessAdmin();
            var response = WhenPost(command);
            ThenReturnDuplicateAdminErrorResponse(response);
        }

        [Test]
        public void GivenUniqueBusinessAdmin_WhenPost_ThenReturnNewBusinessSuccessResponse()
        {
            var command = GivenUniqueBusinessAdmin();
            var response = WhenPost(command);
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

        private string GivenDuplicateBusinessAdmin()
        {
            return RegisterFirstTime();
        }

        private string GivenUniqueBusinessAdmin()
        {
            return CreateValidBusinessRegistrationCommand();
        }

        private string RegisterFirstTime()
        {
            var registration = CreateValidBusinessRegistrationCommand();
            var response = WhenPost(registration);

            return registration;
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


        private Response WhenPost(string json)
        {
            return PostAnonymously<RegistrationData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], null, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(2));
            AssertApplicationError(errors[0], "registration.business", "The business field is required.");
            AssertApplicationError(errors[1], "registration.admin", "The admin field is required.");
        }

        private void ThenReturnMissingPropertiesErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(5));
            AssertApplicationError(errors[0], "registration.business.name", "The name field is required.");
            AssertApplicationError(errors[1], "registration.admin.firstName", "The firstName field is required.");
            AssertApplicationError(errors[2], "registration.admin.lastName", "The lastName field is required.");
            AssertApplicationError(errors[3], "registration.admin.email", "The email field is required.");
            AssertApplicationError(errors[4], "registration.admin.password", "The password field is required.");
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
