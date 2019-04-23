using Talent.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Api.Handlers
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        public async Task HandleAsync(UserCreated @event)
        {
            Console.WriteLine($"{@event.Email} created new user successfully");
            await Task.CompletedTask;
        }
    }
}
