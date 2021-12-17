using AuthenticationService.Helper;
using AuthenticationService.Manager;
using AuthenticationService.Stores;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static UserDefinedBuilder AddUserDefined(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddScoped<ProfileHelper>();

            services.AddScoped<UserManager>(); 
            services.AddScoped<IUserStores, UserStores>();

            services.AddScoped<AdminManager>();
            services.AddScoped<IAdminStores, AdminStores>();
            return new UserDefinedBuilder(services);
        }
    }
    public class UserDefinedBuilder
    {
        public UserDefinedBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        IServiceCollection Services { get; }
    }
}
