using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Identity.Domain.Models
{
    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
