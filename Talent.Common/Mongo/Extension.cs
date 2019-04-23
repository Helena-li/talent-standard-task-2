using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Mongo
{
    public static class Extension
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOption>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOption>>();

                return new MongoClient(options.Value.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoOption>>();
                var client = c.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
