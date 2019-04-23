using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Models
{
    public class TalentVideo
    {
        public string FullVideoName { get; set; }
        public string DisplayVideoName { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
