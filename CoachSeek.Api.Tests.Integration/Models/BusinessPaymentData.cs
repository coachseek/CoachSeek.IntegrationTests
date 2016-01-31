namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessPaymentData
    {
        public string currency { get; set; }
        public bool isOnlinePaymentEnabled { get; set; }
        public bool forceOnlinePayment { get; set; }
        public string paymentProvider { get; set; }
        public string merchantAccountIdentifier { get; set; }
        public bool useProRataPricing { get; set; }
    }
}
