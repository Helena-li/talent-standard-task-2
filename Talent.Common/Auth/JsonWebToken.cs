using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Auth
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string UserRole { get; set; }
    }
}
