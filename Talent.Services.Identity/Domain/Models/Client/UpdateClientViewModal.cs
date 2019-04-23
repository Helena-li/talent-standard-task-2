using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Models;

namespace Talent.Services.Identity.Domain.Models.Client
{
    public class UpdateClientViewModal
    {
        
        public string ClientId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
