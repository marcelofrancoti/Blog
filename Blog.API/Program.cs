using Blog.API.Hubs;
using Blog.API.Util;
using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Intrastruture.Services.IntegrationService;
using Blog.Intrastruture.Services.Interface;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);
//builder.Services.AddScoped<IUsuarioCommandStore, UsuarioCommandStore>();
//builder.Services.AddScoped<IUsuarioQueryStore, UsuarioQueryStore>();
//builder.Services.AddScoped<IPostagemCommandStore, PostagemCommandStore>();
//builder.Services.AddScoped<IPostagemQueryStore, PostagemQueryStore>();
builder.Services.AddApplicationServices();

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSignalR();

    services.AddControllers();

    var assembly = Assembly.GetExecutingAssembly();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ListarPostagemHandler).Assembly));

    services.Scan(scan => scan
        .FromAssemblies(assembly)
        .AddClasses(classes => classes.Where(type => type.Name.EndsWith("CommandStore") || type.Name.EndsWith("QueryStore")))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

    services.AddApplicationServices();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<BlogContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("Blog")));

    services.AddScoped<IDbConnection>(_ =>
    {
        var connectionString = configuration.GetConnectionString("Blog");
        return new NpgsqlConnection(connectionString);
    });
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<PostagemHub>("/hubs/postagem");
    });

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        dbContext.Database.Migrate();
    }

    app.UseAuthorization();

    app.MapControllers();
}
