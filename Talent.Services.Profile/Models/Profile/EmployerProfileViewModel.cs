using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Models;

namespace Talent.Services.Profile.Models.Profile
{
    public class EmployerProfileViewModel
    {
        public string Id { get; set; }
        public ContactDetail CompanyContact { get; set; }
        public ContactDetail PrimaryContact { get; set; }
        public string ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }

        public string VideoName { get; set; }
        public string VideoUrl { get; set; }

        public List<AddSkillViewModel> Skills { get; set; }

        public bool DisplayProfile { get; set; }

    }
}
