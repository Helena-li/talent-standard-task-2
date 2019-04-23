using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Events
{
    public interface IAuthenticatedEvent : IEvent
    {
        Guid PersonId { get; set; }
    }
}
