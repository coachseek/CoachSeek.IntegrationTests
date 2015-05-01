//using System;
//using CoachSeek.Api.Tests.Integration.Models;
//using CoachSeek.Api.Tests.Integration.Models.Expectations;
//using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
//using NUnit.Framework;

//namespace CoachSeek.Api.Tests.Integration.Tests.Booking
//{
//    public abstract class BookingPostTests : BaseBookingTests
//    {
//        private Guid FredSessionBookingId { get; set; }





//        private void GetAndAssertCourse(Guid courseBookingId, Guid firstSessionBookingId, Guid secondSessionBookingId, Guid thirdSessionBookingId)
//        {
//            var courseResponse = AuthenticatedGet<CourseData>("Sessions", BobbyRemueraHolidayCampFor3DaysCourseId);
//            var course = AssertSuccessResponse<CourseData>(courseResponse);

//            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
//            var courseBooking = course.booking.bookings[0];
//            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
//            Assert.That(courseBooking.parentId, Is.Null);
//            var customerBarney = courseBooking.customer;
//            Assert.That(customerBarney.id, Is.EqualTo(Barney.Id));
//            Assert.That(customerBarney.firstName, Is.EqualTo(Barney.FirstName));
//            Assert.That(customerBarney.lastName, Is.EqualTo(Barney.LastName));
//            Assert.That(customerBarney.email, Is.Null);
//            Assert.That(customerBarney.phone, Is.Null);

//            // Check bookings on sessions
//            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
//            var firstSessionFirstBooking = course.sessions[0].booking.bookings[0];
//            Assert.That(firstSessionFirstBooking.id, Is.EqualTo(firstSessionBookingId));
//            Assert.That(firstSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
//            Assert.That(firstSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
//            Assert.That(firstSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
//            Assert.That(firstSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
//            Assert.That(firstSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
//            Assert.That(firstSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

//            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(2));
//            var secondSessionFirstBooking = course.sessions[1].booking.bookings[0];
//            Assert.That(secondSessionFirstBooking.id, Is.EqualTo(secondSessionBookingId));
//            Assert.That(secondSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
//            Assert.That(secondSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
//            Assert.That(secondSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
//            Assert.That(secondSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
//            Assert.That(secondSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
//            Assert.That(secondSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

//            var secondSessionSecondBooking = course.sessions[1].booking.bookings[1];
//            Assert.That(secondSessionSecondBooking.id, Is.EqualTo(FredSessionBookingId));
//            Assert.That(secondSessionSecondBooking.parentId, Is.Null);
//            Assert.That(secondSessionSecondBooking.customer.id, Is.EqualTo(Fred.Id));
//            Assert.That(secondSessionSecondBooking.customer.firstName, Is.EqualTo(Fred.FirstName));
//            Assert.That(secondSessionSecondBooking.customer.lastName, Is.EqualTo(Fred.LastName));
//            Assert.That(secondSessionSecondBooking.customer.email, Is.EqualTo(Fred.Email));
//            Assert.That(secondSessionSecondBooking.customer.phone, Is.EqualTo(Fred.Phone.ToUpper()));

//            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));
//            var thirdSessionFirstBooking = course.sessions[2].booking.bookings[0];
//            Assert.That(thirdSessionFirstBooking.id, Is.EqualTo(thirdSessionBookingId));
//            Assert.That(thirdSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
//            Assert.That(thirdSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
//            Assert.That(thirdSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
//            Assert.That(thirdSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
//            Assert.That(thirdSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
//            Assert.That(thirdSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));
//        }
//    }
//}
