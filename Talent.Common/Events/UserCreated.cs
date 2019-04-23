using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Events
{
    public class UserCreated : IEvent
    {
        public string Email { get; }
        public string Name { get; }

        protected UserCreated()
        {

        }

        public UserCreated(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}
