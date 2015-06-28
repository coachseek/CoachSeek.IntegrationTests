using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Domain { get; set; }
        public ApiBusinessAdmin Admin { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public ApiBusinessPaymentOptions Payment { get; private set; }


        public ExpectedBusiness(string name, string currency, string email)
            : this(name, currency, "Bob", "Smith", email, "password1")
        { }

        public ExpectedBusiness(string name,
                                string currency,
                                string firstName,
                                string lastName,
                                string email,
                                string password)
        {
            Name = name;
            Admin = new ApiBusinessAdmin
            {
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                password = Password = password
            };
            Payment = new ApiBusinessPaymentOptions
            {
                currency  = currency,
                isOnlinePaymentEnabled = false
            };
        }

        public ExpectedBusiness(string name,
                                string currency,
                                bool isOnlinePaymentEnabled,
                                bool? forceOnlinePayment,
                                string paymentProvider,
                                string merchantAccountIdentifier,
                                string firstName, 
                                string lastName, 
                                string email, 
                                string password)
        {
            Name = name;
            Admin = new ApiBusinessAdmin
            {
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                password = Password = password
            };
            Payment = new ApiBusinessPaymentOptions
            {
                currency = currency,
                isOnlinePaymentEnabled = isOnlinePaymentEnabled,
                forceOnlinePayment = forceOnlinePayment,
                paymentProvider = paymentProvider,
                merchantAccountIdentifier = merchantAccountIdentifier
            };
        }
    }
}
