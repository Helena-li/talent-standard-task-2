using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Api.Domain.Models
{
    public class TalentSnapshotMobileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoId { get; set; }
        public string Summary { get; set; }
        public string CurrentEmployment { get; set; }
        public string Visa { get; set; }
        public string Level { get; set; }
        public List<string> Skills { get; set; }
        public string LinkedInUrl { get; set; }
    }
}
