using System;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingDeleteTests : BaseBookingTests
    {
        [Test]
        public void GivenNonExistentBookingId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentBookingId();
            var response = WhenTryDelete(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenExistingStandaloneSessionBookingId_WhenTryDelete_ThenStandaloneSessionBookingIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenExistingStandaloneSessionBookingId(setup);
            var response = WhenTryDelete(id, setup);
            ThenStandaloneSessionBookingIsDeleted(response, setup);
        }

        [Test]
        public void GivenExistingCourseBookingId_WhenTryDelete_ThenCourseBookingIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenExistingCourseBookingId(setup);
            var response = WhenTryDelete(id, setup);
            ThenCourseBookingIsDeleted(response, setup);
        }

        [Test]
        public void GivenExistingCourseSessionBookingId_WhenTryDelete_ThenCourseSessionBookingIsDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenExistingCourseSessionBookingId(setup);
            var response = WhenTryDelete(id, setup);
            ThenCourseSessionBookingIsDeleted(response, setup);
        }

        [Test]
        public void GivenSingleCourseSessionBooking_WhenTryDelete_ThenSessionBookingAndAssociatedCourseBookingAreDeleted()
        {
            var setup = RegisterBusiness();

            var id = GivenSingleCourseSessionBooking(setup);
            var response = WhenTryDelete(id, setup);
            ThenSessionBookingAndAssociatedCourseBookingAreDeleted(response, setup);
        }


        private Guid GivenNonExistentBookingId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingStandaloneSessionBookingId(SetupData setup)
        {
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            return setup.FredOnAaronOrakeiMiniRed14To15.Id;
        }

        private Guid GivenExistingCourseBookingId(SetupData setup)
        {
            RegisterFredOnAllCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id;
        }

        private Guid GivenExistingCourseSessionBookingId(SetupData setup)
        {
            RegisterFredOnAllCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id;
        }

        private Guid GivenSingleCourseSessionBooking(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id;
        }


        private ApiResponse WhenTryDelete(Guid id, SetupData setup)
        {
            return Delete<BookingData>("Bookings", id, setup);
        }


        private void ThenStandaloneSessionBookingIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>(RelativePath, setup.FredOnAaronOrakeiMiniRed14To15.Id, setup);
            AssertNotFound(getResponse);
        }

        private void ThenCourseBookingIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>(RelativePath, setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            AssertNotFound(getResponse);
        }

        private void ThenCourseSessionBookingIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>(RelativePath, setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            AssertNotFound(getResponse);

            GetAndAssertCourseHasLostSessionBooking(setup);
        }

        private void ThenSessionBookingAndAssociatedCourseBookingAreDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>(RelativePath, setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            AssertNotFound(getResponse);

            GetAndAssertCourseAndSessionBookingWereDeleted(setup);
        }

        private void GetAndAssertCourseHasLostSessionBooking(SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(courseBooking.parentId, Is.Null);
            var customer = courseBooking.customer;
            Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(customer.phone, Is.EqualTo(setup.Fred.Phone));

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));
        }

        private void GetAndAssertCourseAndSessionBookingWereDeleted(SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));
        }
    }
}
