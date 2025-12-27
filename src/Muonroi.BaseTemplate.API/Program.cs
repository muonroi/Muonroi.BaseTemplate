var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

builder.AddAppConfiguration();
builder.AddAutofacConfiguration();
builder.ConfigureSerilog();

Log.Information("Starting {ApplicationName} API up", builder.Environment.ApplicationName);

try
{
    var services = builder.Services;
    services.AddBaseTemplateServices(builder, assembly);

    var app = builder.Build();
    await app.UseBaseTemplatePipelineAsync(builder, assembly);
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