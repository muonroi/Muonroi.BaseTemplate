using Muonroi.MvcShell.Models;

namespace Muonroi.MvcShell.Services;

public sealed class MUiManifestApiClient(HttpClient httpClient)
{
    public async Task<MUiManifest?> LoadAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<MUiManifest>($"/api/v1/auth/ui-manifest/{userId}", cancellationToken);
    }
}
