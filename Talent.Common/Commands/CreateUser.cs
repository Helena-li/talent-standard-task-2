using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Talent.Common.Commands
{
    public class CreateUser: ICommand
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public string UserRole { get; set; }
        [Required]
        public bool TermsConditionsAccepted { get; set; }
    }
}
