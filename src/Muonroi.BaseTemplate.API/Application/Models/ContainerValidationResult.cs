namespace Muonroi.BaseTemplate.API.Application.Models;

public sealed class ContainerValidationResult
{
    public bool HasLeftPort { get; init; }
    public bool IsExpired { get; init; }
    public bool IsValid => !HasLeftPort && !IsExpired;
}
