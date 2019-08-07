using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Code.Extensions
{
    public static class ConfigurationExtensionMethods
    {

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<Services.County.CountyService>();
            services.AddScoped<Services.User.UserAccountService>();
            services.AddScoped<Services.Locality.LocalityService>();
            services.AddScoped<Services.Album.AlbumService>();
            services.AddScoped<Services.Comment.CommentService>();


            return services;
        }

        public static IServiceCollection AddCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(serviceProvider =>
            {
                var contextAccesor = serviceProvider.GetService<IHttpContextAccessor>();
                var context = contextAccesor.HttpContext;
                var mail = context.User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.Email)?.Value ?? string.Empty;
                var userService = serviceProvider.GetService<Services.User.UserAccountService>();
                var user = userService.Get(mail);
                if (user != null)
                    return new CurrentUser(isAuthenticated: true)
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name
                    };
                else return new CurrentUser(isAuthenticated: false);

            });
            return services;

        }

    }
}
