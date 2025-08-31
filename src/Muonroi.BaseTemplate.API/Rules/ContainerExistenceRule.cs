

namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("containers")]
public sealed class ContainerExistenceRule(IContainerExistenceGrpcClient grpcClient) : IRule<CreateContainerCommand>
{
    private readonly IContainerExistenceGrpcClient _grpcClient = grpcClient;

    public string Name => "ContainerExistence";
    public IEnumerable<Type> Dependencies => [];

    public string Code => nameof(ContainerExistenceRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => Array.Empty<string>();
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(CreateContainerCommand context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task<RuleResult> EvaluateAsync(CreateContainerCommand context, FactBag facts, CancellationToken cancellationToken = default)
    {
        bool exists = await _grpcClient.ExistsAsync(context.Code, cancellationToken).ConfigureAwait(false);
        return exists ? RuleResult.Failure("Container already exists.") : RuleResult.Passed();
    }
}
