using System.Net;
using System.Web;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Admin
{
    [TestFixture]
    public class AdminTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Admin"; }
        }


        [Test]
        public void GivenEmailAddressIsNotUnsubscribed_WhenTryGetIsEmailUnsubscribed_ThenReturnsNotFound()
        {
            var email = GivenEmailAddressIsNotUnsubscribed();
            var response = WhenTryGetIsEmailUnsubscribed(email);
            AssertStatusCode(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GivenEmailAddressIsUnsubscribed_WhenTryGetIsEmailUnsubscribed_ThenReturnsFound()
        {
            var email = GivenEmailAddressIsUnsubscribed();
            var response = WhenTryGetIsEmailUnsubscribed(email);
            AssertStatusCode(response.StatusCode, HttpStatusCode.Found);
        }


        [Test]
        public void GivenMissingEmailAddress_WhenTryUnsubscribeEmail_ThenReturnsMissingEmailError()
        {
            var email = GivenMissingEmailAddress();
            var response = WhenTryUnsubscribeEmail(email);
            AssertSingleError(response, "email-required", "The Email field is required.", null);
        }

        [Test]
        public void GivenIsNotValidEmailAddress_WhenTryUnsubscribeEmail_ThenReturnsInvalidEmailFormatError()
        {
            var email = GivenIsNotValidEmailAddress();
            var response = WhenTryUnsubscribeEmail(email);
            AssertSingleError(response, "email-invalid", "The Email field is not a valid e-mail address.", null);
        }

        [Test]
        public void GivenEmailAddressIsNotUnsubscribed_WhenTryUnsubscribeEmail_ThenUnsubscribesEmail()
        {
            var email = GivenEmailAddressIsNotUnsubscribed();
            WhenTryUnsubscribeEmail(email);
            ThenUnsubscribesEmail(email);
        }

        [Test]
        public void GivenEmailAddressIsUnsubscribed_WhenTryUnsubscribeEmail_ThenEmailIsStillUnsubscribed()
        {
            var email = GivenEmailAddressIsNotUnsubscribed();
            WhenTryUnsubscribeEmail(email);
            ThenEmailIsStillUnsubscribed(email);
        }

        [Test]
        public void GivenUserDoesNotExist_WhenTryGetUser_ThenReturnsNotFound()
        {
            var email = GivenUserDoesNotExist();
            var response = WhenTryGetUser(email);
            AssertStatusCode(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GivenUserExists_WhenTryGetUser_ThenReturnsUser()
        {
            var setup = RegisterBusiness();

            var username = GivenUserExists(setup);
            var response = WhenTryGetUser(username);
            ThenReturnsUser(response, setup);
        }


        private string GivenMissingEmailAddress()
        {
            return string.Empty;
        }

        private string GivenIsNotValidEmailAddress()
        {
            return "xyz999";
        }

        private string GivenEmailAddressIsNotUnsubscribed()
        {
            return Random.RandomEmail;
        }

        private string GivenEmailAddressIsUnsubscribed()
        {
            var email = Random.RandomEmail;
            UnsubscribeEmail(email);
            return email;
        }

        private string GivenUserDoesNotExist()
        {
            return Random.RandomEmail;
        }

        private string GivenUserExists(SetupData setup)
        {
            return setup.Business.UserName;
        }


        private ApiResponse WhenTryGetIsEmailUnsubscribed(string emailAddress)
        {
            return GetIsEmailUnsubscribed(emailAddress);
        }

        private ApiResponse GetIsEmailUnsubscribed(string emailAddress)
        {
            var url = string.Format("Email/IsUnsubscribed?email={0}", HttpUtility.UrlEncode(emailAddress));
            return AdminGet<bool>(url);
        }

        private ApiResponse WhenTryUnsubscribeEmail(string emailAddress)
        {
            return UnsubscribeEmail(emailAddress);
        }

        private ApiResponse UnsubscribeEmail(string emailAddress)
        {
            var url = string.Format("Email/Unsubscribe?email={0}", HttpUtility.UrlEncode(emailAddress));
            return AdminGet<string>(url);
        }

        private ApiResponse WhenTryGetUser(string emailAddress)
        {
            var url = string.Format("Users/{0}", HttpUtility.UrlEncode(emailAddress));
            return AdminGet<UserData>(url);
        }


        private void ThenUnsubscribesEmail(string emailAddress)
        {
            var response = GetIsEmailUnsubscribed(emailAddress);
            AssertStatusCode(response.StatusCode, HttpStatusCode.Found);
        }

        private void ThenEmailIsStillUnsubscribed(string emailAddress)
        {
            var response = GetIsEmailUnsubscribed(emailAddress);
            AssertStatusCode(response.StatusCode, HttpStatusCode.Found);
        }

        private void ThenReturnsUser(ApiResponse response, SetupData setup)
        {
            var user = AssertSuccessResponse<UserData>(response);

            Assert.That(user.businessId, Is.EqualTo(setup.Business.Id));
            Assert.That(user.businessName, Is.EqualTo(setup.Business.Name));
            Assert.That(user.id, Is.EqualTo(setup.Business.Admin.id));
            Assert.That(user.firstName, Is.EqualTo(setup.Business.Admin.firstName));
            Assert.That(user.lastName, Is.EqualTo(setup.Business.Admin.lastName));
            Assert.That(user.email, Is.EqualTo(setup.Business.Admin.email));
            Assert.That(user.username, Is.EqualTo(setup.Business.Admin.email));
            Assert.That(user.phone, Is.EqualTo(setup.Business.Admin.phone));
        }
    }
}
