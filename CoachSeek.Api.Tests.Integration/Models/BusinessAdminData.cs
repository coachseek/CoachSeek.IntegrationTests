using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessAdminData
    {
        public Guid id { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }
    }
}
