using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConnectingApp.API.Data;
using ConnectingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ConnectingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        private IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // to support quick reload on views on changing html
            services.AddControllersWithViews();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // to add dbcontext
            services.AddDbContext<DataContext>(cfg => cfg.UseSqlServer(_config.GetConnectionString("ConnectingAppConnectionString")));

            // to resolve cors problem
            services.AddCors();

            // to add repository
            services.AddScoped<IAuthRepository, AuthRepository>();

            // to tell efcore that we are using jwt bearer token as authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // to add global exception handler so that in production mode we are not sending developer exception
                // page back to user
                app.UseExceptionHandler(builder => {
                    builder.Run(async ctx => {
                        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = ctx.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            // here we also need to add info in header so we use helper in which we
                            // set the error message in the appropriate header like
                            // 1: Application-Header
                            // 2: Access-Control-Expose-Headers
                            // 3: Access-Control-Allow-Origin
                            ctx.Response.AddApplicationError(error.Error.Message);
                            await ctx.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();

            // to resolve cors
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // to add authentication middleware
            app.UseAuthentication();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
