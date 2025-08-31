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
    // Register permission provider(s) to seed permission groups & UI keys
    services.AddSingleton<MUONROI.Permissioning.AppPermissionProvider>();
    services.AddSingleton<Muonroi.BuildingBlock.External.Interfaces.IPermissionProvider>(sp => sp.GetRequiredService<MUONROI.Permissioning.AppPermissionProvider>());
    _ = services.RegisterServices(configuration);
    _ = services.SwaggerConfig(builder.Environment.ApplicationName);
    _ = services.AddScopeServices(typeof(BaseTemplateDbContext).Assembly);
    _ = services.AddValidateBearerToken<BaseTemplateDbContext, MTokenInfo, Permission>(configuration);
    _ = services.AddDbContextConfigure<BaseTemplateDbContext, Permission>(configuration);
    _ = services.AddCors(configuration);
    _ = services.AddPermissionFilter<Permission>();
    _ = services.AddDynamicPermission<BaseTemplateDbContext>();
    _ = services.AddTenantContext(configuration);
    // Feature flags to enable/disable optional subsystems
    bool useGrpc = configuration.GetValue<bool>("FeatureFlags:UseGrpc", true);
    bool useServiceDiscovery = configuration.GetValue<bool>("FeatureFlags:UseServiceDiscovery", true);
    bool useMessageBus = configuration.GetValue<bool>("FeatureFlags:UseMessageBus", true);
    bool useBackgroundJobs = configuration.GetValue<bool>("FeatureFlags:UseBackgroundJobs", false);

    if (useGrpc)
    {
        services.AddGrpcServer();
    }
    if (useServiceDiscovery)
    {
        _ = services.AddServiceDiscovery(configuration, builder.Environment);
    }
    if (useMessageBus)
    {
        _ = services.AddMessageBus(configuration, assembly);
    }
    if (useBackgroundJobs)
    {
        _ = services.AddBackgroundJobs(configuration);
    }
    WebApplication app = builder.Build();
    await app.UseServiceDiscoveryAsync(builder.Environment);
    _ = app.UseCors("MAllowDomains");
    _ = app.UseMiddleware<TenantContextMiddleware>();
    _ = app.UseDefaultMiddleware<BaseTemplateDbContext, Permission>();
    _ = app.AddLocalization(assembly);
    _ = app.UseRouting();
    _ = app.UseAuthentication();
    _ = app.UseAuthorization();
    _ = app.ConfigureEndpoints();
    bool ensureCreatedFallback = configuration.GetValue<bool>("FeatureFlags:UseEnsureCreatedFallback", false);
    if (ensureCreatedFallback)
    {
        using IServiceScope scope = app.Services.CreateScope();
        BaseTemplateDbContext ctx = scope.ServiceProvider.GetRequiredService<BaseTemplateDbContext>();

        IMigrator migrator = ctx.Database.GetService<IMigrator>();
        IReadOnlyDictionary<string, TypeInfo> migrations = ctx.Database.GetService<IMigrationsAssembly>().Migrations;
        bool anyMigrations = migrations != null && migrations.Count > 0;
        if (!anyMigrations)
        {
            await ctx.Database.EnsureCreatedAsync();
        }
    }


    _ = app.MigrateDatabase<BaseTemplateDbContext>();
    await Muonroi.BaseTemplate.API.Seed.AdminRoleSeeder.SeedAsync<BaseTemplateDbContext>(app.Services);

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
