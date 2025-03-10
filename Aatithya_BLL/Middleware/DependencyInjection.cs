using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Aatithya_BLL.AboutUs.Implematiation;
//using Aatithya_BLL.Products.Implematiation;
//using Aatithya_BLL.Products.Interface;
using Aatithya_BLL.Services.Implematiation;
using Aatithya_BLL.Services.Interface;
using Aatithya_DAL.Repository.Implematation;
using Aatithya_DAL.Repository.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace Aatithya_BLL.Middleware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            //services.AddScoped<IEmployeesService, EmployeesService>();
            //services.AddScoped<IOrganizationService, OrganizationService>();
            //services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IUsersService, UsersService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IDepartmentService, DepartmentService>();
            //services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<IBlogServices, BlogsService>();
            //services.AddScoped<IEventsService, EventsService>();
            //services.AddScoped<IAboutUsService, AboutUsService>();
            //services.AddScoped<ICustomerDetailService, CustomerDetailService>();

            //services.AddScoped<IRoleRightsService, RoleRightsService>(); // Manages role rights and permissions.
            return services;
        }
    }
}
