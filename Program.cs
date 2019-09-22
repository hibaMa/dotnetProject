using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FirstApp.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FirstApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = CreateWebHostBuilder(args).Build();
             //because we cant inject service in constructor
            using(var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;
                try{

                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate(); // apply migration and creat db if not created insted of the cmd
                    Seed.SeedUsers(context);

                }
                catch(Exception ex){
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "ERROR during migration" );


                }


            }
            host.Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
