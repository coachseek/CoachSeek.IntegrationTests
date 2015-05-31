namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class RandomBusiness : ExpectedBusiness
    {
        public RandomBusiness() 
            : base(Random.RandomString, "USD", Random.RandomEmail)
        { }

        public RandomBusiness(string name, string currency, string email)
            : base(name, currency, email)
        { }
    }
}
