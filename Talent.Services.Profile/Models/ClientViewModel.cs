using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Models;

namespace Talent.Services.Profile.Models
{
    public class ClientViewModel
    {
        public String EmployerId { get; set; }
        public String CompanyName { get; set; }
        public DateTime CreatedOn { get; set; }
        public InvitationStatus InvitationStatus { get; set; }
        public int TalentCount { get;set; }
        //talent count get it from employer table 
    }

}
