namespace Muonroi.BaseTemplate.API.Infrastructures;

public interface IContainerExistenceGrpcClient
{
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken);
}

public sealed class ContainerExistenceGrpcClient : IContainerExistenceGrpcClient
{
    public Task<bool> ExistsAsync(string code, CancellationToken cancellationToken)
    {
        // Sample gRPC call to check container existence
        return Task.FromResult(false);
    }
}
