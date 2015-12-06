using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingAddSingleCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenNonExistentCustomer_WhenTryBookSingleCourseSession_ThenReturnNonExistentCustomerError()
        {
            var setup = RegisterBusiness();

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnNonExistentCustomerError(response, command.customer.id.GetValueOrDefault());
        }

        [Test]
        public void GivenTheCourseSessionIsFull_WhenTryBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();

            var command = GivenTheCourseSessionIsFull(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response, command.sessions[0].id.GetValueOrDefault());
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryBookSingleCourseSession_ThenReturnCustomerAlreadyBookedOntoSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenThisCustomerIsAlreadyBookedOntoThisCourseSession(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnCustomerAlreadyBookedOntoSessionError(response, setup);
        }

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup, setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Id);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenTheCourseIsOnlineBookable_WhenTryBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsOnlineBookable(setup, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne_WhenTryBookSingleCourseSession_ThenCreateAnotherCourseBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateAnotherCourseBooking(response, setup);
        }

        [Test]
        public void GivenCustomerIsBookedOnAnotherCourseSessionButNotThisFullSession_WhenTryBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIsBookedOnAnotherCourseSessionButNotThisFullSession(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
        }

        [Test]
        public void GivenDeleteBookingAndAddItAgain_WhenTryBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenDeleteBookingAndAddItAgain(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }


        private ApiResponse WhenTryBookSingleCourseSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookCourse(json, setup);
        }

        protected void ThenCreateCourseSessionBooking(ApiResponse response, SetupData setup)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.BobbyRemueraMiniRed9To10For3Weeks.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.False);
            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(1));

            var sessionBooking = courseBooking.sessionBookings[0];

            setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Assert(sessionBooking.session);
            setup.Fred.Assert(sessionBooking.customer);

            Assert.That(sessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBooking.hasAttended, Is.Null);
            Assert.That(sessionBooking.isOnlineBooking, Is.False);
            Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithSessionBooking(courseBooking.id, sessionBooking.id, setup);
        }

        private void GetAndAssertCourseWithSessionBooking(Guid courseBookingId, Guid sessionBookingId, SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.BobbyRemueraMiniRed9To10For3Weeks.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            Assert.That(courseBooking.isOnlineBooking, Is.False);
            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));

            var sessionBooking = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBooking.id, Is.EqualTo(sessionBookingId));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(sessionBooking.hasAttended, Is.Null);
            Assert.That(sessionBooking.isOnlineBooking, Is.False);
            Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBooking.customer);
        }
    }
}
