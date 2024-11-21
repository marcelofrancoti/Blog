namespace Blog.API.Util
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
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
