using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class EmailTemplateData
    {
        public string type { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public IList<string> placeholders { get; set; }
    }
}
