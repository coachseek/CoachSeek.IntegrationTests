using System;
using System.Linq;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingAddSingleCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenNonExistentCustomer_WhenTryOnlineBookSingleCourseSession_ThenReturnNonExistentCustomerError()
        {
            var setup = RegisterBusiness();

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnNonExistentCustomerError(response, command.customer.id.GetValueOrDefault());
        }

        [Test]
        public void GivenTheCourseSessionIsFull_WhenTryOnlineBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();

            var command = GivenTheCourseSessionIsFull(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response, command.sessions[0].id.GetValueOrDefault());
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryOnlineBookSingleCourseSession_ThenReturnCustomerAlreadyBookedOntoSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenThisCustomerIsAlreadyBookedOntoThisCourseSession(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCustomerAlreadyBookedOntoSessionError(response, setup);
        }

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryOnlineBookSingleCourseSession_ThenReturnCourseSessionIsNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup, setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Id);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionIsNotOnlineBookableError(response, setup.BobbyRemueraMiniRed9To10For3Weeks.Id);
        }

        [Test]
        public void GivenTheCourseIsOnlineBookable_WhenTryOnlineBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsOnlineBookable(setup, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionOnlineBooking(response, setup);
        }

        [Test]
        public void GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne_WhenTryOnlineBookSingleCourseSession_ThenCreateAnotherCourseBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenCreateAnotherOnlineCourseBooking(response, setup);
        }

        [Test]
        public void GivenCustomerIsBookedOnAnotherCourseSessionButNotThisFullSession_WhenTryOnlineBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIsBookedOnAnotherCourseSessionButNotThisFullSession(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
        }

        [Test]
        public void GivenDeleteBookingAndAddItAgain_WhenTryOnlineBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenDeleteBookingAndAddItAgain(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }



        private ApiResponse WhenTryOnlineBookSingleCourseSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryOnlineBookCourse(json, setup);
        }

        private void ThenCreateSingleCourseSessionOnlineBooking(ApiResponse response, SetupData setup)
        {
            ThenCreateSingleCourseSessionBooking(response, setup, true);
        }

        private void ThenCreateSingleCourseSessionBooking(ApiResponse response, SetupData setup)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            Assert.That(courseBooking.id, Is.InstanceOf<Guid>());
            Assert.That(courseBooking.course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(courseBooking.course.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith starting on {0} at 09:00 for 3 days",
                                                                            GetDateFormatNumberOfDaysOut(14))));
            Assert.That(courseBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(courseBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(1));

            var sessionBooking = courseBooking.sessionBookings[0];
            Assert.That(sessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(sessionBooking.session.id), Is.True);
            Assert.That(sessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(15))));
            Assert.That(sessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(sessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName,  setup.Fred.LastName)));

            Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));

            // Check the bookings on the course
            GetAndAssertCourse(courseBooking.id, sessionBooking.id, setup);
        }

        private void GetAndAssertCourse(Guid courseBookingId, Guid sessionBookingId, SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            var customer = courseBooking.customer;
            Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(customer.phone, Is.EqualTo(setup.Fred.Phone));

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));

            var sessionBooking = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBooking.id, Is.EqualTo(sessionBookingId));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(sessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(sessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(sessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(sessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(sessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));
        }


        protected void ThenCreateAnotherOnlineCourseBooking(ApiResponse response, SetupData setup)
        {
            ThenCreateAnotherCourseBooking(response, setup, true);
        }
    }
}
