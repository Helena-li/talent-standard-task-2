using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talent.Services.Profile.Models.Profile
{
    public class SkillPersonListViewModel
    {
        public string PersonSkillId { get; set; }
        public string Skill { get; set; }
        public string ExperienceLevel { get; set; }
    }

    public class AddSkillViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
    }
}