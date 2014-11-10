using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiSessionSaveCommand
    {
        public Guid? businessId { get; set; }

        public ApiServiceKey service { get; set; }
        public ApiLocationKey location { get; set; }
        public ApiCoachKey coach { get; set; }

        public string startDate { get; set; }
        public string startTime { get; set; }
        public int duration { get; set; }
        public int studentCapacity { get; set; }
        public bool isOnlineBookable { get; set; } // eg. Is private or not
        public string colour { get; set; }
    }
}
