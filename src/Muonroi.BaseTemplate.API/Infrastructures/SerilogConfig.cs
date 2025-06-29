namespace Muonroi.BaseTemplate.API.Infrastructures
{
    public static class SerilogConfig
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            _ = builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                MSerilogAction.Configure(context, services, loggerConfiguration, false);
            });
        }
    }
}
