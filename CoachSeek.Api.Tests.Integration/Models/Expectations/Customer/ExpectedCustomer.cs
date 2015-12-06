using System;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Customer
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


        public void Assert(CustomerKeyData actualCustomer)
        {
            NUnit.Framework.Assert.That(actualCustomer.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualCustomer.name, Is.EqualTo(string.Format("{0} {1}", FirstName, LastName)));
        }

        public void Assert(CustomerData actualCustomer)
        {
            NUnit.Framework.Assert.That(actualCustomer.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualCustomer.firstName, Is.EqualTo(FirstName));
            NUnit.Framework.Assert.That(actualCustomer.lastName, Is.EqualTo(LastName));
            NUnit.Framework.Assert.That(actualCustomer.email, Is.EqualTo(Email));
            NUnit.Framework.Assert.That(actualCustomer.phone, Is.EqualTo(Phone));
        }
    }
}
