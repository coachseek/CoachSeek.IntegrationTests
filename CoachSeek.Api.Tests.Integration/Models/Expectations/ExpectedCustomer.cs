using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public abstract class ExpectedCustomer
    {
        public Guid Id { get; set; }

        public abstract string FirstName { get; }
        public abstract string LastName { get; }
        public string Email { get; private set; }
        public string Phone { get; private set; }


        protected ExpectedCustomer() 
        { }

        protected ExpectedCustomer(string email = null, string phone = null)
        {
            Email = email;
            Phone = phone != null ? phone.ToUpperInvariant() : null;
        }
    }
}
