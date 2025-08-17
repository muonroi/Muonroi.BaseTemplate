namespace Muonroi.BaseTemplate.API.Services
{
    public static class RegisterService
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRuleEngine()
                .AddRulesFromAssemblies(typeof(RegisterService).Assembly);
            return services;
        }
    }

}
