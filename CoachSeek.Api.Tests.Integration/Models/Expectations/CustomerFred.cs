namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class CustomerFred : ExpectedCustomer
    {
        public override string FirstName { get { return "Fred"; } }
        public override string LastName { get { return "Flintstone"; } }


        public CustomerFred()
            : base(Random.RandomEmail, Random.RandomString)
        { }
    }
}
