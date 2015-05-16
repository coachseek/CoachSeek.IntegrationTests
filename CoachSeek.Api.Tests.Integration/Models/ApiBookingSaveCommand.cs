using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBookingSaveCommand
    {
        public Guid? id { get; set; }

        public ApiSessionKey session { get; set; }
        public ApiCustomerKey customer { get; set; }


        public ApiBookingSaveCommand()
        { }

        public ApiBookingSaveCommand(Guid sessionId, Guid customerId)
        {
            session = new ApiSessionKey { id = sessionId };
            customer = new ApiCustomerKey { id = customerId };
        }
    }
}
