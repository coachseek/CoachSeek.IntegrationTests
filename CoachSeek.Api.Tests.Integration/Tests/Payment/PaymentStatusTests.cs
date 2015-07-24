using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Tests.Booking;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Payment
{
    [TestFixture]
    public class PaymentStatusTests : BaseBookingTests
    {
        private const string STATUS_PENDING_INVOICE = "pending-invoice";
        private const string STATUS_PENDING_PAYMENT = "pending-payment";
        private const string STATUS_OVERDUE_PAYMENT = "overdue-payment";
        private const string STATUS_PAID = "paid";


        [Test]
        public void GivenWantToSetToInvalidPaymentStatus_WhenTrySetPaymentStatusForSessionBooking_ThenReturnsInvalidPaymentStatusError()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo("Hello world!");
            var response = WhenTrySetPaymentStatusForSessionBooking(command, setup);
            ThenReturnsInvalidPaymentStatusError(response);
        }

        [Test]
        public void GivenWantToSetToPendingInvoice_WhenTrySetPaymentStatusForSessionBooking_ThenSetsSessionBookingPaymentStatusToPendingInvoice()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PENDING_INVOICE);
            WhenTrySetPaymentStatusForSessionBooking(command, setup);
            ThenSetsSessionBookingPaymentStatusTo(STATUS_PENDING_INVOICE, setup);
        }

        [Test]
        public void GivenWantToSetToPendingPayment_WhenTrySetPaymentStatusForSessionBooking_ThenSetsSessionBookingPaymentStatusToPendingPayment()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PENDING_PAYMENT);
            WhenTrySetPaymentStatusForSessionBooking(command, setup);
            ThenSetsSessionBookingPaymentStatusTo(STATUS_PENDING_PAYMENT, setup);
        }

        [Test]
        public void GivenWantToSetToOverduePayment_WhenTrySetPaymentStatusForSessionBooking_ThenSetsSessionBookingPaymentStatusToOverduePayment()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_OVERDUE_PAYMENT);
            WhenTrySetPaymentStatusForSessionBooking(command, setup);
            ThenSetsSessionBookingPaymentStatusTo(STATUS_OVERDUE_PAYMENT, setup);
        }

        [Test]
        public void GivenWantToSetToPaid_WhenTrySetPaymentStatusForSessionBooking_ThenSetsSessionBookingPaymentStatusToPaid()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PAID);
            WhenTrySetPaymentStatusForSessionBooking(command, setup);
            ThenSetsSessionBookingPaymentStatusTo(STATUS_PAID, setup);
        }


        [Test]
        public void GivenWantToSetToInvalidPaymentStatus_WhenTrySetPaymentStatusForCourseBooking_ThenReturnsInvalidPaymentStatusError()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo("Hello world!");
            var response = WhenTrySetPaymentStatusForCourseBooking(command, setup);
            ThenReturnsInvalidPaymentStatusError(response);
        }

        [Test]
        public void GivenWantToSetToPendingInvoice_WhenTrySetPaymentStatusForCourseBooking_ThenSetsCourseBookingPaymentStatusToPendingInvoice()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PENDING_INVOICE);
            WhenTrySetPaymentStatusForCourseBooking(command, setup);
            ThenSetsCourseBookingPaymentStatusTo(STATUS_PENDING_INVOICE, setup);
        }

        [Test]
        public void GivenWantToSetToPendingPayment_WhenTrySetPaymentStatusForCourseBooking_ThenSetsCourseBookingPaymentStatusToPendingPayment()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PENDING_PAYMENT);
            WhenTrySetPaymentStatusForCourseBooking(command, setup);
            ThenSetsCourseBookingPaymentStatusTo(STATUS_PENDING_PAYMENT, setup);
        }

        [Test]
        public void GivenWantToSetToOverduePayment_WhenTrySetPaymentStatusForCourseBooking_ThenSetsCourseBookingPaymentStatusToOverduePayment()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_OVERDUE_PAYMENT);
            WhenTrySetPaymentStatusForCourseBooking(command, setup);
            ThenSetsCourseBookingPaymentStatusTo(STATUS_OVERDUE_PAYMENT, setup);
        }

        [Test]
        public void GivenWantToSetToPaid_WhenTrySetPaymentStatusForCourseBooking_ThenSetsCourseBookingPaymentStatusToPaid()
        {
            var setup = RegisterBusiness();

            var command = GivenWantToSetTo(STATUS_PAID);
            WhenTrySetPaymentStatusForCourseBooking(command, setup);
            ThenSetsCourseBookingPaymentStatusTo(STATUS_PAID, setup);
        }


        private ApiBookingSetPaymentStatusCommand GivenWantToSetTo(string paymentStatus)
        {
            return new ApiBookingSetPaymentStatusCommand { paymentStatus = paymentStatus };
        }


        private object WhenTrySetPaymentStatusForSessionBooking(ApiBookingSetPaymentStatusCommand command, SetupData setup)
        {
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var json = JsonSerialiser.Serialise(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id);

            return WhenTrySetPaymentStatus(json, relativePath, setup);
        }

        private object WhenTrySetPaymentStatusForCourseBooking(ApiBookingSetPaymentStatusCommand command, SetupData setup)
        {
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var json = JsonSerialiser.Serialise(command);
            var relativePath = string.Format("{0}/{1}", RelativePath, setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id);

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

        private void ThenSetsSessionBookingPaymentStatusTo(string paymentStatus, SetupData setup)
        {
            var response = AuthenticatedGet<SingleSessionBookingData>(RelativePath, 
                                                                      setup.FredOnAaronOrakeiMiniRed14To15.Id,
                                                                      setup);
            var booking = (SingleSessionBookingData)response.Payload;

            Assert.That(booking.paymentStatus, Is.EqualTo(paymentStatus));
        }

        private void ThenSetsCourseBookingPaymentStatusTo(string paymentStatus, SetupData setup)
        {
            var response = AuthenticatedGet<CourseBookingData>(RelativePath,
                                                               setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id,
                                                               setup);
            var booking = (CourseBookingData)response.Payload;

            Assert.That(booking.paymentStatus, Is.EqualTo(paymentStatus));

            Assert.That(booking.sessionBookings[0].paymentStatus, Is.EqualTo(paymentStatus));
            Assert.That(booking.sessionBookings[1].paymentStatus, Is.EqualTo(paymentStatus));
        }
    }
}
