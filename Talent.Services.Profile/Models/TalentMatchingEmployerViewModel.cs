using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Models;
using Talent.Services.Profile.Models.Profile;

namespace Talent.Services.Profile.Models
{
    public class TalentMatchingEmployerViewModel
    {
        public string Id { get; set; }
        public ContactDetail CompanyContact { get; set; }
        public ContactDetail PrimaryContact { get; set; }
        public string ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public List<AddSkillViewModel> Skills { get; set; }

        public bool DisplayProfile { get; set; }
    }
}
