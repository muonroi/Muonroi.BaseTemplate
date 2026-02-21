namespace Muonroi.MvcShell.Models;

public sealed class MUiManifest
{
    public string SchemaVersion { get; set; } = string.Empty;
    public DateTime GeneratedAtUtc { get; set; }
    public Guid UserId { get; set; }
    public string? TenantId { get; set; }
    public List<MUiManifestGroup> Groups { get; set; } = [];
}

public sealed class MUiManifestGroup
{
    public string GroupName { get; set; } = string.Empty;
    public string GroupDisplayName { get; set; } = string.Empty;
    public List<MUiManifestItem> Items { get; set; } = [];
}

public sealed class MUiManifestItem
{
    public string PermissionName { get; set; } = string.Empty;
    public string UiKey { get; set; } = string.Empty;
    public string? ParentUiKey { get; set; }
    public string Type { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public string Route { get; set; } = "/";
    public bool IsPublished { get; set; }
    public bool IsGranted { get; set; }
    public bool IsVisible { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsHidden { get; set; }
    public string? DisabledReason { get; set; }
    public List<MUiManifestItem> Children { get; set; } = [];
}
