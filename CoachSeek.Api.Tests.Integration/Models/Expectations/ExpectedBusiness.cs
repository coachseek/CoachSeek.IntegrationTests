using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Domain { get; set; }
        public string Sport { get; set; }
        public ApiBusinessAdmin Admin { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public ApiBusinessPaymentOptions Payment { get; private set; }


        public ExpectedBusiness(string name, string currency, string email)
            : this(name, "Tennis", currency, "Bob", "Smith", email, "0900COACHSEEK", "password1")
        { }

        public ExpectedBusiness(string name,
                                string sport,
                                string currency,
                                string firstName,
                                string lastName,
                                string email,
                                string phone,
                                string password)
        {
            Name = name;
            Sport = sport;
            Admin = new ApiBusinessAdmin
            {
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                phone = phone,
                password = Password = password
            };
            Payment = new ApiBusinessPaymentOptions
            {
                currency  = currency,
                isOnlinePaymentEnabled = false
            };
        }

        public ExpectedBusiness(string name,
                                string sport,
                                string currency,
                                bool isOnlinePaymentEnabled,
                                bool? forceOnlinePayment,
                                string paymentProvider,
                                string merchantAccountIdentifier,
                                Guid adminId,
                                string firstName,
                                string lastName,
                                string email,
                                string phone,
                                string password)
        {
            Name = name;
            Sport = sport;
            Admin = new ApiBusinessAdmin
            {
                id = adminId,
                firstName = firstName,
                lastName = lastName,
                email = UserName = email,
                phone = phone,
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

        public ExpectedBusiness(ExpectedBusiness business)
        {
            Name = business.Name;
            Admin = new ApiBusinessAdmin
            {
                id = business.Admin.id,
                firstName = business.Admin.firstName,
                lastName = business.Admin.lastName,
                email = UserName = business.Admin.email,
                phone = business.Admin.phone,
                password = Password = business.Admin.password
            };
            Payment = new ApiBusinessPaymentOptions
            {
                currency = business.Payment.currency,
                isOnlinePaymentEnabled = business.Payment.isOnlinePaymentEnabled,
                forceOnlinePayment = business.Payment.forceOnlinePayment,
                paymentProvider = business.Payment.paymentProvider,
                merchantAccountIdentifier = business.Payment.merchantAccountIdentifier
            };
        }
    }
}
