using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiServiceKey
    {
        public Guid? id { get; set; }
        public string name { get; set; }

        public ApiServiceKey()
        { }

        public ApiServiceKey(Guid? id)
        {
            this.id = id;
        }
    }
}
