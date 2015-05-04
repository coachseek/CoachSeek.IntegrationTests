using System;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingAddCourseTests : BaseBookingAddCourseTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }

        
        [Test]
        public void GivenCourseIsNotOnlineBookable_WhenTryBookOnlineCourse_ThenReturnNotOnlineBookableError()
        {
            var command = GivenCourseIsNotOnlineBookable();
            var response = WhenTryBookOnlineCourse(command);
            ThenReturnCourseNotOnlineBookableError(response);
        }

        [Test, Ignore("TODO")]
        public void GivenCourseIsFullyBooked_WhenTryBookOnlineCourse_ThenReturnNoSpacesAvailableError()
        {
            var command = GivenCourseIsFullyBooked();
            var response = WhenTryBookOnlineCourse(command);
            ThenReturnCourseNotOnlineBookableError(response);
        }

        [Test]
        public void GivenCourseIsOnlineBookable_WhenTryBookOnlineCourse_ThenCreateSessionBooking()
        {
            var command = GivenCourseIsOnlineBookable();
            var response = WhenTryBookOnlineCourse(command);
            ThenCreateCourseBooking(response);
        }


        protected ApiBookingSaveCommand GivenCourseIsNotOnlineBookable()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = BobbyRemueraHolidayCampFor3DaysCourseId },
                customer = new ApiCustomerKey { id = Wilma.Id }
            };
        }

        private ApiBookingSaveCommand GivenCourseIsFullyBooked()
        {
            throw new NotImplementedException();
        }

        protected ApiBookingSaveCommand GivenCourseIsOnlineBookable()
        {
            return new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronRemuera9To10For4WeeksCourseId },
                customer = new ApiCustomerKey { id = Wilma.Id }
            };
        }


        private void ThenReturnCourseNotOnlineBookableError(Response response)
        {
            AssertSingleError(response, "This course is not online bookable.", "booking.session");
        }

        private void ThenCreateCourseBooking(Response response)
        {
            var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

            Assert.That(courseBooking.id, Is.InstanceOf<Guid>());
            var courseBookingId = courseBooking.id;
            Assert.That(courseBooking.course.id, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(courseBooking.course.name, Is.EqualTo(string.Format("Mini Red at Remuera Racquets Club with Aaron Smith starting on {0} at 09:00 for 4 weeks",
                                                                       GetDateFormatNumberOfDaysOut(7))));
            Assert.That(courseBooking.customer.id, Is.EqualTo(Wilma.Id));
            Assert.That(courseBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Wilma.FirstName, Wilma.LastName)));

            // Check bookings on sessions
            Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(4));

            var firstSessionBooking = courseBooking.sessionBookings[0];
            Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(firstSessionBooking.session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[0]));
            Assert.That(firstSessionBooking.session.name, Is.EqualTo(string.Format("Mini Red at Remuera Racquets Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(7))));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(Wilma.Id));
            Assert.That(firstSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Wilma.FirstName, Wilma.LastName)));

            var secondSessionBooking = courseBooking.sessionBookings[1];
            Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(secondSessionBooking.session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[1]));
            Assert.That(secondSessionBooking.session.name, Is.EqualTo(string.Format("Mini Red at Remuera Racquets Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(14))));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(Wilma.Id));
            Assert.That(secondSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Wilma.FirstName, Wilma.LastName)));

            var thirdSessionBooking = courseBooking.sessionBookings[2];
            Assert.That(thirdSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(thirdSessionBooking.session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[2]));
            Assert.That(thirdSessionBooking.session.name, Is.EqualTo(string.Format("Mini Red at Remuera Racquets Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(21))));
            Assert.That(thirdSessionBooking.customer.id, Is.EqualTo(Wilma.Id));
            Assert.That(thirdSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Wilma.FirstName, Wilma.LastName)));

            var fourthSessionBooking = courseBooking.sessionBookings[3];
            Assert.That(fourthSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(fourthSessionBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(fourthSessionBooking.session.id, Is.EqualTo(AaronRemuera9To10For4WeeksSessionIds[3]));
            Assert.That(fourthSessionBooking.session.name, Is.EqualTo(string.Format("Mini Red at Remuera Racquets Club with Aaron Smith on {0} at 09:00",
                                                                       GetDateFormatNumberOfDaysOut(28))));
            Assert.That(fourthSessionBooking.customer.id, Is.EqualTo(Wilma.Id));
            Assert.That(fourthSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", Wilma.FirstName, Wilma.LastName)));

            // Check the bookings on the course
            GetAndAssertCourse(courseBookingId, 
                               firstSessionBooking.id,
                               secondSessionBooking.id,
                               thirdSessionBooking.id,
                               fourthSessionBooking.id);
        }

        private void GetAndAssertCourse(Guid courseBookingId, 
                                        Guid firstSessionBookingId,
                                        Guid secondSessionBookingId,
                                        Guid thirdSessionBookingId,
                                        Guid fourthSessionBookingId)
        {
            var courseResponse = AuthenticatedGet<CourseData>("Sessions", AaronRemuera9To10For4WeeksCourseId);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);

            Wilma.Assert(courseBooking.customer);
            var customerWilma = courseBooking.customer;

            // Check bookings on sessions
            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            var firstSessionFirstBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionFirstBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            AssertCustomer(firstSessionFirstBooking.customer, customerWilma);

            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(1));
            var secondSessionFirstBooking = course.sessions[1].booking.bookings[0];
            Assert.That(secondSessionFirstBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            AssertCustomer(secondSessionFirstBooking.customer, customerWilma);

            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));
            var thirdSessionFirstBooking = course.sessions[2].booking.bookings[0];
            Assert.That(thirdSessionFirstBooking.id, Is.EqualTo(thirdSessionBookingId));
            Assert.That(thirdSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            AssertCustomer(thirdSessionFirstBooking.customer, customerWilma);

            Assert.That(course.sessions[3].booking.bookings.Count, Is.EqualTo(1));
            var fourthSessionFirstBooking = course.sessions[3].booking.bookings[0];
            Assert.That(fourthSessionFirstBooking.id, Is.EqualTo(fourthSessionBookingId));
            Assert.That(fourthSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            AssertCustomer(fourthSessionFirstBooking.customer, customerWilma);
        }


        public void AssertCustomer(CustomerData actualCustomer, CustomerData expectedCustomer)
        {
            Assert.That(actualCustomer.id, Is.EqualTo(expectedCustomer.id));
            Assert.That(actualCustomer.firstName, Is.EqualTo(expectedCustomer.firstName));
            Assert.That(actualCustomer.lastName, Is.EqualTo(expectedCustomer.lastName));
            Assert.That(actualCustomer.email, Is.EqualTo(expectedCustomer.email));
            Assert.That(actualCustomer.phone, Is.EqualTo(expectedCustomer.phone));
        }

        public void AssertCustomer(CustomerData actualCustomer, ExpectedCustomer expectedCustomer)
        {
            Assert.That(actualCustomer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(actualCustomer.firstName, Is.EqualTo(expectedCustomer.FirstName));
            Assert.That(actualCustomer.lastName, Is.EqualTo(expectedCustomer.LastName));
            Assert.That(actualCustomer.email, Is.EqualTo(expectedCustomer.Email));
            Assert.That(actualCustomer.phone, Is.EqualTo(expectedCustomer.Phone));
        }
    }
}
