namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class RandomBusiness : ExpectedBusiness
    {
        public RandomBusiness() 
            : base(Random.RandomString, Random.RandomEmail)
        { }
    }
}
