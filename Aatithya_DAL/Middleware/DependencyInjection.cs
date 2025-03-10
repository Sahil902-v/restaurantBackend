using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_DAL.Repository.Implematation;
using Aatithya_DAL.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Aatithya_DAL.Middleware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();

            //services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            //services.AddScoped<IOrganizationsRepository, OrganizansRepository>();
            //services.AddScoped<IServicesRepository, ServicesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            //services.AddScoped<IRoleRepository, RoleRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IProductsRepository, ProductsRepository>();
            //services.AddScoped<IBlogsRepository, BlogsRepository>();
            //services.AddScoped<IEventsRepository, EventsRepository>();
            //services.AddScoped<IAboutUsRepository, AboutUsRepository>();
            //services.AddScoped<ICustomerDetailRepository, CustomerDetailRepository>();
            //services.AddScoped<IRoleRightsRepository, RoleRightsRepository>();
            return services;
        }
    }
}
