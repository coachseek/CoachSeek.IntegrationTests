using System.Net;
using System.Web;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
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
            AssertSingleError(response, "Missing email address.");
        }

        [Test]
        public void GivenIsNotValidEmailAddress_WhenTryUnsubscribeEmail_ThenReturnsInvalidEmailFormatError()
        {
            var email = GivenIsNotValidEmailAddress();
            var response = WhenTryUnsubscribeEmail(email);
            AssertSingleError(response, "Invalid email address format.");
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
    }
}
