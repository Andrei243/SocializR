using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_UI.Models;
using ASP.NET_Core_UI.Code.Extensions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using RouteJs;

namespace ASP.NET_Core_UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile("appsetings.json", optional: false, reloadOnChange: true);
            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DataAccess.SocializRContext>();
            services.AddScoped<DataAccess.SocializRUnitOfWork>();
            services.AddCurrentUser();
            services.AddBusinessLogic();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            

            services.AddAuthentication("SocializR")
                .AddCookie("SocializR", options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Feed");
                    
                });
            services.AddAuthorization(options =>
            options.AddPolicy("Admin",
            policy => policy.Requirements.Add(new Authorization.RoleRequirement("admin")))
            );

            services.AddScoped<IAuthorizationHandler, Authorization.AdminHandler>();
            Code.Mappers.WebMapper.Run();
            

            //services.AddRouteJs();
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
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Feed}/{action=Index}/{id?}");
            });
        }
    }
}
