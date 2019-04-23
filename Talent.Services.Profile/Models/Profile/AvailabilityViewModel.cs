using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Profile.Models.Profile
{
    public class AvailabilityViewModel
    {
        [Required]
        public int AvailabilityType { get; set; }
        [Required]
        public int AvailableHours { get; set; }
        [Required]
        public int EarnTarget { get; set; }
    }
}
