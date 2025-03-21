using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_BLL.Services.Implematiation;
using Aatithya_BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Aatithya_BLL.Middleware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.AddScoped<IContact_UsService, Contact_UsService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ILoginService, LoginService>();

            return services;
        }

    }

}
