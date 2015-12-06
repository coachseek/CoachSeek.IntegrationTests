using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddSessionTests : BaseBookingAddTests
    {
        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, Guid.NewGuid());
        }

        protected ApiBookingSaveCommand GivenTheCourseSessionIsFull(SetupData setup)
        {
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.BamBam.Id);
        }

        protected ApiBookingSaveCommand GivenThisCustomerIsAlreadyBookedOntoThisCourseSession(SetupData setup)
        {
            RegisterFredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenTheCourseIsOnlineBookable(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenTheCourseIsNotOnlineBookable(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenCustomerIsBookedOnAnotherCourseSessionButNotThisFullSession(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup, 1);
            RegisterWilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenNonExistentSessionAndCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(Guid.NewGuid(), Guid.NewGuid());
        }

        protected ApiBookingSaveCommand GivenThisCustomerIsAlreadyBookedOntoThisStandaloneSession(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSessionIsOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSessionIsNotOnlineBookable(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed16To17.Id, setup.Wilma.Id);
        }

        protected ApiBookingSaveCommand GivenDeleteBookingAndAddItAgain(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            AuthenticatedDelete("Bookings", setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id.ToString(), setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenDuplicateSessions(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenASessionIsStandalone(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenASessionBelongsToADifferentCourse(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenACourseSessionIsFull(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.BamBam.Id);
        }

        protected ApiBookingSaveCommand GivenValidAllCourseSessionsOutOfOrder(SetupData setup, params Guid[] sessionIds)
        {
            return new ApiBookingSaveCommand(sessionIds, setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSomeSessionsAlreadyBooked(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.Fred.Id);
        }


        protected void ThenReturnNonExistentSessionError(ApiResponse response, Guid sessionId)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionInvalid, 
                              "This session does not exist.",
                              sessionId.ToString());
        }

        protected void ThenReturnNonExistentCustomerError(ApiResponse response, Guid customerId)
        {
            AssertSingleError(response, 
                              ErrorCodes.CustomerInvalid, 
                              "This customer does not exist.",
                              customerId.ToString());
        }

        protected void ThenReturnCourseSessionFullError(ApiResponse response, Guid sessionId)
        {
            AssertSingleError(response,
                              ErrorCodes.SessionFullyBooked,
                              "Session is already fully booked.",
                              sessionId.ToString());
        }

        protected void ThenReturnCourseSessionIsNotOnlineBookableError(ApiResponse response, Guid courseId)
        {
            AssertSingleError(response,
                              ErrorCodes.CourseNotOnlineBookable,
                              "The course is not online bookable.",
                              courseId.ToString());
        }

        protected void ThenReturnCustomerAlreadyBookedOntoSessionError(ApiResponse response, SetupData setup)
        {
            AssertSingleError(response,
                              ErrorCodes.CustomerAlreadyBookedOntoSession,
                              "This customer is already booked for this session.",
                              string.Format("Customer: '{0}', Session: '{1}'", setup.Fred.Id, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id));
        }

        protected void ThenReturnDuplicateStandaloneSessionBookingError(ApiResponse response, Guid customerId, Guid sessionId)
        {
            AssertSingleError(response,
                              ErrorCodes.CustomerAlreadyBookedOntoSession,
                              "This customer is already booked for this session.",
                              string.Format("Customer: '{0}', Session: '{1}'", customerId, sessionId));
        }

        protected void ThenReturnDuplicateCourseSessionBookingError(ApiResponse response, Guid customerId, Guid sessionId)
        {
            AssertSingleError(response,
                              ErrorCodes.CustomerAlreadyBookedOntoSession,
                              "This customer is already booked for this session.",
                              string.Format("Customer: '{0}', Session: '{1}'", customerId, sessionId));
        }

        protected void ThenCreateSessionBooking(ApiResponse response, 
                                                ExpectedStandaloneSession session, 
                                                ExpectedCustomer customer, 
                                                SetupData setup,
                                                int expectedBookingCount = 1)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertCreateSingleSessionBooking(booking, session, customer);
            var bookingId = booking.id;

            var sessionResponse = AuthenticatedGet<SessionData>("Sessions", booking.session.id, setup);
            var sessionData = AssertSuccessResponse<SessionData>(sessionResponse);

            Assert.That(sessionData.booking.bookings.Count, Is.EqualTo(expectedBookingCount));
            var bookingOne = sessionData.booking.bookings[expectedBookingCount - 1];

            AssertCustomerBooking(bookingOne, bookingId, customer);
        }

        protected void ThenCreateSessionOnlineBooking(ApiResponse response,
                                                      ExpectedStandaloneSession session,
                                                      ExpectedCustomer customer,
                                                      SetupData setup,
                                                      int expectedBookingCount = 1)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertCreateSingleSessionOnlineBooking(booking, session, customer);
            var bookingId = booking.id;

            var sessionResponse = AuthenticatedGet<SessionData>("Sessions", booking.session.id, setup);
            var sessionData = AssertSuccessResponse<SessionData>(sessionResponse);

            Assert.That(sessionData.booking.bookings.Count, Is.EqualTo(expectedBookingCount));
            var bookingOne = sessionData.booking.bookings[expectedBookingCount - 1];

            AssertCustomerBooking(bookingOne, bookingId, customer);
        }

        private void AssertCustomerBooking(CustomerBookingData booking, Guid expectedBookingId, ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.EqualTo(expectedBookingId));

            var bookingCustomer = booking.customer;
            Assert.That(bookingCustomer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(bookingCustomer.firstName, Is.EqualTo(expectedCustomer.FirstName));
            Assert.That(bookingCustomer.lastName, Is.EqualTo(expectedCustomer.LastName));
            Assert.That(bookingCustomer.email, Is.EqualTo(expectedCustomer.Email));
            Assert.That(bookingCustomer.phone, Is.EqualTo(expectedCustomer.Phone));
        }

        protected void ThenCreateSingleCourseSessionBooking(ApiResponse response, SetupData setup, bool isOnlineBooking = false)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.AaronOrakeiHolidayCamp9To15For3Days.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(1));

            var sessionBooking = courseBooking.sessionBookings[0];

            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Assert(sessionBooking.session);
            setup.Fred.Assert(sessionBooking.customer);

            Assert.That(sessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBooking.hasAttended, Is.Null);
            Assert.That(sessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithSingleSessionBooking(courseBooking.id, sessionBooking.id, setup, isOnlineBooking);
        }

        private void GetAndAssertCourseWithSingleSessionBooking(Guid courseBookingId, Guid sessionBookingId, SetupData setup, bool isOnlineBooking)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));

            var sessionBooking = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBooking.id, Is.EqualTo(sessionBookingId));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(sessionBooking.hasAttended, Is.Null);
            Assert.That(sessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBooking.customer);
        }

        protected void ThenCreateAnotherCourseBooking(ApiResponse response, SetupData setup, bool isOnlineBooking = false)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.AaronOrakeiHolidayCamp9To15For3Days.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(1));

            var sessionBooking = courseBooking.sessionBookings[0];

            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Assert(sessionBooking.session);
            setup.Fred.Assert(sessionBooking.customer);

            Assert.That(sessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBooking.hasAttended, Is.Null);
            Assert.That(sessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertNewCourseBookingWithSingleSessionBookings(courseBooking.id, sessionBooking.id, setup, isOnlineBooking);
        }

        private void GetAndAssertNewCourseBookingWithSingleSessionBookings(Guid courseBookingId, Guid sessionBookingId, SetupData setup, bool isOnlineBooking)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(2));

            var courseBookingOne = course.booking.bookings[0];
            Assert.That(courseBookingOne.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBookingOne.parentId, Is.Null);
            setup.Fred.Assert(courseBookingOne.customer);

            var courseBookingTwo = course.booking.bookings[1];
            Assert.That(courseBookingTwo.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBookingTwo.parentId, Is.Null);
            Assert.That(courseBookingTwo.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBookingTwo.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));

            var sessionBookingOne = course.sessions[0].booking.bookings[0];
            Assert.That(sessionBookingOne.id, Is.EqualTo(sessionBookingId));
            Assert.That(sessionBookingOne.parentId, Is.EqualTo(courseBookingTwo.id));
            Assert.That(sessionBookingOne.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBookingOne.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBookingOne.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBookingOne.customer);

            var sessionBookingTwo = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBookingTwo.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sessionBookingTwo.parentId, Is.EqualTo(courseBookingOne.id));
            setup.Fred.Assert(sessionBookingTwo.customer);
        }

        protected void ThenCreateAllCourseSessionBookings(ApiResponse response, SetupData setup, bool isOnlineBooking = false)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.AaronOrakeiHolidayCamp9To15For3Days.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(3));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Assert(firstSessionBooking.session);
            setup.Fred.Assert(firstSessionBooking.customer);
            Assert.That(firstSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Assert(secondSessionBooking.session);
            setup.Fred.Assert(secondSessionBooking.customer);
            Assert.That(secondSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var thirdSessionBooking = courseBooking.sessionBookings[2];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Assert(thirdSessionBooking.session);
            setup.Fred.Assert(thirdSessionBooking.customer);
            Assert.That(thirdSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(thirdSessionBooking.hasAttended, Is.Null);
            Assert.That(thirdSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(thirdSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(thirdSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithAllSessionBookings(courseBooking.id, 
                                                     firstSessionBooking.id, 
                                                     secondSessionBooking.id, 
                                                     thirdSessionBooking.id, 
                                                     isOnlineBooking, 
                                                     setup);
        }

        private void GetAndAssertCourseWithAllSessionBookings(Guid courseBookingId,
                                                              Guid firstSessionBookingId,
                                                              Guid secondSessionBookingId,
                                                              Guid thirdSessionBookingId,
                                                              bool isOnlineBooking, 
                                                              SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));

            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var sessionBookingOne = course.sessions[0].booking.bookings[0];
            Assert.That(sessionBookingOne.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(sessionBookingOne.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBookingOne.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBookingOne.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBookingOne.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBookingOne.customer);

            var sessionBookingTwo = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBookingTwo.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(sessionBookingTwo.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBookingTwo.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBookingTwo.customer);

            var sessionBookingThree = course.sessions[2].booking.bookings[0];
            Assert.That(sessionBookingThree.id, Is.EqualTo(thirdSessionBookingId));
            Assert.That(sessionBookingThree.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(sessionBookingThree.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(sessionBookingThree.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(sessionBookingThree.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(sessionBookingThree.customer);
        }

        protected void ThenCreateAnotherCourseSessionBooking(ApiResponse response, SetupData setup, bool isOnlineBooking = false)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.AaronOrakeiHolidayCamp9To15For3Days.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(2));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Assert(firstSessionBooking.session);
            setup.Fred.Assert(firstSessionBooking.customer);
            Assert.That(firstSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Assert(secondSessionBooking.session);
            setup.Fred.Assert(secondSessionBooking.customer);
            Assert.That(secondSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithTwoCourseBookings(courseBooking.id, firstSessionBooking.id, secondSessionBooking.id, isOnlineBooking, setup);
        }

        private void GetAndAssertCourseWithTwoCourseBookings(Guid courseBookingId,
                                                             Guid firstSessionBookingId,
                                                             Guid secondSessionBookingId,
                                                             bool isOnlineBooking,
                                                             SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(2));

            var courseBookingOne = course.booking.bookings[0];
            Assert.That(courseBookingOne.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBookingOne.parentId, Is.Null);
            Assert.That(courseBookingOne.isOnlineBooking, Is.False);
            Assert.That(courseBookingOne.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBookingOne.customer);

            var courseBookingTwo = course.booking.bookings[1];
            Assert.That(courseBookingTwo.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBookingTwo.parentId, Is.Null);
            Assert.That(courseBookingTwo.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBookingTwo.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBookingTwo.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var firstSessionBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingTwo.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(firstSessionBooking.customer);

            var secondSessionBooking = course.sessions[1].booking.bookings[0];
            Assert.That(secondSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingOne.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.False);
            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(secondSessionBooking.customer);

            var thirdSessionBooking = course.sessions[2].booking.bookings[0];
            Assert.That(thirdSessionBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBookingTwo.id));
            Assert.That(thirdSessionBooking.hasAttended, Is.Null);
            Assert.That(thirdSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(thirdSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(thirdSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(thirdSessionBooking.customer);
        }

        protected void ThenCreateSeveralCourseSessionBooking(ApiResponse response, SetupData setup, bool isOnlineBooking = false)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            setup.AaronOrakeiHolidayCamp9To15For3Days.Assert(courseBooking.course);
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(courseBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(2));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Assert(firstSessionBooking.session);
            setup.Fred.Assert(firstSessionBooking.customer);
            Assert.That(firstSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Assert(secondSessionBooking.session);
            setup.Fred.Assert(secondSessionBooking.customer);
            Assert.That(secondSessionBooking.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithSeveralSessionBookings(courseBooking.id,
                                                         firstSessionBooking.id,
                                                         secondSessionBooking.id,
                                                         isOnlineBooking,
                                                         setup);
        }

        private void GetAndAssertCourseWithSeveralSessionBookings(Guid courseBookingId,
                                                                  Guid firstSessionBookingId,
                                                                  Guid secondSessionBookingId,
                                                                  bool isOnlineBooking,
                                                                  SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));

            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            Assert.That(courseBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(courseBooking.customer);

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var firstSessionBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);
            Assert.That(firstSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(firstSessionBooking.customer);

            var secondSessionBooking = course.sessions[2].booking.bookings[0];
            Assert.That(secondSessionBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);
            Assert.That(secondSessionBooking.isOnlineBooking, Is.EqualTo(isOnlineBooking));
            if (isOnlineBooking)
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_PAYMENT));
            else
                Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            setup.Fred.Assert(secondSessionBooking.customer);
        }
    }
}
