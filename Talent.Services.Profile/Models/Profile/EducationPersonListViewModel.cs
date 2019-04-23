using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Talent.Services.Profile.Models.Profile
{
    public class EducationPersonListViewModel
    {
        
    }
    public class AddEducationViewModel
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string InstituteName { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public int YearOfGraduation { get; set; }

        public String Id { get; set; }
    }
}