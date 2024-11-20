using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blog.Aplication
{
    public static class InicializeCommandQuery
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
