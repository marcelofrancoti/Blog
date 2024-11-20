using Blog.Aplication.Postagens.Interface;
using Blog.Intrastruture.Services.Interface;

namespace Blog.API.Util
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrando Stores
            services.Scan(scan => scan
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Store")))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }
    }
}
