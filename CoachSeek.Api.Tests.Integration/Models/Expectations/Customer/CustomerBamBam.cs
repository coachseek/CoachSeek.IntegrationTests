namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Customer
{
    public class CustomerBamBam : ExpectedCustomer
    {
        public override string FirstName { get { return "Bam Bam"; } }
        public override string LastName { get { return "Rubble"; } }


        public CustomerBamBam()
            : base(Random.RandomEmail, Random.RandomString)
        { }
    }
}
