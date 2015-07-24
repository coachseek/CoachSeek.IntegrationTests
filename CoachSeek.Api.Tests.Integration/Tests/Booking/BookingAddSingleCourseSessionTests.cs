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
    public class BookingAddSingleCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenNonExistentCustomer_WhenTryBookSingleCourseSession_ThenReturnNonExistentCustomerError()
        {
            var setup = RegisterBusiness();

            var command = GivenNonExistentCustomer(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnNonExistentCustomerError(response);
        }

        [Test]
        public void GivenTheCourseSessionIsFull_WhenTryBookSingleCourseSession_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();

            var command = GivenTheCourseSessionIsFull(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnCourseSessionFullError(response);
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryBookSingleCourseSession_ThenReturnDuplicateCourseSessionBookingError()
        {
            var setup = RegisterBusiness();

            var command = GivenThisCustomerIsAlreadyBookedOntoThisCourseSession(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenReturnDuplicateCourseSessionBookingError(response);
        }

        [Test]
        public void GivenValidSingleCourseSession_WhenTryBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenValidSingleCourseSession(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne_WhenTryBookSingleCourseSession_ThenCreateAnotherSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateAnotherSingleCourseSessionBooking(response, setup);
        }

        [Test]
        public void GivenDeleteBookingAndAddItAgain_WhenTryBookSingleCourseSession_ThenCreateSingleCourseSessionBooking()
        {
            var setup = RegisterBusiness();

            var command = GivenDeleteBookingAndAddItAgain(setup);
            var response = WhenTryBookSingleCourseSession(command, setup);
            ThenCreateSingleCourseSessionBooking(response, setup);
        }

        // TODO: Delete a session booking and re-add it and should be ok.
        // TODO: When have deleted all session bookings in a course booking then also remove the empty course booking.
        // TODO: ?

        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, Guid.NewGuid());
        }

        private ApiBookingSaveCommand GivenTheCourseSessionIsFull(SetupData setup)
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

        private ApiBookingSaveCommand GivenValidSingleCourseSession(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenCustomerIsBookedOnAnotherCourseSessionButNotThisOne(SetupData setup)
        {
            RegisterFredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
         
            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id);
        }

        private ApiBookingSaveCommand GivenDeleteBookingAndAddItAgain(SetupData setup)
        {
            RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            Delete<BookingData>("Bookings", setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.Id, setup);

            return new ApiBookingSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id);
        }


        private ApiResponse WhenTryBookSingleCourseSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryBookCourse(json, setup);
        }


        private void ThenReturnCourseSessionFullError(ApiResponse response)
        {
            AssertSingleError(response, "One or more of the sessions is already fully booked.");
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

            Assert.That(courseBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

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

            Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourse(courseBooking.id, sessionBooking.id, setup);
        }

        private void ThenCreateAnotherSingleCourseSessionBooking(ApiResponse response, SetupData setup)
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
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(1));

            var sessionBooking = courseBooking.sessionBookings[0];
            Assert.That(sessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(sessionBooking.parentId, Is.EqualTo(courseBooking.id));
            Assert.That(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions.Select(x => x.Id).Contains(sessionBooking.session.id), Is.True);
            Assert.That(sessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(15))));
            Assert.That(sessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(sessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(sessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            // Check the bookings on the course
            GetAndAssertCourseWithTwoBookings(courseBooking.id, sessionBooking.id, setup);
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

        private void GetAndAssertCourseWithTwoBookings(Guid courseBookingId, Guid sessionBookingId, SetupData setup)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", setup.AaronOrakeiHolidayCamp9To15For3Days.Id, setup);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(2));
            var courseBookingOne = course.booking.bookings[0];
            Assert.That(courseBookingOne.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(courseBookingOne.parentId, Is.Null);
            var customer = courseBookingOne.customer;
            Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(customer.phone, Is.EqualTo(setup.Fred.Phone));

            var courseBookingTwo = course.booking.bookings[1];
            Assert.That(courseBookingTwo.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBookingTwo.parentId, Is.Null);
            customer = courseBookingTwo.customer;
            Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(customer.phone, Is.EqualTo(setup.Fred.Phone));

            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(0));

            var sessionBookingOne = course.sessions[0].booking.bookings[0];
            Assert.That(sessionBookingOne.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sessionBookingOne.parentId, Is.EqualTo(courseBookingOne.id));
            Assert.That(sessionBookingOne.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(sessionBookingOne.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(sessionBookingOne.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(sessionBookingOne.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(sessionBookingOne.customer.phone, Is.EqualTo(setup.Fred.Phone));

            var sessionBookingTwo = course.sessions[1].booking.bookings[0];
            Assert.That(sessionBookingTwo.id, Is.EqualTo(sessionBookingId));
            Assert.That(sessionBookingTwo.parentId, Is.EqualTo(courseBookingTwo.id));
            Assert.That(sessionBookingTwo.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(sessionBookingTwo.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(sessionBookingTwo.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(sessionBookingTwo.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(sessionBookingTwo.customer.phone, Is.EqualTo(setup.Fred.Phone));
        }





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
