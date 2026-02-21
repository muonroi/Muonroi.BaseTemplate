using Muonroi.MvcShell.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<MUiManifestApiClient>(client =>
{
    var backendUrl = builder.Configuration["MuonroiBackend:BaseUrl"] ?? "http://localhost:5000";
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapGet("/ui/manifest/{userId:guid}", async (Guid userId, MUiManifestApiClient client, CancellationToken ct) =>
{
    var manifest = await client.LoadAsync(userId, ct);
    return manifest is null ? Results.NotFound() : Results.Ok(manifest);
});

app.MapDefaultControllerRoute();
app.Run();
