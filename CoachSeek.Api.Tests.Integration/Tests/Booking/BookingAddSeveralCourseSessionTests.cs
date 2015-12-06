using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingAddSeveralCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenDuplicateSessions_WhenTryBookSeveralCourseSessions_ThenReturnDuplicateSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenDuplicateSessions(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnDuplicateSessionError(response, setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
        }

        [Test]
        public void GivenASessionIsStandalone_WhenTryBookSeveralCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionIsStandalone(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiMiniRed14To15.Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response, 
                                              setup.AaronOrakeiMiniRed14To15.Id, 
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenASessionBelongsToADifferentCourse_WhenTryBookSeveralCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionBelongsToADifferentCourse(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response,
                                              setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenACourseSessionIsFull_WhenTryBookSeveralCourseSessions_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            var command = GivenACourseSessionIsFull(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnCourseSessionFullError(response, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
        }

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryBookSeveralCourseSessions_ThenReturnCourseSessionIsNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[2].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateCourseSessionBookingForSeveralSessions(response, setup);
        }

        [Test]
        public void GivenTheCourseIsOnlineBookable_WhenTryBookSeveralCourseSessions_ThenCreateSeveralCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsOnlineBookable(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateSeveralCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenValidSeveralCourseSessionsOutOfOrder_WhenTryBookSeveralCourseSessions_ThenCreateSeveralCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsOnlineBookable(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateSeveralCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenSomeSessionsAlreadyBooked_WhenTryBookSeveralCourseSessions_ThenCreateAnotherCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenSomeSessionsAlreadyBooked(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateAnotherCourseSessionBooking(response, setup);
        }


        private ApiResponse WhenTryBookSeveralCourseSessions(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookCourse(json, setup);
        }


        protected void ThenCreateCourseSessionBookingForSeveralSessions(ApiResponse response, SetupData setup)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.BobbyRemueraMiniRed9To10For3Weeks.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.False);
            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(2));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Assert(firstSessionBooking.session);
            setup.Fred.Assert(firstSessionBooking.customer);
            Assert.That(firstSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.False);
            Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[2].Assert(secondSessionBooking.session);
            setup.Fred.Assert(secondSessionBooking.customer);
            Assert.That(secondSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.False);
            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithSeveralSessionBookings(courseBooking.id,
                                                         firstSessionBooking.id,
                                                         secondSessionBooking.id,
                                                         setup);
        }

        private void GetAndAssertCourseWithSeveralSessionBookings(Guid courseBookingId,
                                                                  Guid firstSessionBookingId,
                                                                  Guid secondSessionBookingId,
                                                                  SetupData setup)
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

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var firstSessionBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.False);
            Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(firstSessionBooking.customer);

            var secondSessionBooking = course.sessions[2].booking.bookings[0];
            Assert.That(secondSessionBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.False);
            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(secondSessionBooking.customer);
        }
    }
}
