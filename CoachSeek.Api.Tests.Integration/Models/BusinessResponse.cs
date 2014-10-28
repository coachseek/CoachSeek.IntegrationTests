using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessResponse
    {
        public BusinessData data { get; set; }
        public List<ApplicationError> errors { get; set; }
        public bool isSuccessful { get; set; }
    }
}
