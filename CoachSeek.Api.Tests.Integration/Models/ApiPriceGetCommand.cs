using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiPriceGetCommand
    {
        public IList<ApiSessionKey> sessions { get; set; }
    }
}
