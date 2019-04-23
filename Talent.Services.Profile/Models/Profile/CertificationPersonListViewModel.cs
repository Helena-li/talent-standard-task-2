using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Services.Profile.Models.Profile
{
    public class CertificationPersonListViewModel
    {
        public string Id { get; set; }
        public string CertificationName { get; set; }
        public string CertificationFrom { get; set; }
        public int CertificationYear { get; set; }
    }

    public class AddCertificationViewModel
    {
        public string Id { get; set; }
        public string CertificationName { get; set; }
        public string CertificationFrom { get; set; }
        public int CertificationYear { get; set; }
    }
}
