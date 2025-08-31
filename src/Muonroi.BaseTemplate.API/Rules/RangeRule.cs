namespace Muonroi.BaseTemplate.API.Rules;

[RuleGroup("numbers")]
public sealed class RangeRule : IRule<int>
{
    public string Name => "Range";
    public IEnumerable<Type> Dependencies => new[] { typeof(PositiveRule) };

    public string Code => nameof(RangeRule);
    public int Order => 0;
    public IReadOnlyList<string> DependsOn => new[] { nameof(PositiveRule) };
    public HookPoint HookPoint => HookPoint.BeforeRule;
    public RuleType Type => RuleType.Validation;

    public Task ExecuteAsync(int context, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<RuleResult> EvaluateAsync(int context, FactBag facts, CancellationToken cancellationToken = default)
    {
        bool result = context <= 100;
        facts["range"] = result;
        return Task.FromResult(result ? RuleResult.Passed() : RuleResult.Failure("Number must be 100 or less"));
    }
}
