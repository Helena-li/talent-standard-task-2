using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using RawRabbit;
using Talent.Common.Commands;
using Talent.Common.Events;
using Talent.Common.RabbitMq;

namespace Talent.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var hostUrl = config["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://0.0.0.0:60880";

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                            .UseConfiguration(config)
                            .UseUrls(hostUrl)
                            .UseStartup<TStartup>()
                            .UseDefaultServiceProvider(options => options.ValidateScopes = false);
            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _bus;
            public HostBuilder(IWebHost webhost)
            {
                _webHost = webhost; 
            }

            public BusBuilder UseRabbitMq()
            {
                _bus = (IBusClient)_webHost.Services.GetService(typeof(IBusClient));

                return new BusBuilder(_webHost, _bus);
            }

            public override ServiceHost Build()
            {
                throw new NotImplementedException();
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _bus;

            public BusBuilder(IWebHost webHost, IBusClient bus)
            {
                _webHost = webHost;
                _bus = bus;
            }

            public BusBuilder SubcribeToCommand<TCommand>() where TCommand : ICommand
            {
                var handler = (ICommandHandler<TCommand>)_webHost.Services
                    .GetService(typeof(ICommandHandler<TCommand>));
                _bus.WithCommandHandlerAsync(handler);
                return this;
            }

            public BusBuilder SubcribeToEvent<TEvent>() where TEvent : IEvent
            {
                var eventType = typeof(TEvent);
                var handler = (IEventHandler<TEvent>)_webHost.Services
                    .GetService(typeof(IEventHandler<TEvent>));
                _bus.WithEventHandlerAsync(handler);
                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }
    }
}
