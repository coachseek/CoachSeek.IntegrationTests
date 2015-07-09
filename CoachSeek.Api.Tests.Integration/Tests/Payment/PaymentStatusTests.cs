using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Tests.Booking;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Payment
{
    [TestFixture]
    public class PaymentStatusTests : BaseBookingTests
    {
        private const string STATUS_PENDING_INVOICE = "pending-invoice";
        private const string STATUS_PENDING_PAYMENT = "pending-payment";
        private const string STATUS_PAID = "paid";


        [Test]
        public void GivenWantToSetToInvalidPaymentStatus_WhenTrySetPaymentStatus_ThenReturnsInvalidPaymentStatusError()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetTo("Hello world!");
            var response = WhenTrySetPaymentStatus(command, setup);
            ThenReturnsInvalidPaymentStatusError(response);
        }

        [Test]
        public void GivenWantToSetToPendingInvoice_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPendingInvoice()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetTo(STATUS_PENDING_INVOICE);
            WhenTrySetPaymentStatus(command, setup);
            ThenSetsPaymentStatusTo(STATUS_PENDING_INVOICE, setup);
        }

        [Test]
        public void GivenWantToSetToPendingPayment_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPendingPayment()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetTo(STATUS_PENDING_PAYMENT);
            WhenTrySetPaymentStatus(command, setup);
            ThenSetsPaymentStatusTo(STATUS_PENDING_PAYMENT, setup);
        }

        [Test]
        public void GivenWantToSetToPaid_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPaid()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenWantToSetTo(STATUS_PAID);
            WhenTrySetPaymentStatus(command, setup);
            ThenSetsPaymentStatusTo(STATUS_PAID, setup);
        }


        private ApiBookingSetPaymentStatusCommand GivenWantToSetTo(string paymentStatus)
        {
            return new ApiBookingSetPaymentStatusCommand { paymentStatus = paymentStatus };
        }


        private object WhenTrySetPaymentStatus(ApiBookingSetPaymentStatusCommand command, SetupData setup)
        {
            var json = JsonConvert.SerializeObject(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);

            return WhenTrySetPaymentStatus(json, relativePath, setup);
        }

        protected object WhenTrySetPaymentStatus(string json, string relativePath, SetupData setup)
        {
            try
            {
                return new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json,
                                                                                       setup.Business.UserName,
                                                                                       setup.Business.Password,
                                                                                       relativePath);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenReturnsInvalidPaymentStatusError(object response)
        {
            AssertSingleError((ApiResponse)response, "This payment status does not exist.");
        }

        private void ThenSetsPaymentStatusTo(string paymentStatus, SetupData setup)
        {
            var response = AuthenticatedGet<SingleSessionBookingData>(RelativePath, 
                                                                      setup.FredOnAaronOrakeiMiniRed14To15.Id,
                                                                      setup);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.paymentStatus, Is.EqualTo(paymentStatus));
        }
    }

}
