using Microsoft.Extensions.DependencyInjection;
using simple_aspnetcore_react_shared_validation.Services;
using System.Reflection;

namespace simple_aspnetcore_react_shared_validation.Extensions
{
    public static class ValidationDescriptorServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationDescriptorService(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            services.AddTransient<IValidationDescriptorService>(serviceProvider => new ValidationDescriptorService(assembliesToScan, serviceProvider));

            return services;
        }
    }
}
