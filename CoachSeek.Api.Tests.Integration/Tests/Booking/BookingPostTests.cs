using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BookingPostTests : ScheduleTests
    {
        private Guid FredSessionBookingId { get; set; }


        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }

        protected override string RelativePath
        {
            get { return "Bookings"; }
        }

        [TestFixture]
        public class BookingCommandTests : BookingPostTests
        {
            [Test]
            public void GivenNoBookingSaveCommand_WhenTryBookSession_ThenReturnNoDataErrorResponse()
            {
                var command = GivenNoBookingSaveCommand();
                var response = WhenTryBookSession(command);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptyBookingSaveCommand_WhenTryBookSession_ThenReturnMultipleErrors()
            {
                var command = GivenEmptyBookingSaveCommand();
                var response = WhenTryBookSession(command);
                AssertMultipleErrors(response, new[,] { { "The session field is required.", "booking.session" },
                                                        { "The customer field is required.", "booking.customer" } });
            }

            private string GivenNoBookingSaveCommand()
            {
                return "";
            }

            private string GivenEmptyBookingSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class BookingSessionTests : BookingPostTests
        {
            [Test]
            public void GivenNonExistentSession_WhenTryBookSession_ThenReturnNonExistentSessionError()
            {
                var command = GivenNonExistentSession();
                var response = WhenTryBookSession(command);
                ThenReturNonExistentSessionError(response);
            }

            [Test]
            public void GivenNonExistentCustomer_WhenTryBookSession_ThenReturnNonExistentCustomerError()
            {
                var command = GivenNonExistentCustomer();
                var response = WhenTryBookSession(command);
                ThenReturNonExistentCustomerError(response);
            }

            [Test]
            public void GivenNonExistentSessionAndCustomer_WhenTryBookSession_ThenReturnNonExistentSessionErrorOnly()
            {
                var command = GivenNonExistentSessionAndCustomer();
                var response = WhenTryBookSession(command);
                ThenReturNonExistentSessionError(response);
            }

            [Test]
            public void GivenACustomerWhoIsNotBookedOntoASession_WhenTryBookSession_ThenReturnSuccessfulBookingResponse()
            {
                var command = GivenACustomerWhoIsNotBookedOntoASession();
                var response = WhenTryBookSession(command);
                ThenReturnSuccessfulBookingResponse(response);
            }


            private ApiBookingSaveCommand GivenNonExistentSession()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = Guid.NewGuid() },
                    customer = new ApiCustomerKey { id = FredId }
                };
            }

            private ApiBookingSaveCommand GivenNonExistentCustomer()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = AaronOrakei14To15SessionId },
                    customer = new ApiCustomerKey { id = Guid.NewGuid() }
                };
            }

            private ApiBookingSaveCommand GivenNonExistentSessionAndCustomer()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = Guid.NewGuid() },
                    customer = new ApiCustomerKey { id = Guid.NewGuid() }
                };
            }

            private ApiBookingSaveCommand GivenACustomerWhoIsNotBookedOntoASession()
            {
                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = AaronOrakei16To17SessionId },
                    customer = new ApiCustomerKey { id = BarneyId }
                };
            }


            private void ThenReturNonExistentSessionError(Response response)
            {
                AssertSingleError(response, "This session does not exist.", "booking.session.id");
            }

            private void ThenReturNonExistentCustomerError(Response response)
            {
                AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
            }

            private void ThenReturnNonExistentSessionAndCustomerErrors(Response response)
            {
                AssertMultipleErrors(response, new[,] { { "This session does not exist.", "booking.session.id" },
                                                        { "This customer does not exist.", "booking.customer.id" } });
            }

            private void ThenReturnSuccessfulBookingResponse(Response response)
            {
                var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

                Assert.That(booking.id, Is.InstanceOf<Guid>());
                var bookingId = booking.id;
                Assert.That(booking.parentId, Is.Null);
                Assert.That(booking.session.id, Is.EqualTo(AaronOrakei16To17SessionId));
                Assert.That(booking.session.name, Is.EqualTo(string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 16:00", 
                                                                           GetFormattedDateOneWeekOut())));
                Assert.That(booking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                var sessionResponse = Get<SessionData>("Sessions", booking.session.id);
                var session = AssertSuccessResponse<SessionData>(sessionResponse);

                Assert.That(session.booking.bookings.Count, Is.EqualTo(1));
                var bookingOne = session.booking.bookings[0];
                Assert.That(bookingOne.id, Is.EqualTo(bookingId));
                var bookingCustomer = bookingOne.customer;
                Assert.That(bookingCustomer.id, Is.EqualTo(BarneyId));
                Assert.That(bookingCustomer.firstName, Is.EqualTo(BARNEY_FIRST_NAME));
                Assert.That(bookingCustomer.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
                Assert.That(bookingCustomer.email, Is.Null);
                Assert.That(bookingCustomer.phone, Is.Null);
            }
        }


        [TestFixture]
        public class BookingCourseTests : BookingPostTests
        {
            [Test]
            public void GivenACustomerWhoIsNotBookedOntoACourse_WhenTryBookCourse_ThenReturnSuccessfulCourseBookingResponse()
            {
                var command = GivenACustomerWhoIsNotBookedOntoACourse();
                var response = WhenTryBookCourse(command);
                ThenReturnSuccessfulCourseBookingResponse(response);
            }


            private ApiBookingSaveCommand GivenACustomerWhoIsNotBookedOntoACourse()
            {
                // Book a single session on the same course for a more realistic test. 
                BookSingleSessionOnCourse();

                return new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = BobbyRemueraHolidayCampFor3DaysCourseId },
                    customer = new ApiCustomerKey { id = BarneyId }
                };
            }

            private void BookSingleSessionOnCourse()
            {
                var command = new ApiBookingSaveCommand
                {
                    session = new ApiSessionKey { id = BobbyRemueraHolidayCampFor3DaysSessionIds[1] },
                    customer = new ApiCustomerKey { id = FredId }
                };

                var booking = WhenTryBookSession(command);
                FredSessionBookingId = ((SingleSessionBookingData) booking.Payload).id;
            }


            private void ThenReturnSuccessfulCourseBookingResponse(Response response)
            {
                var courseBooking = AssertSuccessResponse<CourseBookingData>(response);

                Assert.That(courseBooking.id, Is.InstanceOf<Guid>());
                var courseBookingId = courseBooking.id;
                Assert.That(courseBooking.course.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
                Assert.That(courseBooking.course.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith starting on {0} at 10:00 for 3 days",
                                                                           GetDateFormatNumberOfDaysOut(2))));
                Assert.That(courseBooking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(courseBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                // Check bookings on sessions
                Assert.That(courseBooking.sessionBookings.Count, Is.EqualTo(3));

                var firstSessionBooking = courseBooking.sessionBookings[0];
                Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
                Assert.That(firstSessionBooking.parentId, Is.EqualTo(courseBookingId));
                Assert.That(firstSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[0]));
                Assert.That(firstSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
                                                                           GetDateFormatNumberOfDaysOut(2))));
                Assert.That(firstSessionBooking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(firstSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                var secondSessionBooking = courseBooking.sessionBookings[1];
                Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
                Assert.That(secondSessionBooking.parentId, Is.EqualTo(courseBookingId));
                Assert.That(secondSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
                Assert.That(secondSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
                                                                           GetDateFormatNumberOfDaysOut(3))));
                Assert.That(secondSessionBooking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(secondSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                var thirdSessionBooking = courseBooking.sessionBookings[2];
                Assert.That(thirdSessionBooking.id, Is.InstanceOf<Guid>());
                Assert.That(thirdSessionBooking.parentId, Is.EqualTo(courseBookingId));
                Assert.That(thirdSessionBooking.session.id, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[2]));
                Assert.That(thirdSessionBooking.session.name, Is.EqualTo(string.Format("Holiday Camp at Remuera Racquets Club with Bobby Smith on {0} at 10:00",
                                                                           GetDateFormatNumberOfDaysOut(4))));
                Assert.That(thirdSessionBooking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(thirdSessionBooking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                // Check the bookings on the course
                GetAndAssertCourse(courseBookingId, firstSessionBooking.id, secondSessionBooking.id, thirdSessionBooking.id);
            }
        }

        private void GetAndAssertCourse(Guid courseBookingId, Guid firstSessionBookingId, Guid secondSessionBookingId, Guid thirdSessionBookingId)
        {
            var courseResponse = Get<CourseData>("Sessions", BobbyRemueraHolidayCampFor3DaysCourseId);
            var course = AssertSuccessResponse<CourseData>(courseResponse);

            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            var courseBooking = course.booking.bookings[0];
            Assert.That(courseBooking.id, Is.EqualTo(courseBookingId));
            Assert.That(courseBooking.parentId, Is.Null);
            Assert.That(courseBooking.sessionId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysCourseId));
            var customerBarney = courseBooking.customer;
            Assert.That(customerBarney.id, Is.EqualTo(BarneyId));
            Assert.That(customerBarney.firstName, Is.EqualTo(BARNEY_FIRST_NAME));
            Assert.That(customerBarney.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
            Assert.That(customerBarney.email, Is.Null);
            Assert.That(customerBarney.phone, Is.Null);

            // Check bookings on sessions
            Assert.That(course.sessions[0].booking.bookings.Count, Is.EqualTo(1));
            var firstSessionFirstBooking = course.sessions[0].booking.bookings[0];
            Assert.That(firstSessionFirstBooking.id, Is.EqualTo(firstSessionBookingId));
            Assert.That(firstSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(firstSessionFirstBooking.sessionId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[0]));
            Assert.That(firstSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
            Assert.That(firstSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
            Assert.That(firstSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
            Assert.That(firstSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
            Assert.That(firstSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

            Assert.That(course.sessions[1].booking.bookings.Count, Is.EqualTo(2));
            var secondSessionFirstBooking = course.sessions[1].booking.bookings[0];
            Assert.That(secondSessionFirstBooking.id, Is.EqualTo(secondSessionBookingId));
            Assert.That(secondSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(secondSessionFirstBooking.sessionId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
            Assert.That(secondSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
            Assert.That(secondSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
            Assert.That(secondSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
            Assert.That(secondSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
            Assert.That(secondSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));

            var secondSessionSecondBooking = course.sessions[1].booking.bookings[1];
            Assert.That(secondSessionSecondBooking.id, Is.EqualTo(FredSessionBookingId));
            Assert.That(secondSessionSecondBooking.parentId, Is.Null);
            Assert.That(secondSessionSecondBooking.sessionId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[1]));
            Assert.That(secondSessionSecondBooking.customer.id, Is.EqualTo(FredId));
            Assert.That(secondSessionSecondBooking.customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(secondSessionSecondBooking.customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(secondSessionSecondBooking.customer.email, Is.EqualTo(FredEmail));
            Assert.That(secondSessionSecondBooking.customer.phone, Is.EqualTo(FredPhone.ToUpper()));

            Assert.That(course.sessions[2].booking.bookings.Count, Is.EqualTo(1));
            var thirdSessionFirstBooking = course.sessions[2].booking.bookings[0];
            Assert.That(thirdSessionFirstBooking.id, Is.EqualTo(thirdSessionBookingId));
            Assert.That(thirdSessionFirstBooking.parentId, Is.EqualTo(courseBookingId));
            Assert.That(thirdSessionFirstBooking.sessionId, Is.EqualTo(BobbyRemueraHolidayCampFor3DaysSessionIds[2]));
            Assert.That(thirdSessionFirstBooking.customer.id, Is.EqualTo(customerBarney.id));
            Assert.That(thirdSessionFirstBooking.customer.firstName, Is.EqualTo(customerBarney.firstName));
            Assert.That(thirdSessionFirstBooking.customer.lastName, Is.EqualTo(customerBarney.lastName));
            Assert.That(thirdSessionFirstBooking.customer.email, Is.EqualTo(customerBarney.email));
            Assert.That(thirdSessionFirstBooking.customer.phone, Is.EqualTo(customerBarney.phone));
        }

        private Response WhenTryBookSession(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookSession(json);
        }

        private Response WhenTryBookCourse(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookCourse(json);
        }

        private Response WhenTryBookSession(string json)
        {
            return Post<SingleSessionBookingData>(json);
        }

        private Response WhenTryBookCourse(string json)
        {
            return Post<CourseBookingData>(json);
        }
    }
}
