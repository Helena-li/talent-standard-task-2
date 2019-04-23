using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Models
{
    public class SearchCompanyModel
    {
        public string Name { get; set; }//company name
        public Location Location { get; set; }
    }

    public class SearchJobModel
    {
        public string Name { get; set; }//Job title
        public Location Location { get; set; }
        public JobCategory Category { get; set; }
        public string Position { get; set; }//search title/description/summary
    }

    public class SearchTalentModel
    {
        public Location Location {get;set;}
        public string Name { get; set; }
        public string Visa { get; set; }
        public string Position { get; set; }
        public string Skill { get; set; }
    }
}
