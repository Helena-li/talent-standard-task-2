using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Profile.Models.Profile
{
    public class ExperienceViewModel
    {
        public String Id { get; set; }
        public String Company { get; set; }
        public String Position { get; set; }
        public String Responsibilities { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
