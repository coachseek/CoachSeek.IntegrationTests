using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration
{
    [TestFixture]
    public class BusinessRegistrationTests : WebIntegrationTest
    {
        private string BusinessName { get; set; }
        private string Domain { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

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
        public void GivenMissingRegistrantProperties_WhenPost_ThenReturnRegistrantPropertyErrorResponse()
        {
            var command = GivenMissingRegistrantProperties();
            var response = WhenPost(command);
            ThenReturnRegistrantPropertyErrorResponse(response);
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

        private string GivenMissingRegistrantProperties()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                businessName = BusinessName,
                registrant = new ApiBusinessRegistrant()
            };

            return JsonConvert.SerializeObject(registration);
        }

        private string GivenMultipleErrorsOnProperties()
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                businessName = BusinessName,
                registrant = new ApiBusinessRegistrant
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
                businessName = BusinessName,
                registrant = new ApiBusinessRegistrant
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
            return Post<BusinessData>(json);
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
            AssertApplicationError(errors[0], "registration.businessName", "The businessName field is required.");
            AssertApplicationError(errors[1], "registration.registrant", "The registrant field is required.");
        }

        private void ThenReturnRegistrantPropertyErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(4));
            AssertApplicationError(errors[0], "registration.registrant.firstName", "The firstName field is required.");
            AssertApplicationError(errors[1], "registration.registrant.lastName", "The lastName field is required.");
            AssertApplicationError(errors[2], "registration.registrant.email", "The email field is required.");
            AssertApplicationError(errors[3], "registration.registrant.password", "The password field is required.");
        }

        private void ThenReturnMultipleErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(3));
            AssertMultipleEmailErrors(errors[0], errors[1]);
            AssertApplicationError(errors[2], "registration.registrant.password", "The field password must be a string with a maximum length of 20.");
        }

        private void AssertMultipleEmailErrors(ApplicationError error1, ApplicationError error2)
        {
            if (error1.message.Contains("maximum length"))
            {
                AssertApplicationError(error1, "registration.registrant.email", "The field email must be a string with a maximum length of 100.");
                AssertApplicationError(error2, "registration.registrant.email", "The email field is not a valid e-mail address.");
            }
            else
            {
                AssertApplicationError(error1, "registration.registrant.email", "The email field is not a valid e-mail address.");
                AssertApplicationError(error2, "registration.registrant.email", "The field email must be a string with a maximum length of 100.");
            }
        }

        private void ThenReturnDuplicateAdminErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "registration.registrant.email", "This email address is already in use.");
        }

        private void ThenReturnNewBusinessSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<BusinessData>());
            var business = (BusinessData)response.Payload;
            Assert.That(business.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(business.name, Is.EqualTo(BusinessName));
            Assert.That(business.domain, Is.EqualTo(Domain));
            var admin = business.admin;
            Assert.That(admin, Is.Not.Null);
            Assert.That(admin.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.firstName, Is.EqualTo(FirstName));
            Assert.That(admin.lastName, Is.EqualTo(LastName));
            Assert.That(admin.email, Is.EqualTo(Email));
            Assert.That(admin.username, Is.EqualTo(Email));
            Assert.That(admin.passwordHash, Is.EqualTo(Password)); // TODO
            Assert.That(admin.passwordSalt, Is.EqualTo(string.Empty)); // TODO
        }



        public class BusinessResponse
        {
            public BusinessData data { get; set; }
            public List<ApplicationError> errors { get; set; }
            public bool isSuccessful { get; set; } }
    }

        public class BusinessData
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string domain { get; set; }
            public BusinessAdminData admin { get; set; }
            public IList<LocationData> locations { get; set; }
            public IList<CoachData> coaches { get; set; }

        }

        public class BusinessAdminData
        {
            public Guid id { get; set; }

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string username { get; set; }
            public string passwordHash { get; set; }
            public string passwordSalt { get; set; }
        }

        public class LocationData
        {
            public Guid id { get; set; }
            public string name { get; set; }
        }

        public class CoachData
        {
            public Guid id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            //public WeeklyWorkingHoursData WorkingHours { get; set; }
        }

        public class WebError
        {
            public string message { get; set; }
        }
    }

