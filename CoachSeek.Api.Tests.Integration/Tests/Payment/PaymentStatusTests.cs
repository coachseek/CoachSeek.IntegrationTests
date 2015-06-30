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


        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenWantToSetToInvalidPaymentStatus_WhenTrySetPaymentStatus_ThenReturnsInvalidPaymentStatusError()
        {
            var command = GivenWantToSetTo("Hello world!");
            var response = WhenTrySetPaymentStatus(command);
            ThenReturnsInvalidPaymentStatusError(response);
        }

        [Test]
        public void GivenWantToSetToPendingInvoice_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPendingInvoice()
        {
            var command = GivenWantToSetTo(STATUS_PENDING_INVOICE);
            WhenTrySetPaymentStatus(command);
            ThenSetsPaymentStatusTo(STATUS_PENDING_INVOICE);
        }

        [Test]
        public void GivenWantToSetToPendingPayment_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPendingPayment()
        {
            var command = GivenWantToSetTo(STATUS_PENDING_PAYMENT);
            WhenTrySetPaymentStatus(command);
            ThenSetsPaymentStatusTo(STATUS_PENDING_PAYMENT);
        }

        [Test]
        public void GivenWantToSetToPaid_WhenTrySetPaymentStatus_ThenSetsPaymentStatusToPaid()
        {
            var command = GivenWantToSetTo(STATUS_PAID);
            WhenTrySetPaymentStatus(command);
            ThenSetsPaymentStatusTo(STATUS_PAID);
        }


        private ApiBookingSetPaymentStatusCommand GivenWantToSetTo(string paymentStatus)
        {
            return new ApiBookingSetPaymentStatusCommand { paymentStatus = paymentStatus };
        }


        private object WhenTrySetPaymentStatus(ApiBookingSetPaymentStatusCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, FredOnAaronOrakei14To15SessionId);

            return WhenTrySetPaymentStatus(json, relativePath);
        }

        protected object WhenTrySetPaymentStatus(string json, string relativePath)
        {
            try
            {
                return new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json,
                                                                                       Business.UserName,
                                                                                       Business.Password,
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

        private void ThenSetsPaymentStatusTo(string paymentStatus)
        {
            var response = AuthenticatedGet<SingleSessionBookingData>("Bookings", FredOnAaronOrakei14To15SessionId);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.paymentStatus, Is.EqualTo(paymentStatus));
        }
    }

}
