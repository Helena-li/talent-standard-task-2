using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Events
{
    public interface IRejectedEvent : IEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}
