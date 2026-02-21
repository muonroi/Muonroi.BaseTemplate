using Muonroi.Ui.Engine.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<MUiEngineApiClient>(client =>
{
    var backendUrl = builder.Configuration["MuonroiBackend:BaseUrl"] ?? "http://localhost:5000";
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapGet("/ui/engine/{userId:guid}", async (Guid userId, MUiEngineApiClient client, CancellationToken ct) =>
{
    var manifest = await client.MLoadByUserIdAsync(userId, ct);
    return manifest is null ? Results.NotFound() : Results.Ok(manifest);
});

app.MapGet("/ui/engine/current", async (MUiEngineApiClient client, CancellationToken ct) =>
{
    var manifest = await client.MLoadCurrentAsync(ct);
    return manifest is null ? Results.NotFound() : Results.Ok(manifest);
});

app.MapDefaultControllerRoute();
app.Run();
