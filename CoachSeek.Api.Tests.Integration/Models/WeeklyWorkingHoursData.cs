using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class WeeklyWorkingHoursData
    {
        public DailyWorkingHoursData monday { get; set; }
        public DailyWorkingHoursData tuesday { get; set; }
        public DailyWorkingHoursData wednesday { get; set; }
        public DailyWorkingHoursData thursday { get; set; }
        public DailyWorkingHoursData friday { get; set; }
        public DailyWorkingHoursData saturday { get; set; }
        public DailyWorkingHoursData sunday { get; set; }
    }
}
