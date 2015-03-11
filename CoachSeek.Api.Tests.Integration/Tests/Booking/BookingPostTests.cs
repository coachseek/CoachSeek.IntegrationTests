using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BookingPostTests : ScheduleTests
    {
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
            public void GivenNoBookingSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
            {
                var command = GivenNoBookingSaveCommand();
                var response = WhenPost(command);
                AssertSingleError(response, "Please post us some data!");
            }

            [Test]
            public void GivenEmptyBookingSaveCommand_WhenPost_ThenReturnMultipleErrors()
            {
                var command = GivenEmptyBookingSaveCommand();
                var response = WhenPost(command);
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
        public class BookingNewTests : BookingPostTests
        {
            [Test]
            public void GivenNonExistentSession_WhenTryBook_ThenReturnNonExistentSessionError()
            {
                var command = GivenNonExistentSession();
                var response = WhenTryBook(command);
                ThenReturNonExistentSessionError(response);
            }

            [Test]
            public void GivenNonExistentCustomer_WhenTryBook_ThenReturnNonExistentCustomerError()
            {
                var command = GivenNonExistentCustomer();
                var response = WhenTryBook(command);
                ThenReturNonExistentCustomerError(response);
            }

            [Test]
            public void GivenNonExistentSessionAndCustomer_WhenTryBook_ThenReturnNonExistentSessionAndCustomerErrors()
            {
                var command = GivenNonExistentSessionAndCustomer();
                var response = WhenTryBook(command);
                ThenReturnNonExistentSessionAndCustomerErrors(response);
            }

            [Test]
            public void GivenACustomerWhoIsNotInASession_WhenTryBook_ThenReturnSuccessfulBookingResponse()
            {
                var command = GivenACustomerWhoIsNotInASession();
                var response = WhenTryBook(command);
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

            private ApiBookingSaveCommand GivenACustomerWhoIsNotInASession()
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
                var booking = AssertSuccessResponse<BookingData>(response);

                Assert.That(booking.id, Is.InstanceOf<Guid>());
                var bookingId = booking.id;
                Assert.That(booking.session.id, Is.EqualTo(AaronOrakei16To17SessionId));
                Assert.That(booking.session.name, Is.EqualTo(string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 16:00", 
                                                                           GetFormattedDateOneWeekOut())));
                Assert.That(booking.customer.id, Is.EqualTo(BarneyId));
                Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", BARNEY_FIRST_NAME, RUBBLE_LAST_NAME)));

                var sessionResponse = Get<SessionData>("Sessions", booking.session.id);
                var session = AssertSuccessResponse<SessionData>(sessionResponse);

                Assert.That(session.booking.bookings.Count, Is.EqualTo(1));
                var bookingOne = session.booking.bookings[0];
                Assert.That(bookingOne.bookingId, Is.EqualTo(bookingId));
                var bookingCustomer = bookingOne.customer;
                Assert.That(bookingCustomer.id, Is.EqualTo(BarneyId));
                Assert.That(bookingCustomer.firstName, Is.EqualTo(BARNEY_FIRST_NAME));
                Assert.That(bookingCustomer.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
                Assert.That(bookingCustomer.email, Is.Null);
                Assert.That(bookingCustomer.phone, Is.Null);
            }
        }


        private Response WhenTryBook(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return Post<BookingData>(json);
        }
    }
}
