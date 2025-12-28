namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("numbers")]
public sealed class PositiveRule : IRule<int>
{
    public string Name => "Positive";
    public IEnumerable<Type> Dependencies => Array.Empty<Type>();

    public string Code => nameof(PositiveRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => Array.Empty<string>();
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(int context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<RuleResult> EvaluateAsync(int context, FactBag facts, CancellationToken cancellationToken = default)
    {
        bool result = context > 0;
        facts["positive"] = result;
        return Task.FromResult(result ? RuleResult.Passed() : RuleResult.Failure("Number must be positive"));
    }
}
