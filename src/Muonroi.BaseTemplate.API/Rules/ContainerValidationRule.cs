namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("containers")]
public sealed class ContainerValidationRule(IContainerValidationApiClient apiClient) : IRule<CreateContainerCommand>
{
    private readonly IContainerValidationApiClient _apiClient = apiClient;

    public string Name => "ContainerValidation";
    public IEnumerable<Type> Dependencies => [typeof(ContainerExistenceRule)];

    public string Code => nameof(ContainerValidationRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => [nameof(ContainerExistenceRule)];
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(CreateContainerCommand context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task<RuleResult> EvaluateAsync(CreateContainerCommand context, FactBag facts, CancellationToken cancellationToken = default)
    {
        ContainerValidationResult result = await _apiClient.ValidateAsync(context.Code, cancellationToken).ConfigureAwait(false);
        if (!result.IsValid)
        {
            return RuleResult.Failure("Container validation failed.");
        }
        facts["validation"] = result;
        return RuleResult.Passed();
    }
}
