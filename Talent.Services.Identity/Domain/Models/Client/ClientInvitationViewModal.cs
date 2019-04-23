using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Identity.Domain.Models.Client
{
    public class ClientInvitationViewModal
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string ConfirmedEmail { get; set; }
        [Required]
        public String RecruitersMessage { get; set; }
    }
}
