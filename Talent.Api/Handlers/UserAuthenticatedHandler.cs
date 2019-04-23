using Talent.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Api.Handlers
{
    public class UserAuthenticatedHandler : IEventHandler<UserAuthenticated>
    {
        public async Task HandleAsync(UserAuthenticated @event)
        {
            Console.WriteLine("Handle async user authenticated handler");
            Console.WriteLine($"User created: {@event.Email}");
            await Task.CompletedTask;
        }
    }
}
