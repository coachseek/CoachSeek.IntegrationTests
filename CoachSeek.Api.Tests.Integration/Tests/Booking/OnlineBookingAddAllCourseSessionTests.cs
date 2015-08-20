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
    public class OnlineBookingAddAllCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenDuplicateSessions_WhenTryOnlineBookAllCourseSessions_ThenReturnDuplicateSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenDuplicateSessions(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnDuplicateSessionError(response);
        }

        [Test]
        public void GivenASessionIsStandalone_WhenTryOnlineBookAllCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionIsStandalone(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response, 
                                              setup.AaronOrakeiMiniRed14To15.Id,
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenASessionBelongsToADifferentCourse_WhenTryOnlineBookAllCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionBelongsToADifferentCourse(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response,
                                              setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenACourseSessionIsFull_WhenTryOnlineBookAllCourseSessions_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            var command = GivenACourseSessionIsFull(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnCourseSessionFullError(response, command.sessions[2].id.GetValueOrDefault());
        }

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryOnlineBookAllCourseSessions_ThenReturnCourseSessionIsNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnCourseSessionIsNotOnlineBookableError(response, setup.BobbyRemueraMiniRed9To10For3Weeks.Id);
        }

        [Test]
        public void GivenValidAllCourseSessions_WhenTryOnlineBookAllCourseSessions_ThenCreateAllCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidAllCourseSessions(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenCreateAllCourseSessionBookings(response, setup);
        }

        [Test]
        public void GivenValidAllCourseSessionsOutOfOrder_WhenTryOnlineBookAllCourseSessions_ThenCreateAllCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidAllCourseSessionsOutOfOrder(setup);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenCreateAllCourseSessionBookings(response, setup);
        }


        protected ApiBookingSaveCommand GivenDuplicateSessions(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id
            }, 
            setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenASessionIsStandalone(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiMiniRed14To15.Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenASessionBelongsToADifferentCourse(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenACourseSessionIsFull(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.BamBam.Id);
        }

        private ApiBookingSaveCommand GivenTheCourseIsNotOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[2].Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenValidAllCourseSessions(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenValidAllCourseSessionsOutOfOrder(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id
            },
            setup.Fred.Id);
        }


        private ApiResponse WhenTryOnlineBookAllCourseSessions(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryOnlineBookCourse(json, setup);
        }


        protected void ThenReturnDuplicateSessionError(ApiResponse response)
        {
            AssertSingleError(response,
                              ErrorCodes.BookingContainsDuplicateSessions,
                              "Booking contains duplicate sessions.");
        }

        private void ThenReturnSessionNotInCourseError(ApiResponse response, Guid sessionId, Guid courseId)
        {
            AssertSingleError(response,
                              ErrorCodes.SessionNotInCourse,
                              "Session is not in course.",
                              string.Format("Session: '{0}', Course: '{1}'", sessionId, courseId));
        }

        private void ThenReturnCourseSessionFullError(ApiResponse response, Guid sessionId)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionFullyBooked, 
                              "Session is already fully booked.",
                              sessionId.ToString());
        }

        private void ThenReturnCourseSessionIsNotOnlineBookableError(ApiResponse response, Guid courseId)
        {
            AssertSingleError(response, 
                              ErrorCodes.CourseNotOnlineBookable, 
                              "The course is not online bookable.",
                              courseId.ToString());
        }

        private void ThenCreateAllCourseSessionBookings(ApiResponse response, SetupData setup)
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
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(3));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(firstSessionBooking.session.id), Is.True);
            Assert.That(firstSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(14))));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(firstSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(secondSessionBooking.session.id), Is.True);
            Assert.That(secondSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(15))));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(secondSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));

            var thirdSessionBooking = courseBooking.sessionBookings[2];
            Assert.That(thirdSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(secondSessionBooking.session.id), Is.True);
            Assert.That(thirdSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(16))));
            Assert.That(thirdSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(thirdSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(thirdSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));

            // Check the bookings on the course
            GetAndAssertCourse(courseBooking.id, 
                               firstSessionBooking.id,
                               secondSessionBooking.id,
                               thirdSessionBooking.id,
                               setup);
        }

        private void GetAndAssertCourse(Guid courseBookingId, 
                                        Guid firstSessionBookingId,
                                        Guid secondSessionBookingId,
                                        Guid thirdSessionBookingId,
                                        SetupData setup)
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

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var firstSessionBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(firstSessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(firstSessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(firstSessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(firstSessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));

            var secondSessionBooking = course.sessions[1].booking.bookings[0];
            Assert.That(secondSessionBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(secondSessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(secondSessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(secondSessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(secondSessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));

            var thirdSessionBooking = course.sessions[2].booking.bookings[0];
            Assert.That(thirdSessionBooking.id, Is.EqualTo(thirdSessionBookingId));
            Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(thirdSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(thirdSessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(thirdSessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(thirdSessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(thirdSessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));
        }

        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, Guid.NewGuid());
        }
    }
}
