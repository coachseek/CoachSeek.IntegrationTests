using System;
using System.Linq;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
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
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnNonExistentCustomerError(response);
        }

        [Test]
        public void GivenTheCourseSessionIsFull_WhenTryOnlineBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            var command = GivenTheCourseSessionIsFull(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response);
        }

        // TODO: Customer already booked

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryOnlineBookSingleCourseSession_ThenReturnCourseSessionIsNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionIsNotOnlineBookableError(response);
        }

        [Test]
        public void GivenValidSingleCourseSession_WhenTryOnlineBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidSingleCourseSession(setup);
            var response = WhenTryOnlineBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }


        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, Guid.NewGuid());
        }

        private ApiBookingSaveCommand GivenTheCourseSessionIsFull(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.BamBam.Id);
        }

        private ApiBookingSaveCommand GivenTheCourseIsNotOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenValidSingleCourseSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id);
        }


        private ApiResponse WhenTryOnlineBookSingleCourseSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryOnlineBookCourse(json, setup);
        }


        private void ThenReturnCourseSessionFullError(ApiResponse response)
        {
            AssertSingleError(response, "One or more of the sessions is already fully booked.");
        }

        private void ThenReturnCourseSessionIsNotOnlineBookableError(ApiResponse response)
        {
            AssertSingleError(response, "The course is not online bookable.");
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
    }
}
