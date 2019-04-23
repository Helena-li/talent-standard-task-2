using Talent.Common.Commands;
using Talent.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.MessagePatterns;
using RawRabbit;
using RawRabbit.Instantiation;
using RawRabbit.Pipe;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Talent.Common.RabbitMq
{
    public static class Extension
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
                   ICommandHandler<TCommand> handler) where TCommand : ICommand
                   => bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
                       ctx => ctx.UseConsumerConfiguration(cfg =>
                       cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>()))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(cfg =>
                cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>()))));

        //When we dockerise to different instance, it's important to get the assembly to get accurate queue name
        private static string GetQueueName<T>()
            => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = options
            });
            services.AddSingleton<IBusClient>(_ => client);
        }
    }
}
