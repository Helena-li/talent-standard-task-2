using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Identity.Domain.Models
{
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
    }
    public class VerifyResetToken
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
