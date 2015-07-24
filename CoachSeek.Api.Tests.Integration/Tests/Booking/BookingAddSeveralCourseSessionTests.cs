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
    public class BookingAddSeveralCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenDuplicateSessions_WhenTryBookSeveralCourseSessions_ThenReturnDuplicateSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenDuplicateSessions(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnDuplicateSessionError(response);
        }

        [Test]
        public void GivenSecondSessionIsStandalone_WhenTryBookSeveralCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenSecondSessionIsStandalone(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response);
        }

        [Test]
        public void GivenSecondSessionBelongsToADifferentCourse_WhenTryBookSeveralCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup);
            RegisterCustomerFred(setup);

            var command = GivenSecondSessionBelongsToADifferentCourse(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response);
        }

        [Test]
        public void GivenACourseSessionIsFull_WhenTryBookSeveralCourseSessions_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            var command = GivenACourseSessionIsFull(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenReturnCourseSessionFullError(response);
        }

        [Test]
        public void GivenValidSeveralCourseSessions_WhenTryBookSeveralCourseSessions_ThenCreateSeveralCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidSeveralCourseSessions(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateSeveralCourseSessionBookings(response, setup);
        }

        [Test]
        public void GivenValidSeveralCourseSessionsOutOfOrder_WhenTryBookSeveralCourseSessions_ThenCreateSeveralCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidSeveralCourseSessionsOutOfOrder(setup);
            var response = WhenTryBookSeveralCourseSessions(command, setup);
            ThenCreateSeveralCourseSessionBookings(response, setup);
        }


        protected ApiBookingSaveCommand GivenDuplicateSessions(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id
            }, 
            setup.Fred.Id);
        }

        protected ApiBookingSaveCommand GivenSecondSessionIsStandalone(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiMiniRed14To15.Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenSecondSessionBelongsToADifferentCourse(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
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
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.BamBam.Id);
        }

        private ApiBookingSaveCommand GivenValidSeveralCourseSessions(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            },
            setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenValidSeveralCourseSessionsOutOfOrder(SetupData setup)
        {
            return new ApiBookingSaveCommand(new[]
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id
            },
            setup.Fred.Id);
        }


        private ApiResponse WhenTryBookSeveralCourseSessions(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookCourse(json, setup);
        }


        protected void ThenReturnDuplicateSessionError(ApiResponse response)
        {
            AssertSingleError(response, "Some sessions are duplicates.", "booking.sessions");
        }

        private void ThenReturnSessionNotInCourseError(ApiResponse response)
        {
            AssertSingleError(response, "One or more of the sessions is not in the course.", "booking.sessions");
        }

        private void ThenReturnCourseSessionFullError(ApiResponse response)
        {
            AssertSingleError(response, "One or more of the sessions is already fully booked.");
        }

        private void ThenCreateSeveralCourseSessionBookings(ApiResponse response, SetupData setup)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            Assert.That(courseBooking.id, Is.InstanceOf<Guid>());
            Assert.That(courseBooking.course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(courseBooking.course.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith starting on {0} at 09:00 for 3 days",
                                                                            GetDateFormatNumberOfDaysOut(14))));
            Assert.That(courseBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(courseBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(2));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(firstSessionBooking.session.id), Is.True);
            Assert.That(firstSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(14))));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(firstSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(secondSessionBooking.session.id), Is.True);
            Assert.That(secondSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(16))));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(secondSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourse(courseBooking.id, firstSessionBooking.id, secondSessionBooking.id, setup);
        }

        private void GetAndAssertCourse(Guid courseBookingId, Guid firstSessionBookingId, Guid secondSessionBookingId, SetupData setup)
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
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));

            var firstSessionBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(firstSessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(firstSessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(firstSessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(firstSessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));

            var secondSessionBooking = course.sessions[2].booking.bookings[0];
            Assert.That(secondSessionBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(secondSessionBooking.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(secondSessionBooking.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(secondSessionBooking.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(secondSessionBooking.customer.phone, Is.EqualTo(setup.Fred.Phone));
        }


        //[Test]
        //public void GivenACustomerWhoIsNotBookedOntoACourse_WhenTryBookCourse_ThenCreateCourseBooking()
        //{
        //    var command = GivenACustomerWhoIsNotBookedOntoACourse();
        //    var response = WhenTryBookCourse(command);
        //    ThenCreateCourseBooking(response);
        //}







        //private ApiBookingSaveCommand GivenACustomerWhoIsNotBookedOntoACourse()
        //{
        //    // Book a single session on the same course for a more realistic test. 
        //    BookSingleSessionOnCourse();

        //    return new ApiBookingSaveCommand
        //    {
        //        session = new ApiSessionKey { id = BobbyRemueraHolidayCampFor3DaysCourseId },
        //        customer = new ApiCustomerKey { id = Barney.Id }
        //    };
        //}

        //private ApiBookingSaveCommand GivenASessionIsFull()
        //{
        //    return new ApiBookingSaveCommand
        //    {
        //        session = new ApiSessionKey { id = AaronOrakeiMiniBlueFor2DaysCourseId },
        //        customer = new ApiCustomerKey { id = Wilma.Id }
        //    };
        //}

        //private void BookSingleSessionOnCourse()
        //{
        //    var command = new ApiBookingSaveCommand
        //    {
        //        session = new ApiSessionKey { id = BobbyRemueraHolidayCampFor3DaysSessionIds[1] },
        //        customer = new ApiCustomerKey { id = Fred.Id }
        //    };

        //    var booking = WhenTryBookSession(command);
        //    FredSessionBookingId = ((SingleSessionBookingData)booking.Payload).id;
        //}


        //private void ThenCreateCourseBooking(Response response)
        //{
        //    var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

        //    Assert.That(courseBooking.id, Is.InstanceOf<Guid>());
        //    var courseBookingId = courseBooking.id;
        //    Assert.That(courseBooking.course.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
        //    Assert.That(courseBooking.course.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith starting on {0} at 10:00 for 3 days",
        //                                                               GetDateFormatNumberOfDaysOut(2))));
        //    Assert.That(courseBooking.customer.id, Is.EqualTo(Barney.Id));
        //    Assert.That(courseBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Barney.FirstName, Barney.LastName)));

        //    // Check bookings on sessions
        //    Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(3));

        //    var firstSessionBooking = courseBooking.sessionBookings[0];
        //    Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
        //    Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(firstSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[0]));
        //    Assert.That(firstSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
        //                                                               GetDateFormatNumberOfDaysOut(2))));
        //    Assert.That(firstSessionBooking.customer.id, Is.EqualTo(Barney.Id));
        //    Assert.That(firstSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Barney.FirstName, Barney.LastName)));

        //    var secondSessionBooking = courseBooking.sessionBookings[1];
        //    Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
        //    Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(secondSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
        //    Assert.That(secondSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
        //                                                               GetDateFormatNumberOfDaysOut(3))));
        //    Assert.That(secondSessionBooking.customer.id, Is.EqualTo(Barney.Id));
        //    Assert.That(secondSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Barney.FirstName, Barney.LastName)));

        //    var thirdSessionBooking = courseBooking.sessionBookings[2];
        //    Assert.That(thirdSessionBooking.id, Is.InstanceOf<Guid>());
        //    Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(thirdSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[2]));
        //    Assert.That(thirdSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
        //                                                               GetDateFormatNumberOfDaysOut(4))));
        //    Assert.That(thirdSessionBooking.customer.id, Is.EqualTo(Barney.Id));
        //    Assert.That(thirdSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Barney.FirstName, Barney.LastName)));

        //    // Check the bookings on the course
        //    GetAndAssertCourse(courseBookingId, firstSessionBooking.id, secondSessionBooking.id, thirdSessionBooking.id);
        //}

        //private void GetAndAssertCourse(Guid courseBookingId, Guid firstSessionBookingId, Guid secondSessionBookingId, Guid thirdSessionBookingId)
        //{
        //    var courseResponse = AuthenticatedGet<CourseData>("Sessions", BobbyRemueraHolidayCampFor3DaysCourseId);
        //    var course = AssertSuccessResponse<CourseData>(courseResponse);

        //    Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
        //    var courseBooking = course.booking.bookings[0];
        //    Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
        //    Assert.That(courseBooking.parentId, Is.Null);
        //    var customerBarney = courseBooking.customer;
        //    Assert.That(customerBarney.id, Is.EqualTo(Barney.Id));
        //    Assert.That(customerBarney.firstName, Is.EqualTo(Barney.FirstName));
        //    Assert.That(customerBarney.lastName, Is.EqualTo(Barney.LastName));
        //    Assert.That(customerBarney.email, Is.Null);
        //    Assert.That(customerBarney.phone, Is.Null);

        //    // Check bookings on sessions
        //    Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
        //    var firstSessionFirstBooking = course.sessions[0].booking.bookings[0];
        //    Assert.That(firstSessionFirstBooking.id, Is.EqualTo(firstSessionBookingId));
        //    Assert.That(firstSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(firstSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
        //    Assert.That(firstSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
        //    Assert.That(firstSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
        //    Assert.That(firstSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
        //    Assert.That(firstSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

        //    Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(2));
        //    var secondSessionFirstBooking = course.sessions[1].booking.bookings[0];
        //    Assert.That(secondSessionFirstBooking.id, Is.EqualTo(secondSessionBookingId));
        //    Assert.That(secondSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(secondSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
        //    Assert.That(secondSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
        //    Assert.That(secondSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
        //    Assert.That(secondSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
        //    Assert.That(secondSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

        //    var secondSessionSecondBooking = course.sessions[1].booking.bookings[1];
        //    Assert.That(secondSessionSecondBooking.id, Is.EqualTo(FredSessionBookingId));
        //    Assert.That(secondSessionSecondBooking.parentId, Is.Null);
        //    Assert.That(secondSessionSecondBooking.customer.id, Is.EqualTo(Fred.Id));
        //    Assert.That(secondSessionSecondBooking.customer.firstName, Is.EqualTo(Fred.FirstName));
        //    Assert.That(secondSessionSecondBooking.customer.lastName, Is.EqualTo(Fred.LastName));
        //    Assert.That(secondSessionSecondBooking.customer.email, Is.EqualTo(Fred.Email));
        //    Assert.That(secondSessionSecondBooking.customer.phone, Is.EqualTo(Fred.Phone.ToUpper()));

        //    Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));
        //    var thirdSessionFirstBooking = course.sessions[2].booking.bookings[0];
        //    Assert.That(thirdSessionFirstBooking.id, Is.EqualTo(thirdSessionBookingId));
        //    Assert.That(thirdSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
        //    Assert.That(thirdSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
        //    Assert.That(thirdSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
        //    Assert.That(thirdSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
        //    Assert.That(thirdSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
        //    Assert.That(thirdSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));
        //}
    }
}
