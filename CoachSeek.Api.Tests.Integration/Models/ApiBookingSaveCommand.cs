using System;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBookingSaveCommand
    {
        public Guid? id { get; set; }

        public IList<ApiSessionKey> sessions { get; set; }
        public ApiCustomerKey customer { get; set; }


        public ApiBookingSaveCommand()
        {
            sessions = new List<ApiSessionKey>();
        }

        public ApiBookingSaveCommand(Guid sessionId, Guid customerId)
        {
            sessions = new List<ApiSessionKey>
            {
                new ApiSessionKey {id = sessionId}
            };
            customer = new ApiCustomerKey { id = customerId };
        }

        public ApiBookingSaveCommand(IEnumerable<Guid> sessionIds, Guid customerId)
        {
            sessions = new List<ApiSessionKey>();
            foreach (var sessionId in sessionIds)
                sessions.Add(new ApiSessionKey { id = sessionId });            
            customer = new ApiCustomerKey { id = customerId };
        }
    }
}
