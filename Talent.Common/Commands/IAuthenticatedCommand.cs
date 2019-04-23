using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Commands
{
    public interface IAuthenticatedCommand: ICommand
    {
        Guid PersonId { get; set; }
    }
}
