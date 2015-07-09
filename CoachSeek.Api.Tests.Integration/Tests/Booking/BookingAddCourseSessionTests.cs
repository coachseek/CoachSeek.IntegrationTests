using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingAddCourseSessionTests : BaseBookingAddSessionTests
    {
        //[Test]
        //public void GivenNonExistentCustomer_WhenTryBookCourseSessions_ThenReturnNonExistentCustomerError()
        //{
        //    var setup = RegisterBusiness();
        //    RegisterStandaloneAaronOrakei14To15(setup);

        //    var command = GivenNonExistentCustomer(setup);
        //    var response = WhenTryBookStandaloneSession(command, setup);
        //    ThenReturnNonExistentCustomerError(response);
        //}




        //[Test]
        //public void GivenASessionIsFull_WhenTryBookCourse_ThenReturnCourseFullError()
        //{
        //    var command = GivenASessionIsFull();
        //    var response = WhenTryBookCourse(command);
        //    ThenReturnCourseFullError(response);
        //}

        //[Test]
        //public void GivenACustomerWhoIsNotBookedOntoACourse_WhenTryBookCourse_ThenCreateCourseBooking()
        //{
        //    var command = GivenACustomerWhoIsNotBookedOntoACourse();
        //    var response = WhenTryBookCourse(command);
        //    ThenCreateCourseBooking(response);
        //}


        protected ApiBookingSaveCommand GivenNonExistentCustomer(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, Guid.NewGuid());
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
