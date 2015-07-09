//using System;
//using CoachSeek.Api.Tests.Integration.Models;

//namespace CoachSeek.Api.Tests.Integration.Tests.Booking
//{
//    public abstract class BaseBookingAddCourseTests : BaseBookingAddTests
//    {
//        protected Guid FredSessionBookingId { get; set; }


//        protected ApiBookingSaveCommand GivenNonExistentCustomer()
//        {
//            return new ApiBookingSaveCommand
//            {
//                session = new ApiSessionKey { id = AaronOrakei14To15.Id },
//                customer = new ApiCustomerKey { id = Guid.NewGuid() }
//            };
//        }


//        protected void ThenReturnNonExistentCustomerError(Response response)
//        {
//            AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
//        }

//        protected void ThenReturnCourseFullError(Response response)
//        {
//            AssertSingleError(response, "This course cannot be booked as it has sessions that are fully booked.");
//        }
//    }
//}
