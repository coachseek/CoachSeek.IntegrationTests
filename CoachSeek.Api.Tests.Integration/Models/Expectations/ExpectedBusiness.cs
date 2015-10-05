using System;
using CoachSeek.Common;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Domain { get; set; }
        public string Sport { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime AuthorisedUntil { get; set; }
        public string SubscriptionPlan { get; set; }
        public ApiBusinessAdmin Admin { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public ApiBusinessPaymentOptions Payment { get; private set; }


        public ExpectedBusiness(string name, string currency, string email)
            : this(name, "tennis", currency, "Bob", "Smith", email, "0900COACHSEEK", "password1", DateTime.UtcNow)
        { }

        public ExpectedBusiness(string name,
                                string sport,
                                string currency,
                                string firstName,
                                string lastName,
                                string email,
                                string phone,
                                string password,
                                DateTime? createdOn = null,
                                string subscription = Constants.SUBSCRIPTION_TRIAL)
            : this(name, 
                   sport, 
                   currency,
                   false,
                   false,
                   null,
                   null,
                   null,
                   firstName, 
                   lastName, 
                   email, 
                   phone, 
                   password, 
                   createdOn)
        { }

        public ExpectedBusiness(string name,
                                string sport,
                                string currency,
                                bool isOnlinePaymentEnabled,
                                bool? forceOnlinePayment,
                                string paymentProvider,
                                string merchantAccountIdentifier,
                                Guid? adminId,
                                string firstName,
                                string lastName,
                                string email,
                                string phone,
                                string password,
                                DateTime? createdOn = null,
                                string subscription = Constants.SUBSCRIPTION_TRIAL)
        {
            Name = name;
            Sport = sport;
            CreatedOn = createdOn ?? DateTime.UtcNow;
            AuthorisedUntil = CreatedOn.AddDays(30);
            SubscriptionPlan = subscription;
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
            CreatedOn = business.CreatedOn;
            AuthorisedUntil = business.AuthorisedUntil;
            SubscriptionPlan = business.SubscriptionPlan; 
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
