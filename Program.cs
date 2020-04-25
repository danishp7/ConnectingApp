using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectingApp.API.Data;
using ConnectingApp.API.Data.SeedData;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConnectingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // we'll build it and run after the seeding
            var host = CreateWebHostBuilder(args).Build();

            // first we need to create the scope
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // to get the context
                    var ctx = services.GetRequiredService<DataContext>();

                    // it will add migrations to the database
                    ctx.Database.Migrate();

                    // now call seeduser method and pass the context in it
                    Seed.SeedUser(ctx);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "an error occured during migratuons");
                }
            }

            // now we'll run the host
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
