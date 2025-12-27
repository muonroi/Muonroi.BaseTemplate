namespace Muonroi.BaseTemplate.API.Infrastructures;

public static class StartupExtensions
{
    public static void AddBaseTemplateServices(this IServiceCollection services,
        WebApplicationBuilder builder, Assembly assembly)
    {
        var configuration = builder.Configuration;

        // ===== CORE SERVICES =====
        _ = services.AddApplication(assembly);
        _ = services.AddInfrastructure(configuration);

        // ===== PERMISSION & AUTHORIZATION =====
        services.AddSingleton<MUONROI.Permissioning.AppPermissionProvider>();
        services.AddSingleton<IPermissionProvider>(sp =>
            sp.GetRequiredService<MUONROI.Permissioning.AppPermissionProvider>());
        _ = services.AddPermissionFilter<Permission>();
        _ = services.AddDynamicPermission<BaseTemplateDbContext>();

        // ===== API CONFIGURATION =====
        _ = services.RegisterServices(configuration);
        _ = services.SwaggerConfig(builder.Environment.ApplicationName);
        _ = services.AddScopeServices(typeof(BaseTemplateDbContext).Assembly);

        // ===== AUTHENTICATION & TOKEN =====
        _ = services.AddValidateBearerToken<BaseTemplateDbContext, MTokenInfo, Permission>(configuration);

        // ===== DATABASE =====
        _ = services.AddDbContextConfigure<BaseTemplateDbContext, Permission>(configuration);

        // ===== CACHING =====
        // Caching is configured via AddInfrastructure() based on CacheConfigs section
        // Supports: Memory, Redis, MultiLevel (see appsettings.Example.json)

        // ===== MULTI-TENANCY =====
        _ = services.AddTenantContext(configuration);

        // ===== CORS =====
        _ = services.AddCors(configuration);

        // ===== RULES ENGINE =====
        _ = services.AddSingleton<IRuleSetStore>(_ =>
            new FileRuleSetStore(Path.Combine(AppContext.BaseDirectory, "rules")));
        _ = services.AddSingleton<RulesEngineService>();

        // ===== HEALTH CHECKS =====
        // Health checks are registered via AddInfrastructure() and exposed at /health via ConfigureEndpoints()
        // MessageBus adds Kafka/RabbitMQ health checks when enabled

        // ===== OPTIONAL FEATURES (controlled by FeatureFlags) =====
        var useGrpc = configuration.GetValue("FeatureFlags:UseGrpc", false);
        var useServiceDiscovery = configuration.GetValue("FeatureFlags:UseServiceDiscovery", false);
        var useMessageBus = configuration.GetValue("FeatureFlags:UseMessageBus", false);
        var useBackgroundJobs = configuration.GetValue("FeatureFlags:UseBackgroundJobs", false);

        // gRPC: Inter-service communication
        if (useGrpc) services.AddGrpcServer();

        // Service Discovery: Consul integration for microservices
        if (useServiceDiscovery) _ = services.AddServiceDiscovery(configuration, builder.Environment);

        // Message Bus: RabbitMQ/Kafka via MassTransit
        if (useMessageBus) _ = services.AddMessageBus(configuration, assembly);

        // Background Jobs: Hangfire/Quartz for scheduled tasks
        if (useBackgroundJobs) _ = services.AddBackgroundJobs(configuration);
    }

    public static async Task UseBaseTemplatePipelineAsync(this WebApplication app,
        WebApplicationBuilder builder, Assembly assembly)
    {
        var configuration = builder.Configuration;

        await app.UseServiceDiscoveryAsync(builder.Environment);
        _ = app.UseRouting();
        _ = app.UseCors("MAllowDomains");
        _ = app.UseMiddleware<TenantContextMiddleware>();
        _ = app.UseDefaultMiddleware<BaseTemplateDbContext, Permission>();
        _ = app.AddLocalization(assembly);
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        _ = app.ConfigureEndpoints();

        var ensureCreatedFallback = configuration.GetValue("FeatureFlags:UseEnsureCreatedFallback", false);
        if (ensureCreatedFallback)
        {
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<BaseTemplateDbContext>();
            var anyMigrations = (await ctx.Database.GetAppliedMigrationsAsync()).Any();
            if (!anyMigrations) await ctx.Database.EnsureCreatedAsync();
        }

        _ = app.MigrateDatabase<BaseTemplateDbContext>();
        await Seed.AdminRoleSeeder.SeedAsync<BaseTemplateDbContext>(app.Services);
    }
}