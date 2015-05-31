using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedBusiness
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Currency { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }

        public string Name { get; private set; }
        public ApiBusinessAdmin Admin { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }


        public ExpectedBusiness(string name, string currency, string email)
            : this(name, currency, null, null, "Bob", "Smith", email, "password1")
        { }

        public ExpectedBusiness(string name,
                                string currency,
                                string firstName,
                                string lastName,
                                string email,
                                string password)
        {
            Name = name;
            Currency = currency;
            PaymentProvider = null;
            MerchantAccountIdentifier = null;
            Admin = new ApiBusinessAdmin
            {
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                password = Password = password
            };
        }

        public ExpectedBusiness(string name,
                                string currency,
                                string paymentProvider,
                                string merchantAccountIdentifier,
                                string firstName, 
                                string lastName, 
                                string email, 
                                string password)
        {
            Name = name;
            Currency = currency;
            PaymentProvider = paymentProvider;
            MerchantAccountIdentifier = merchantAccountIdentifier;
            Admin = new ApiBusinessAdmin
            {
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                password = Password = password
            };
        }
    }
}
