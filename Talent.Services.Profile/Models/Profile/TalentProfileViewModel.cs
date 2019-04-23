using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Models;

namespace Talent.Services.Profile.Models.Profile
{
    public class TalentProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public bool IsMobilePhoneVerified { get; set; }

        public Address Address { get; set; }
        public string Nationality { get; set; }
        public string VisaStatus { get; set; }
        public DateTime? VisaExpiryDate { get; set; } 
        public string ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }

        public string VideoName { get; set; }
        public string VideoUrl { get; set; }
        public string CvName { get; set; }
        public string CvUrl { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public LinkedAccounts LinkedAccounts { get; set; }
        public JobSeekingStatus JobSeekingStatus { get; set; }

        public List<AddLanguageViewModel> Languages { get; set; }
        public List<AddSkillViewModel> Skills { get; set; }
        public List<AddEducationViewModel> Education { get; set; }
        public List<AddCertificationViewModel> Certifications { get; set; }
        public List<ExperienceViewModel> Experience { get; set; }
    }
}
