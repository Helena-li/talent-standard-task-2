using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talent.Services.Profile.Models.Profile
{
    public class LanguagePersonListViewModel
    {
        public string PersonLanguageId { get; set; }
        public string Language { get; set; }
        public string LanguageLevel { get; set; }
    }

    public class AddLanguageViewModel
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public string Id { get; set; }
        public string CurrentUserId { get; set; }
    }
}