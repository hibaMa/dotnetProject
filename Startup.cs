using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FirstApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using FirstApp.API.helper;
using AutoMapper;

namespace FirstApp.API
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
            services.AddCors();
            services.AddAutoMapper(typeof(UserRepository).Assembly);
            //CloudinarySettings obj values = CloudinarySettings in the jsonn-conf
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt => //for ref looping user-photo
            opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options => {

                Options.TokenValidationParameters = new TokenValidationParameters{

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration.GetSection("AppSitting:Token").Value)),
                ValidateAudience = false,
                ValidateIssuer = false

                };

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
             // app.UseHsts();
             app.UseExceptionHandler(builder => { // re-exicute request
                 builder.Run(async context => { //app middleware to req pipeline 
                     context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                     var error = context.Features.Get<IExceptionHandlerFeature>();
                     if(error!= null){
                         context.Response.addApplicationError(error.Error.Message);
                         await context.Response.WriteAsync(error.Error.Message);
                    
                     }
                 });
             });
            }

            // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
        }
    }

}
