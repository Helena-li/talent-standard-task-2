using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Aws
{
    public static class Extensions
    {
        public static void AddAws(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new AwsOptions();
            var section = configuration.GetSection("aws");
            section.Bind(options);
            services.Configure<AwsOptions>(configuration.GetSection("aws"));
            services.AddSingleton<IAwsService, AwsService>();
        }
    }
}
