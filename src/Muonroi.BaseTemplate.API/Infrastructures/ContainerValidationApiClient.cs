namespace Muonroi.BaseTemplate.API.Infrastructures;

public interface IContainerValidationApiClient
{
    Task<ContainerValidationResult> ValidateAsync(string code, CancellationToken cancellationToken);
}

public sealed class ContainerValidationApiClient : IContainerValidationApiClient
{
    public Task<ContainerValidationResult> ValidateAsync(string code, CancellationToken cancellationToken)
    {
        // Sample REST call to validate container
        return Task.FromResult(new ContainerValidationResult());
    }
}
