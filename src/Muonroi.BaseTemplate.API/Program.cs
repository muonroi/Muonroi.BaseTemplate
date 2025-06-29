


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Assembly assembly = Assembly.GetExecutingAssembly();
ConfigurationManager configuration = builder.Configuration;

builder.AddAppConfiguration();
builder.AddAutofacConfiguration();
builder.ConfigureSerilog();
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    MSerilogAction.Configure(context, services, loggerConfiguration, false);
});

Log.Information("Starting {ApplicationName} API up", builder.Environment.ApplicationName);

try
{
    IServiceCollection services = builder.Services;

    _ = services.AddApplication(assembly);
    _ = services.AddInfrastructure(configuration);
    _ = services.RegisterServices(configuration);
    _ = services.SwaggerConfig(builder.Environment.ApplicationName);
    _ = services.AddScopeServices(typeof(BaseTemplateDbContext).Assembly);
    _ = services.AddValidateBearerToken<MTokenInfo, Permission>(configuration);
    _ = services.AddDbContextConfigure<BaseTemplateDbContext, Permission>(configuration);
    _ = services.AddCors(configuration);
    _ = services.AddPermissionFilter<Permission>();
    WebApplication app = builder.Build();
    _ = app.UseCors("MAllowDomains");
    _ = app.UseDefaultMiddleware<BaseTemplateDbContext, Permission>();
    _ = app.AddLocalization(assembly);
    _ = app.UseRouting();
    _ = app.UseAuthentication();
    _ = app.UseAuthorization();
    _ = app.ConfigureEndpoints();
    _ = app.MigrateDatabase<BaseTemplateDbContext>();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception: {Message}", ex.Message);
}
finally
{
    Log.Information("Shut down {ApplicationName} complete", builder.Environment.ApplicationName);
    await Log.CloseAndFlushAsync();
}