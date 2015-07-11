using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiCoachKey
    {
        public Guid? id { get; set; }
        public string name { get; set; }

        public ApiCoachKey()
        { }

        public ApiCoachKey(Guid? id)
        {
            this.id = id;
        }
    }
}
