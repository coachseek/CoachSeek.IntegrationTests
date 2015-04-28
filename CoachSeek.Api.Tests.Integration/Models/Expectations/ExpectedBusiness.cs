using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedBusiness
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }

        public string Name { get; private set; }
        public ApiBusinessAdmin Admin { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }


        public ExpectedBusiness(string name, string email)
        {
            Name = name;
            Admin = new ApiBusinessAdmin
            {
                firstName = "Bob",
                lastName = "Smith",
                email = UserName = email,
                password = Password = "password1"
            };
        }
    }
}
