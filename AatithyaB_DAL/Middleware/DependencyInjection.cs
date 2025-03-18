using AatithyaB_DAL.Repository.Implemantation;
using AatithyaB_DAL.Repository.Interface;

namespace AatithyaB_DAL.Middleware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IContact_UsRepository, Contact_UsRepository>();

            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<ILoginRepository, LoginRepository>();

            return services;
        }
    }
}
