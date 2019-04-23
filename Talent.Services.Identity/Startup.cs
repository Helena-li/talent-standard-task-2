using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Talent.Common.Auth;
using Talent.Common.Commands;
using Talent.Common.Contracts;
using Talent.Common.Mongo;
using Talent.Common.RabbitMq;
using Talent.Common.Security;
using Talent.Common.Services;
using Talent.Services.Identity.Domain.Services;
using Talent.Services.Identity.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Talent.Services.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowWebApp", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                    = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            });
            services.AddLogging();
            services.AddJwt(Configuration);
            services.AddMongoDB(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddScoped<ICommandHandler<AuthenticateUser>, AuthenticateUserHandler>();
            services.AddScoped<ICommandHandler<CreateUser>, CreateUserHandler>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Func<IServiceProvider, IPrincipal> getPrincipal =
                     (sp) => sp.GetService<IHttpContextAccessor>().HttpContext.User;
            services.AddScoped(typeof(Func<IPrincipal>), sp => {
                Func<IPrincipal> func = () => {
                    return getPrincipal(sp);
                };
                return func;
            });
            //services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserAppContext, UserAppContext>();
            services.AddSingleton<IPasswordStorage, PasswordStorage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowWebApp");
            app.UseMvc();
        }
    }
}
